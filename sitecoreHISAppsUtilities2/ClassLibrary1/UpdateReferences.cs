using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using Sitecore.Layouts;
using Sitecore.Web.UI.Sheer;
using Sitecore.Data.Fields;
using System.Collections.Generic;

namespace HealthIS.Apps.CopyItems
{
    class ItemEventHandler
    {
        private Item sourceRoot;
        private Item copyRoot;

        public void UpdateReferences(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            sourceRoot = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;
            Assert.IsNotNull(sourceRoot, "copyRoot is null.");
            copyRoot = Sitecore.Events.Event.ExtractParameter(args, 1) as Item;
            Assert.IsNotNull(copyRoot, "copyRoot is null.");

            Item[] targetPages = GetAllPages();
            if(targetPages == null) { return; }
            
            foreach (Item page in targetPages)
            {
                UpdateDatasourcePaths(page);
            }            
        }

        // update datasource references on target page item
        private void UpdateDatasourcePaths(Item target)
        {
            try
            {
                if (target != null)
                {
                    //Log.Info("target: " + target.ID + " - " + target.Name, this);

                    // get all target's renderings
                    RenderingReference[] renderings = target.Visualization.GetRenderings(Context.Device, true);
                    //Log.Info("Total # target's renderings: " + renderings.Length + ".", this);

                    Item targetResourceFolder = GetResourceFolder(target.Paths.Path);
                    string targetDatasourceID = string.Empty;                    

                    // Get the layout definitions and the device
                    LayoutDefinition sharedLayout = LayoutDefinition.Parse(LayoutField.GetFieldValue(target.Fields[FieldIDs.LayoutField]));
                    LayoutDefinition finalLayout = LayoutDefinition.Parse(LayoutField.GetFieldValue(target.Fields[FieldIDs.FinalLayoutField]));
                    DeviceDefinition device = finalLayout.Devices[0] as DeviceDefinition;

                    //int count = 1;
                    foreach (RenderingReference rend in renderings)
                    {
                        string renderingDatasourceID = rend.Settings.DataSource;
                        if (!String.IsNullOrEmpty(renderingDatasourceID))
                        {
                            //Log.Info("rend[" + count + "] = " + rend.RenderingItem.Name + " = " + rend.RenderingID.ToString(), this);

                            string renderingUID = rend.UniqueId;
                            //Log.Info("rend.UniqueId = " + renderingUID, this);


                            // get target datasource ID
                            if (targetResourceFolder != null && targetResourceFolder.HasChildren)
                            {
                                //Log.Info("targetResourceFolder has " + targetResourceFolder.Children.Count + " children.", this);
                                targetDatasourceID = GetTargetDatasourceID(targetResourceFolder.Paths.Path, renderingDatasourceID);
                            }
                            else
                            {
                                if (ItemIsSourceRootChild(renderingDatasourceID))
                                {
                                    targetDatasourceID = GetTargetDatasourceID(renderingDatasourceID);
                                }
                                else { return; } // datasource exists in area of content tree that was not copied, do not update                                                           
                            }
                            //Log.Info("targetDatasourceID: " + targetDatasourceID, this);

                            // Update rendering datasource value accordingly and verify
                            if (!String.IsNullOrEmpty(targetDatasourceID))
                            {
                                //Log.Info("Datasource should be pointing to : " + targetDatasourceID + " | but it is pointing to: " + renderingDatasourceID, this);
                                device.GetRenderingByUniqueId(renderingUID).Datasource = targetDatasourceID;
                                VerifyReferenceUpdate(target, device.GetRenderingByUniqueId(renderingUID).Datasource); 

                                // Save the layout changes
                                target.Editing.BeginEdit();
                                ItemUtil.SetLayoutDetails(target, sharedLayout.ToXml(), finalLayout.ToXml());
                                target.Editing.EndEdit();
                            }
                        }
                        //count++;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to update item references after CopyTo - " + target.ID.ToString() + " - " + ex.Message, ex, this);
            }
        }

        // escape item names that contain dashes for query
        private string EscapeItemNamesWithDashes(string queryPath)
        {
            if (!queryPath.Contains("-"))
                return queryPath;

            var strArray = queryPath.Split(new char[] { '/' });
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].IndexOf('-') > 0)
                    strArray[i] = "#" + strArray[i] + "#";
            }
            return string.Join("/", strArray);
        }

        // get all pages that need their references updated, target and any sub-pages
        private Item[] GetAllPages()
        {
            if (copyRoot != null && IsPageItem(copyRoot))
            {
                List<Item> pages = new List<Item> { copyRoot };
                if (copyRoot.HasChildren)
                {
                    foreach (Item item in copyRoot.Axes.GetDescendants())
                    {
                        if (IsPageItem(item))
                        {
                            pages.Add(item);
                        }
                    }
                    return pages.ToArray();
                }
            }
            return null;
        }

        // check to see if item is a page item (has layout)
        private bool IsPageItem(Item item)
        {
            LayoutItem layout = item.Visualization.GetLayout(Sitecore.Context.Device);
            if (layout == null) // item does not have page layout
            {
                return false;
            }
            else // item has page layout
            {
                return true;
            }
        }

        // get resource folder of specified page item 
        private Item GetResourceFolder(string rootPath)
        {
            if (!String.IsNullOrEmpty(rootPath))
            {
                Item resourceFolder = Factory.GetDatabase("master").GetItem("/sitecore/templates/User Defined2/Component Template/Resources Folder");
                if (resourceFolder != null)
                {
                    string query = EscapeItemNamesWithDashes(rootPath) + "/*[@@templateid='" + resourceFolder.ID.ToString() + "']";
                    Item folder = Factory.GetDatabase("master").SelectSingleItem(query);
                    if (folder != null)
                    {
                        return folder;
                    }
                }
            }
            return null;
        }

        // get the id of the (copied) target datasource
        private string GetTargetDatasourceID(string datasourceID) 
        {
            if (!String.IsNullOrEmpty(datasourceID))
            {
                Item sourceDS = Factory.GetDatabase("master").GetItem(datasourceID);
                if (sourceDS != null)
                {
                    string[] sourceDsPath = sourceDS.Paths.Path.Split(new[] { "/" + sourceRoot.Name + "/" }, StringSplitOptions.None);
                    Item targetDS = Factory.GetDatabase("master").GetItem(copyRoot.Paths.Path + "/" + sourceDsPath[1]);
                    if (targetDS != null)
                    {
                        return targetDS.ID.ToString();
                    }
                }
            }
            return null;
        }

        // get the id of the (copied) target datasource - use when target item has Resources folder with datasource items in it
        private string GetTargetDatasourceID(string targetResourceFolderPath, string datasourceID)
        {
            if (!String.IsNullOrEmpty(targetResourceFolderPath) && !String.IsNullOrEmpty(datasourceID))
            {
                Item sourceDS = Factory.GetDatabase("master").GetItem(datasourceID);
                if (sourceDS != null)
                {
                    Item targetDS = Factory.GetDatabase("master").GetItem(targetResourceFolderPath + "/" + sourceDS.Name);
                    if (targetDS != null)
                    {
                        return targetDS.ID.ToString();
                    }
                    else
                    {
                        if (ItemIsSourceRootChild(datasourceID))
                        {
                            return GetTargetDatasourceID(datasourceID);
                        }                                                                                 
                    }                
                }
            }
            return null;
        }

        // determine if item is child of source root
        private bool ItemIsSourceRootChild(string itemID)
        {
            if (!String.IsNullOrEmpty(itemID))
            {
                Item item = Factory.GetDatabase("master").GetItem(itemID);
                if (item != null && item.Paths.Path.StartsWith(sourceRoot.Paths.Path))
                {
                    return true;
                }
            }
            return false;
        }

        // check to see if reference was updated successfully
        private void VerifyReferenceUpdate(Item target, string datasource)
        {
            if (target != null && !String.IsNullOrEmpty(datasource))
            {
                Item datasourceItem = Factory.GetDatabase("master").GetItem(datasource);
                if (datasourceItem != null)
                {
                    if (!datasourceItem.Paths.Path.StartsWith(copyRoot.Paths.Path))
                    {
                        SheerResponse.Alert("Failed to update item references after CopyTo, please see log file.<br/>Target item ID: " + target.ID.ToString());
                        Log.Error("Failed to update item references after CopyTo: rendering datasource in " + target.ID.ToString() + "'s final layout does not match target path.", this);
                    }
                }
                else
                {
                    SheerResponse.Alert("Failed to update item references after CopyTo, please see log file.<br/>Target item ID: " + target.ID.ToString());
                    Log.Error("Failed to update item references after CopyTo: rendering datasource in " + target.ID.ToString() + "'s final layout is not set or item does not exist.", this);
                }
            }
            else
            {
                SheerResponse.Alert("Failed to update item references after CopyTo, please see log file.<br/>Target item ID: " + target.ID.ToString());
                Log.Error("Failed to update item references after CopyTo: target item "+ target.ID.ToString() + " is null or rendering datasource in final layout is not set.", this);
            }
        }
    }    
}
