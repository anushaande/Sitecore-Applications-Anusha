using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Workflows.Simple;
using Sitecore.Security.Accounts;
using Sitecore.Configuration;
using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Caching;

namespace HealthIS.WorkflowEmailNotification
{
    public class Helper
    {
        // custom user roles in sitecore
        private static string scAuthor = "hscnet\\SC_Role_Author"; // author role
        private static string scContributor = "hscnet\\SC_Role_Contributor"; // contributor role
        private static string scModerator = "hscnet\\SC_Role_Moderator"; // moderator role
        private static string scInternalUser = "hscnet\\SC_Internal_Users"; // for IS/IT dept internal users
        
        // Get special valuables and update with correct data
        public static string GetText(Item commandItem, string field, WorkflowPipelineArgs args)
        {
            string text = commandItem[field];
            if (text.Length > 0)
                return ReplaceVariables(text, args);
            return string.Empty;
        }

        // Replace special valuables in template
        public static string ReplaceVariables(string data, WorkflowPipelineArgs args)
        {
            data = data.Replace("$userFullName$", User.Current.Profile.FullName);
            data = data.Replace("$itemPath$", args.DataItem.Paths.FullPath);
            data = data.Replace("$itemVersion$", args.DataItem.Version.ToString());
            data = data.Replace("$itemUpdatedDate$", DateTime.Now.ToString("g"));
            data = data.Replace("$comment$", args.CommentFields["Comments"].ToString());

            return data;
        }

        // Get list of "To" email addresses for request email
        public static string GetToAddressesForRequestEmail(User currentUser, string itemPath)
        {
            if (currentUser.IsInRole(scContributor)) 
            {
                // currentUser is a contributor, get author emails from contributor's content group, if any
                List<string> myDeptContentGroup = new List<string>();
                string emailList = "";
                UserRoles allUserRoles = currentUser.Roles;

                // Find contributor's content group(s)
                foreach (Role role in allUserRoles)
                {
                    if (role.DisplayName.ToLower().StartsWith("hscnet\\sc_") && role.DisplayName.ToLower().EndsWith("_content"))
                    {
                        myDeptContentGroup.Add(role.DisplayName);
                    }
                }

                // If contributor is in at least one dept content group
                if (myDeptContentGroup.Count > 0)
                {
                    foreach (string dept in myDeptContentGroup)
                    {
                        IEnumerable<User> allMembersOfContentGroup = RolesInRolesManager.GetUsersInRole(Role.FromName(dept), true);
                        foreach (User person in allMembersOfContentGroup)
                        {
                            if (person.IsInRole(scAuthor))
                            {
                                using (new Sitecore.Security.Accounts.UserSwitcher(person))
                                {
                                    if (Sitecore.Configuration.Factory.GetDatabase("master").GetItem(itemPath).Security.CanWrite(person))
                                    {
                                        // authors have WRITE access to the item updated by contributor
                                        if (!String.IsNullOrEmpty(person.Profile.Email)) { emailList += person.Profile.Email + ";"; }
                                    }
                                    else { goto nextGroup; }
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(emailList)) { goto done; }
                    nextGroup: ;
                    }
                }
                done: ;
                return emailList;
            }
            if (currentUser.IsInRole(scAuthor) || currentUser.IsInRole(scModerator) || currentUser.IsInRole(scInternalUser))
            {
                return currentUser.Profile.Email; // currentUser is not a contributor, return their email
            }
            return null;
        }

        // Get list of other author emails from current author's content group, if any
        public static string GetToAddressesForApproveRejectEmail(User currentAuthor, string itemPath)
        {
            if (currentAuthor.IsInRole(scAuthor))
            {
                List<string> myDeptContentGroup = new List<string>();
                string emailList = "";
                string currentAuthorEmail = currentAuthor.Profile.Email;
                UserRoles allUserRoles = currentAuthor.Roles;

                // Find current author's content group(s)
                foreach (Role role in allUserRoles)
                {
                    if (role.DisplayName.ToLower().StartsWith("hscnet\\sc_") && role.DisplayName.ToLower().EndsWith("_content"))
                    {
                        myDeptContentGroup.Add(role.DisplayName);
                    }
                }

                // If author is in at least one dept content group
                if (myDeptContentGroup.Count > 0)
                {
                    foreach (string dept in myDeptContentGroup)
                    {
                        IEnumerable<User> allMembersOfContentGroup = RolesInRolesManager.GetUsersInRole(Role.FromName(dept), true);
                        foreach (User person in allMembersOfContentGroup)
                        {
                            if (person.IsInRole(scAuthor))
                            {
                                using (new Sitecore.Security.Accounts.UserSwitcher(person))
                                {
                                    if (Sitecore.Configuration.Factory.GetDatabase("master").GetItem(itemPath).Security.CanWrite(person))
                                    {
                                        // other authors also have WRITE access to the item originally updated by contributor
                                        if ((!String.IsNullOrEmpty(person.Profile.Email)) && (!person.Profile.Email.Equals(currentAuthorEmail)))
                                        {
                                            emailList += person.Profile.Email + ";";
                                        }
                                    }
                                    else { goto nextGroup; }
                                }
                            }
                        }
                        if (!String.IsNullOrEmpty(emailList)) { goto done; }
                    nextGroup: ;
                    }
                }
                done: ;
                return emailList;
            }
            return null;
        }

        // returns processorItem if WorkflowPipelineArgs input is valid
        public static ProcessorItem ValidateArgsProcessorItem(WorkflowPipelineArgs args)
        {
            // Check the condition before executing
            Assert.ArgumentNotNull((object)args, "args");

            ProcessorItem processorItem = args.ProcessorItem;

            if (processorItem == null)
            {
                return null;
            }    
            else // item in processor
            {
                LayoutItem layoutItem = args.DataItem.Visualization.GetLayout(Sitecore.Context.Device);
                if (layoutItem == null) // item does not have page layout
                {
                    string[] acceptablePaths = { "/sitecore/content/Data Components", "/sitecore/system/Modules/Wildcards/Routes" };
                    if (acceptablePaths.Any(args.DataItem.Paths.Path.Contains))
                    {
                        return processorItem; // valid workflow item, although item has no page layout
                    }
                    else { return null; } // invalid workflow item
                }
                // item has page layout
                return processorItem;
            }
        }

        // add item version
        public static void UpdateItemInformation(Item item, Sitecore.Workflows.WorkflowState newState)
        {
            if (item == null || String.IsNullOrEmpty(item.Fields["__Default workflow"].Value)) { return; }
            try
            {
                Item isIntialVersionInWeb = Sitecore.Configuration.Factory.GetDatabase("web").GetItem(item.ID);
                Sitecore.Workflows.IWorkflow currnetItemWorkflow = item.Database.WorkflowProvider.GetWorkflow(item);
                if (!currnetItemWorkflow.GetState(item).FinalState && item.Fields["__Workflow state"].Value != ID.Parse(newState.StateID).ToString())
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        // Update New Workflow state first
                        item.Editing.BeginEdit();
                        item.Fields["__Workflow state"].Value = ID.Parse(newState.StateID).ToString();
                        item.Locking.Unlock();
                        item.Editing.EndEdit();

                        // If it is final state
                        if (newState.FinalState)
                        {
                            Item updatedItem = item.Versions.AddVersion();
                            updatedItem.Editing.BeginEdit();
                            updatedItem.Fields["__Workflow state"].Value = ID.Parse(newState.StateID).ToString();
                            updatedItem.Locking.Unlock();
                            updatedItem.Editing.EndEdit();
                        }
                    }
                }
                Sitecore.Diagnostics.Log.Audit("HealthIS WP- Finish updating item \"" + item.Paths.FullPath + "\"", item);
            } 
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("HealthIS WP- Error while updating item \"" + item.Paths.FullPath + "\"", ex, item);
            }

            try
            {
                // If it is final state, find any media item in the datasoure and publish
                if (newState.FinalState)
                {
                    foreach (Field field in item.Fields)
                    {
                        // Media Field
                        if (FieldTypeManager.GetField(field) is ImageField && !String.IsNullOrEmpty(field.Value))
                        {
                            ImageField imagePath = field;
                            Item mediaItem = imagePath.MediaItem.Paths.Item;

                            // Publish image item
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {
                                Database source = Sitecore.Configuration.Factory.GetDatabase("master");
                                Database target = Sitecore.Configuration.Factory.GetDatabase("web");
                                var options = new Sitecore.Publishing.PublishOptions(source, target,
                                                                    Sitecore.Publishing.PublishMode.SingleItem, mediaItem.Language,
                                                                    DateTime.Now)
                                {
                                    RootItem = mediaItem,
                                    Deep = false,
                                };
                                var publisher = new Sitecore.Publishing.Publisher(options);
                                publisher.PublishAsync();
                            }
                            Sitecore.Diagnostics.Log.Audit("HealthIS WP- Finish updating media item field \"" + field.Value + "\" in the item \"" + mediaItem.Paths.FullPath + "\"", mediaItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("HealthIS WP- Error while updating media item in \"" + item.Paths.FullPath + "\"", ex, item);
            }
        }

        // Clearing Cache
        public static void ClearDatabaseCache(Database db, Item item)
        {
            // Clear item's Item Cache
            Sitecore.Context.Database.Caches.ItemCache.RemoveItem(item.ID);

            // Clear item's Data Cache
            Sitecore.Context.Database.Caches.DataCache.RemoveItemInformation(item.ID);

            // Clear item's Standard Value Cache
            Sitecore.Context.Database.Caches.StandardValuesCache.RemoveKeysContaining(item.ID.ToString());

            // Clear item's Prefetch Cache
            GetSqlPrefetchCache(item.Database.Name).Remove(item);
        }

        public static Cache GetSqlPrefetchCache(string database)
        {
            return Sitecore.Caching.CacheManager.FindCacheByName("SqlDataProvider - Prefetch data(" + database + ")");
        }
    }
}
