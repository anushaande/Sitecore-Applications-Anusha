using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Publishing;
using Sitecore.Workflows.Simple;
using Sitecore.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using Sitecore.SecurityModel;

namespace HealthIS.Support
{
    public class ClearCaches
    {
        public void clearCaches(object sender, EventArgs args)
        {
            //List<string> cacheName = new List<string>();
            //cacheName.Add("SqlDataProvider - Prefetch data(web)");
            //cacheName.Add("web[data]");
            //cacheName.Add("web[items]");

            //foreach (var name in cacheName) {
            //    try
            //    {
            //        var targetCache = Sitecore.Caching.CacheManager.GetAllCaches().Where(c => c.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            //        targetCache.First().Clear();
            //        Sitecore.Diagnostics.Log.Info("Cleared cache: " + name, this);
            //    } catch {
            //        Sitecore.Diagnostics.Log.Info("Failed clearing cache: " + name, this);
            //    }
            //}


            // Clear all caches
            Sitecore.Caching.CacheManager.ClearAllCaches();
            Sitecore.Diagnostics.Log.Info("Manually - HealthUSF Support: Cleared All Caches", this);
        }

        //public void workflowProcess(WorkflowPipelineArgs args)
        //{
        //    Sitecore.Workflows.Simple.PublishAction aa = new Sitecore.Workflows.Simple.PublishAction();
        //    Item dataItem = args.DataItem;
        //    Item innerItem = args.ProcessorItem.InnerItem;
        //    //Database[] targets = Enumerable.ToArray<Database>(GetTargets(parameters, innerItem, dataItem));
        //    Language[] language = new Language[] { dataItem.Language };
        //    Handle publishTask = PublishManager.PublishItem(dataItem, null, language, false, false, false);

        //    PublishStatus status;

        //    while ((status = PublishManager.GetStatus(publishTask)) != null && !status.IsDone)
        //    {
        //        System.Threading.Thread.Sleep(500);
        //    }

        //    // Once publishing task is doen, clear all caches
        //    Sitecore.Caching.CacheManager.ClearAllCaches();
        //    Sitecore.Diagnostics.Log.Info("Workflow - HealthUSF Support: Cleared All Caches", this);
        //}
    }
}
