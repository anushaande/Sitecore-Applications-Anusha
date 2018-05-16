using System;
using HealthIS.Apps;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows.Simple;
using Sitecore;
using Sitecore.Data.Validators;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Sitecore.Web;
using Sitecore.Text;
using Sitecore.Buckets.Managers;
using System.Collections.Generic;
using Sitecore.ContentSearch;
using HealthIS.Apps.MVC.Blog;

namespace Blog.WorkflowEmailNotification
{
    public class Validation
    {
        // Access workflow process to get info
        public void Process(WorkflowPipelineArgs args)
        {
            Item currentPost = args.DataItem;
            string titleField = FieldValidation(currentPost, "Title");
            string thumbnailImage = FieldValidation(currentPost, "ThumbnailImage");
            string description = FieldValidation(currentPost, "Description");
            string content = FieldValidation(currentPost, "Content");
            string publishedDate = FieldValidation(currentPost, "PubDate");

            // Validate required fields
            if (IsFieldAvailable(currentPost, "Title") && String.IsNullOrEmpty(titleField) ||
                IsFieldAvailable(currentPost, "ThumbnailImage") && String.IsNullOrEmpty(thumbnailImage) ||
                IsFieldAvailable(currentPost, "Description") && String.IsNullOrEmpty(description) ||
                IsFieldAvailable(currentPost, "Content") && String.IsNullOrEmpty(content)) 
            {
                Sitecore.Diagnostics.Log.Audit("Blog WP- One of required fields is empty. Stop submitting the request at " + currentPost.Paths.FullPath, this);
                SheerResponse.Alert("One of required fields is empty");
                args.AbortPipeline();
            }
            else
            {
                Sitecore.Data.Database webDb = Sitecore.Configuration.Factory.GetDatabase("web");

                // if the item has not been published
                if (currentPost != null && webDb.GetItem(currentPost.ID) == null)
                {
                    string sitecoreTime = Sitecore.DateUtil.ToIsoDate(DateTime.Now);

                    // Update publish date
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        currentPost.Editing.BeginEdit();
                        currentPost.Fields["__Display name"].Value = currentPost.DisplayName.Replace("_Draft", "");
                        // Update pubDate field for RSS Feed
                        currentPost.Fields["pubDate"].Value = sitecoreTime;
                        // Update __Created date field for Bucket Sync
                        currentPost.Fields["__Created"].Value = sitecoreTime;
                        currentPost.Editing.EndEdit();

                        // Bucket sync
                        BucketManager.Sync(currentPost.Database.GetItem(BlogSettings.ArticleRootPath.ID));
                    }

                    // Refresh the new post for  search index master
                    var tempItem = (SitecoreIndexableItem)currentPost;
                    string searchIndexMaster = BlogSettings.SearchIndexName;
                    Sitecore.ContentSearch.ContentSearchManager.GetIndex(searchIndexMaster).Refresh(tempItem);
                    Sitecore.Diagnostics.Log.Audit("Search Indexing: " + searchIndexMaster, this);
                }
            }
        }

        public string FieldValidation(Item item, string fieldName)
        {
            string value = String.Empty;
            if (item != null && item.Fields != null && item.Fields[fieldName] != null)
            {
                value = item.Fields[fieldName].Value;
            }
            return value;
        }

        public bool IsFieldAvailable(Item item, string fieldName)
        {
            if (item.Fields != null && item.Fields[fieldName] != null)
            {
                return true;
            }
            return false;
        }
    }
}
