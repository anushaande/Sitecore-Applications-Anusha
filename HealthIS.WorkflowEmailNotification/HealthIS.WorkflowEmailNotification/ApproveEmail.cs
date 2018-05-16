using System;
using HealthIS.Apps;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows.Simple;
using System.Collections.Generic;
using System.Linq;

namespace HealthIS.WorkflowEmailNotification
{
    public class ApproveEmail
    {   
        // Access workflow process to get info
        public void Process(WorkflowPipelineArgs args)
        {
            ProcessorItem processorItem = Helper.ValidateArgsProcessorItem(args);
            if (processorItem == null) return;

            Item innerItem = processorItem.InnerItem;
            UpdateWorkflowState update = new UpdateWorkflowState();
            User userWhoMadeUpdate = User.FromName(args.DataItem.Statistics.UpdatedBy, true);
            
            // Get "To Addresses" for approval email
            string toAddress = "";
            string listOfAuthorsEmails = Helper.GetToAddressesForApproveRejectEmail(User.Current, args.DataItem.Paths.FullPath);
            if (!String.IsNullOrEmpty(listOfAuthorsEmails))
            {
                toAddress = userWhoMadeUpdate.Profile.Email + ";" + listOfAuthorsEmails;
            }
            // no other author emails found, only send to contributor who made original update
            else { toAddress = userWhoMadeUpdate.Profile.Email; }
                        
            string fullPath = innerItem.Paths.FullPath;
            string fromAddress = User.Current.Profile.Email;
            string subject = Helper.GetText(innerItem, "subject", args);
            string message = Helper.GetText(innerItem, "message", args);            

            Error.Assert(subject.Length > 0, "The 'Subject' field is not specified in the mail action item: " + fullPath);
            Error.Assert(message.Length > 0, "The 'Message' field is not specified in the mail action item: " + fullPath);
            
            // Update main page item and its resource folder
            update.ChangeWorkflowState(args.DataItem, "Approved", true);

            // Find all controls on item
            foreach (string control in update.GetDatasourceValue(args, args.DataItem))
            {
                if (!String.IsNullOrEmpty(control))
                {
                    Item ctrl = args.DataItem.Database.GetItem(control);
                    if (ctrl != null)
                    {
                        update.ChangeWorkflowState(ctrl, "Approved", false);
                    }
                }
            }
            try
            {
                if (!String.IsNullOrEmpty(toAddress.Trim()) && !String.IsNullOrEmpty(fromAddress.Trim()))
                {
                    // Post Slack Message
                    PostSlackMessage.PostMessage(args.DataItem, User.Current, true, args.CommentFields["Comments"].ToString());

                    // Send email if no issue found
                    if (Util.sendEmail(toAddress, subject, message, fromAddress, "", "", true))
                    {
                        if (fromAddress == "support@health.usf.edu") { fromAddress = "User doesn't have email address"; }
                        Sitecore.Diagnostics.Log.Info("Begin info", this);
                        Sitecore.Diagnostics.Log.Info("fromAddress " + fromAddress, this);
                        Sitecore.Diagnostics.Log.Info("toAddress " + toAddress, this);
                        Sitecore.Diagnostics.Log.Info("subject " + subject, this);
                        Sitecore.Diagnostics.Log.Info("message " + message, this);
                        Sitecore.Diagnostics.Log.Info("End info", this);
                        Sitecore.Diagnostics.Log.Info("Email Successfully Sent!!", this);
                    }
                    else
                    {
                        SheerResponse.ShowError("Sending Email Failed. Please try to approve again", "");
                        args.AbortPipeline();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Sending Email Failed - Exception Error", ex, this);
            }
        }

        // Get all publishable items
        public List<Item> getAllPublishableItems(WorkflowPipelineArgs args)
        {
            UpdateWorkflowState update = new UpdateWorkflowState();
            List<Item> getPublishableItems = new List<Item>();

            getPublishableItems.Add(args.DataItem);
            getPublishableItems.Add(args.DataItem.Children.Where(c => c.DisplayName.EndsWith("_Resources")).FirstOrDefault());

            // Find all controls on item
            foreach (string control in update.GetDatasourceValue(args, args.DataItem))
            {
                Item getControlMainDatasource = args.DataItem.Database.GetItem(control);
                if (getControlMainDatasource != null)
                {
                    getPublishableItems.Add(getControlMainDatasource);
                    List<Item> getAllChildren = getControlMainDatasource.Axes.GetDescendants().ToList();
                    // Get all main datasources' children
                    foreach (Item item in getAllChildren)
                    {
                        if (item != null)
                        {
                            getPublishableItems.Add(item);
                        }
                    }
                }
            }
            return getPublishableItems;
        }
    }
}