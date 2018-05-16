using System;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Workflows.Simple;
using Sitecore.Globalization;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Publishing;
using Sitecore.ContentSearch;
using HealthIS.Apps;
using HealthIS.Apps.MVC.Blog;

namespace Blog.WorkflowEmailNotification
{
    public class AutoPublish
    {
        public Database masterDb
        {
            get
            {
                return Sitecore.Data.Database.GetDatabase("master");
            }
        }
        public Database targetDb 
        {
            get
            {
                return Sitecore.Data.Database.GetDatabase("web");
            }
        }

        public void Process(WorkflowPipelineArgs args)
        {
            if (args.ProcessorItem == null) return;
            PublishPostItem(args.DataItem);
        }

        //public void PublishAuthorProfile(Item dataItem)
        //{
        //    // Match Created By field
        //    string createdBy = dataItem.Statistics.CreatedBy.Replace("\\", "");
        //    Item getUserProfileItem = BlogSettings.AuthorProfileLibrary.Axes.GetDescendants().Where(c => c.Name.Equals(createdBy)).FirstOrDefault();
        //    Item itemInWeb = targetDb.GetItem(getUserProfileItem.ID);
        //    if (getUserProfileItem != null && (itemInWeb == null || getUserProfileItem.Statistics.Revision != itemInWeb.Statistics.Revision))
        //    {
        //        Sitecore.Publishing.PublishOptions publishOptions = new Sitecore.Publishing.PublishOptions(masterDb,
        //            targetDb,
        //            Sitecore.Publishing.PublishMode.SingleItem,
        //            getUserProfileItem.Language,
        //            System.DateTime.Now);

        //        Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
        //        publisher.Options.RootItem = getUserProfileItem;
        //        publisher.Options.Deep = false;
        //        publisher.Options.PublishRelatedItems = true;
        //        publisher.Publish();
        //    }
        //}

        public void PublishPostItem(Item dataItem)
        {
            Item item = dataItem;

            // Add new version / Update Workflow / Keep inital created date for RSS Feed Sort
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                if (Sitecore.Data.Database.GetDatabase("web").GetItem(item.ID) != null)
                {
                    Sitecore.Workflows.IWorkflow workflow = item.Database.WorkflowProvider.GetWorkflow(item);
                    Sitecore.Workflows.WorkflowState newState = workflow.GetStates().FirstOrDefault(state => state.FinalState == true);
                    Sitecore.Data.Fields.DateField pubDateTime = item.Fields["pubDate"];

                    string originalAuthor = dataItem.Fields["__Created by"].Value;

                    item = dataItem.Versions.AddVersion();
                    item.Editing.BeginEdit();
                    item.Fields["__Workflow state"].Value = newState.StateID.ToString();
                    item.Fields["__Created"].Value = pubDateTime.Value;
                    item.Fields["__Created by"].Value = originalAuthor;
                    item.Locking.Unlock();
                    item.Editing.EndEdit();
                }
            }

            //// Clearing master db cache
            //ClearCaches.ClearDatabaseCache(masterDb, item);

            // Author Profile - Republish
            if (BlogSettings.AuthorProfileLibrary != null)
            {
                PublihsingMethod(BlogSettings.AuthorProfileLibrary, Sitecore.Publishing.PublishMode.SingleItem, true, true);
            }

            // Media Library - Smart Publish
            Item imageLibrary = masterDb.GetItem(BlogSettings.ImageLibrary);
            if (imageLibrary != null) 
            {
                PublihsingMethod(imageLibrary, Sitecore.Publishing.PublishMode.Smart, true);
            }
            
            // Tag Repository - Smart Publish
            if (BlogSettings.TagRepository != null)
            {
                PublihsingMethod(BlogSettings.TagRepository, Sitecore.Publishing.PublishMode.Smart, true);
            }

            // Post - Republish
            if (item != null)
            {
                PublihsingMethod(item, Sitecore.Publishing.PublishMode.SingleItem, false);
            }

            //// Clearing web db cache
            //ClearCaches.ClearDatabaseCache(targetDb, item);

            // Refresh the new post for search index web
            if (Sitecore.Data.Database.GetDatabase("web").GetItem(item.ID) != null)
            {
                var tempItem = (SitecoreIndexableItem)Sitecore.Data.Database.GetDatabase("web").GetItem(item.ID);
                // Replace master with web
                string searchIndexWeb = BlogSettings.SearchIndexName.Replace("master", "web");
                Sitecore.ContentSearch.ContentSearchManager.GetIndex(searchIndexWeb).Refresh(tempItem);
                Sitecore.Diagnostics.Log.Audit("Search Indexing: " + searchIndexWeb, this);
            }
        }

        public void PublihsingMethod(Item rootItem, Sitecore.Publishing.PublishMode publishMode, bool deepOption, bool relatedItem = false)
        {
            Sitecore.Publishing.PublishOptions publishOptions = new Sitecore.Publishing.PublishOptions(masterDb,
                    targetDb,
                    publishMode,
                    rootItem.Language,
                    System.DateTime.Now);

            Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
            publisher.Options.RootItem = rootItem;
            publisher.Options.Deep = deepOption;
            publisher.Options.PublishRelatedItems = relatedItem;
            publisher.Publish();
            Sitecore.Diagnostics.Log.Audit("Blog - Published item - " + rootItem.Paths.FullPath + " (Publish Subitems: " + deepOption.ToString() + ")", this);
        }
    }
}