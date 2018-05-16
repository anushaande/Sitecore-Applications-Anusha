using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Workflows.Simple;
using Sitecore.Globalization;
using System.Collections.Generic;


namespace HealthIS.WorkflowEmailNotification
{
    public class AutoPublish
    {
        public void Process(WorkflowPipelineArgs args)
        {
            ProcessorItem processorItem = Helper.ValidateArgsProcessorItem(args);
            if (processorItem == null) return;

            // Add publishable item in Queue
            ApproveEmail ae = new ApproveEmail();
            Database master = Sitecore.Configuration.Factory.GetDatabase("master");
            List<Item> getDatasourceChildren = ae.getAllPublishableItems(args);
            foreach (Item publishItem in getDatasourceChildren)
            {
                if (publishItem != null)
                {
                    Sitecore.Publishing.PublishManager.AddToPublishQueue(publishItem, ItemUpdateType.Created);
                    //Helper.ClearDatabaseCache(master, publishItem);
                    //Sitecore.Diagnostics.Log.Info("WP IS - Cleared Master DB Cache - " + publishItem.Paths.FullPath, this);
                    //Sitecore.Diagnostics.Log.Info("WP IS - Publishing Queue - " + publishItem.Paths.FullPath + " added in Queue", this);

                    //Database targetDb = Sitecore.Configuration.Factory.GetDatabase("web");
                    //if (targetDb.GetItem(publishItem.ID) != null)
                    //{
                    //    Helper.ClearDatabaseCache(targetDb, publishItem);
                    //    Sitecore.Diagnostics.Log.Info("WP IS - Cleared Web DB Cache - " + publishItem.Paths.FullPath, this);
                    //}
                }
            }

            // Publish all items in Queue
            Database[] targetDBs = new Database[] { Sitecore.Configuration.Factory.GetDatabase("web") };
            Language[] languages = new Language[] { Sitecore.Data.Managers.LanguageManager.GetLanguage("en") };
            Sitecore.Publishing.PublishManager.PublishIncremental(master, targetDBs, languages);
        }
    }
}