using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Workflows;
using Sitecore.Workflows.Simple;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthIS.WorkflowEmailNotification
{
    public class UpdateWorkflowState
    {
        // List all controls in page item
        public RenderingReference[] GetListOfSublayouts(string itemId, Item targetItem)
        {
            RenderingReference[] renderings = null;
            
            if (Sitecore.Data.ID.IsID(itemId))
            {
                renderings = targetItem.Visualization.GetRenderings(Sitecore.Context.Device, true);
            }
            return renderings;
        }

        // Return all datasource defined on one item
        public IEnumerable<string> GetDatasourceValue(WorkflowPipelineArgs args, Item targetItem)
        {
            List<string> uniqueDatasourceValues = new List<string>();
            Sitecore.Layouts.RenderingReference[] renderings = GetListOfSublayouts(targetItem.ID.ToString(), targetItem);

            LayoutField layoutField = new LayoutField(targetItem.Fields[Sitecore.FieldIDs.FinalLayoutField]);
            LayoutDefinition layoutDefinition = LayoutDefinition.Parse(layoutField.Value);
            DeviceDefinition deviceDefinition = layoutDefinition.GetDevice(Sitecore.Context.Device.ID.ToString());
            
            foreach (var rendering in renderings)
            {
                if (!uniqueDatasourceValues.Contains(rendering.Settings.DataSource))
                    uniqueDatasourceValues.Add(rendering.Settings.DataSource);
            }         
            return uniqueDatasourceValues;
        }

        // Check workflow state and update state
        public WorkflowResult ChangeWorkflowState(Item item, ID workflowStateId)
        {
            // If the item's workflow is in Approved state (Which means the content of the item (datasource) was not updated)
            if (item.Database.WorkflowProvider.GetWorkflow(item).IsApproved(item, item.Database))
            {
                return new WorkflowResult(false, "I don't update " + item.Name + " Workflow because there is no update");
            }

            Sitecore.Layouts.RenderingReference[] renderings = GetListOfSublayouts(item.ID.ToString(), item);
            return new WorkflowResult(true, "OK", workflowStateId);
        }

        // Verify workflow state and update workflow state
        public WorkflowResult ChangeWorkflowState(Item item, string workflowStateName, bool isPageOrResourceFolder)
        {
            IWorkflow workflow = item.Database.WorkflowProvider.GetWorkflow(item);
            if (workflow == null)
            {
                Sitecore.Diagnostics.Log.Info("HealthIS - " + item.Name + " - No workflow assigned to item", this);
                return new WorkflowResult(false, "No workflow assigned to item");
            }
            
            WorkflowState newState = workflow.GetStates().FirstOrDefault(state => state.DisplayName == workflowStateName);
            if (newState == null)
            {
                Sitecore.Diagnostics.Log.Info("HealthIS - " + item.Name + " - Cannot find workflow state " + workflowStateName, this);
                return new WorkflowResult(false, "Cannot find workflow state " + workflowStateName);
            }

            if (isPageOrResourceFolder)
            {
                Helper.UpdateItemInformation(item, newState);
                Helper.UpdateItemInformation(item.GetChildren().Where(c => c.DisplayName.EndsWith("_Resources")).FirstOrDefault(), newState);
            }
            else
            {
                datasourceUpdate(item, newState);
            }
            
            return ChangeWorkflowState(item, ID.Parse(newState.StateID));
        }

        // The main datasource and its subitem updates
        public void datasourceUpdate(Item item, WorkflowState newState)
        {
            bool canWrite = Sitecore.Data.Database.GetDatabase("master").SelectItems(item.Paths.FullPath)[0].Security.CanWrite(Sitecore.Context.User);
            

            if (item == null || !canWrite) { return; }

            Helper.UpdateItemInformation(item, newState);

            if (item.HasChildren)
            {
                // First child of Datasource
                foreach (Item child1 in item.Children)
                {
                    Helper.UpdateItemInformation(child1, newState);

                    // Second Child
                    if (child1.HasChildren)
                    {
                        foreach (Item child2 in child1.Children)
                        {
                            Helper.UpdateItemInformation(child2, newState);
                            
                            // Third Child
                            if (child2.HasChildren)
                            {
                                foreach (Item child3 in child2.Children)
                                {
                                    Helper.UpdateItemInformation(child3, newState);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}