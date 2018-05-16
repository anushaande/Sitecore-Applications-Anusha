using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthIS.Apps
{
    class ItemEventHandler
    {
        public void OnItemSaving(object sender, EventArgs args)
        {
            //updatedItem.Versions.AddVersion();

            //try
            //{
            //    Sitecore.Events.SitecoreEventArgs eventArgs = args as Sitecore.Events.SitecoreEventArgs;
            //    Sitecore.Diagnostics.Assert.IsNotNull(eventArgs, "eventArgs");
            //    Sitecore.Data.Items.Item updatedItem = eventArgs.Parameters[0] as Sitecore.Data.Items.Item;
            //    Sitecore.Diagnostics.Assert.IsNotNull(updatedItem, "item");
            //    Sitecore.Data.Items.Item existingItem = updatedItem.Database.GetItem(
            //      updatedItem.ID,
            //      updatedItem.Language,
            //      updatedItem.Version);
            //    Sitecore.Diagnostics.Assert.IsNotNull(existingItem, "existingItem");
            //    string existingWFState = "";
            //    string updatedWFState = "";
            //    if (existingItem.Fields[Sitecore.FieldIDs.WorkflowState] != null) { existingWFState = existingItem.Fields[Sitecore.FieldIDs.WorkflowState].Value; }
            //    if (updatedItem.Fields[Sitecore.FieldIDs.WorkflowState] != null) { updatedWFState = updatedItem.Fields[Sitecore.FieldIDs.WorkflowState].Value; }
            //    if (existingWFState == "" && updatedWFState == "" && existingItem.Fields[Sitecore.FieldIDs.Workflow] != null)//WF State not existent || can't be parsed
            //    {
            //        if (existingItem.Fields[Sitecore.FieldIDs.Workflow] != null)
            //        {
            //            if (!String.IsNullOrEmpty(existingItem.Fields[Sitecore.FieldIDs.Workflow].Value))
            //            {

            //                IWorkflow wf = updatedItem.Database.WorkflowProvider.GetWorkflow(existingItem.Fields[Sitecore.FieldIDs.Workflow].Value);
            //                if (wf != null)
            //                {
            //                    string workingState = Util.GetWorkingState(wf);
            //                    if (!string.IsNullOrEmpty(workingState))
            //                    {
            //                        updatedItem.Fields[Sitecore.FieldIDs.WorkflowState].Value = workingState;//set wf state to working
            //                        //Util.LogError("WF State not existent || can't be parsed for " + updatedItem.Name + ". Set WF state to working.", null, this);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else if (existingWFState != "" && updatedWFState != "" && existingWFState != updatedWFState && existingItem.Fields[Sitecore.FieldIDs.Workflow] != null)//Workflow states changed
            //    {
            //        if (!String.IsNullOrEmpty(existingItem.Fields[Sitecore.FieldIDs.Workflow].Value))
            //        {
            //            IWorkflow wf = updatedItem.Database.WorkflowProvider.GetWorkflow(existingItem.Fields[Sitecore.FieldIDs.Workflow].Value);
            //            if (wf != null)//Item being saved is a document(page)
            //            {
            //                string finalWFState = Util.GetFinalWFState(wf);
            //                if (updatedWFState == finalWFState && !string.IsNullOrEmpty(finalWFState))//Updated item is to be in final wf state (approved)
            //                {
            //                    //approve(move to approve state) all components
            //                    bool doOld = true;
            //                    if (doOld)
            //                    {//original way of doing it J LOOK HERE
            //                        foreach (Item child in existingItem.Children)
            //                        {
            //                            if (child.Name == "Resources")
            //                            {
            //                                foreach (Item ds in child.Children)
            //                                {
            //                                    UpdateWFState(ds, updatedWFState);
            //                                }
            //                                break;
            //                            }
            //                        }
            //                    }
            //                    else
            //                    {//New way of doing it, that crashes because of SQL error

            //                        Sitecore.Context.ClientPage.ClientResponse.Alert("Updating wf's");
            //                        foreach (Item related in GetReferrers(existingItem))
            //                        {
            //                            Sitecore.Context.ClientPage.ClientResponse.Alert("Updating WF state to: " + updatedWFState + " for " + related.Name);
            //                            UpdateWFState(related, updatedWFState);
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    //Util.LogError("Updating WF state to: " + updatedWFState + " for " + updatedItem.Name, null, null);
            //                }

            //            }
            //            else //component updated
            //            {
            //                if (updatedWFState == Util.GetWorkingState(wf)) //component is being updated to working state
            //                {
            //                    Item resourcesFolder = GetParentResourcesFolder(updatedItem);
            //                    if (resourcesFolder != null)
            //                    {
            //                        Item pg = updatedItem.Parent.Parent;

            //                        using (new EditContext(pg))
            //                        {
            //                            pg.Fields[Sitecore.FieldIDs.WorkflowState].Value = updatedWFState; //update page item to working state
            //                            //Util.LogError("Setting page " + pg.Name + " wf state to" + updatedWFState, null, null);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Util.LogError("Item Saving Error: " + ex.Message, ex, this);
            //}
        }

        //public Item GetParentResourcesFolder(Item i)
        //{
        //    if (i.Parent == null) { return null; }
        //    if (i.Parent.Name == "Resources") { return i.Parent; }
        //    else { return GetParentResourcesFolder(i); }
        //}

        //public List<Item> GetReferrers(Item item)
        //{
        //    try
        //    {
        //        //Item item = Sitecore.Data.Database.GetDatabase("master").GetItem(new Sitecore.Data.ID(itemId));
        //        // getting all linked Items that refer to the Item
        //        ItemLink[] itemLinks = Sitecore.Globals.LinkDatabase.GetReferrers(item);
        //        if (itemLinks == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            List<Item> items = new List<Item>();
        //            foreach (ItemLink itemLink in itemLinks)
        //            {
        //                // comparing the database name of the linked Item
        //                if (itemLink.SourceDatabaseName == "master")
        //                {
        //                    Item linkItem = Sitecore.Data.Database.GetDatabase("master").Items[itemLink.SourceItemID];
        //                    if (linkItem != null)
        //                    {
        //                        items.Add(linkItem);
        //                    }
        //                }
        //            }
        //            return items;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Sitecore.Context.ClientPage.ClientResponse.Alert(ex.Message);
        //        return null;
        //    }
        //}

        //public void UpdateWFState(Item i, string updatedWFState)
        //{
        //    if (i.Fields[Sitecore.FieldIDs.WorkflowState].Value != updatedWFState)//update if not already in state
        //    {
        //        using (new EditContext(i))
        //        {
        //            i.Fields[Sitecore.FieldIDs.WorkflowState].Value = updatedWFState;
        //            //Util.LogError("Setting component " + i.Name + " wf state to " + updatedWFState, null, null);
        //        }
        //    }

        //    i.Versions.AddVersion(); // Add new version whenever item approved
                

        //    //recursively update wf
        //    if (i.HasChildren)
        //    {
        //        foreach (Item child in i.Children)
        //        {
        //            UpdateWFState(child, updatedWFState);
        //        }
        //    }
            
        //}
    }
}
