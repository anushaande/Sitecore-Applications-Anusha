using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Web;

namespace HealthIS.Layouts.Models
{
    public class Master
    {
        public Item currentItem = PageContext.Current.Item;

        // BEGIN - Main Global Field Values - Master Data Template
        public string PageItemName
        {
            get { return currentItem["Page Title"]; }
        }
        public string MetaKeywords
        {
            get { return currentItem["Meta Keywords"]; }
        }
        public string MetaTitle
        {
            get { return currentItem["Meta Title"]; }
        }
        public string MetaDescription
        {
            get { return currentItem["Meta Description"]; }
        }
        public string TemplateStylesPath
        {
            get { return currentItem["Template Styles Path"]; }
        }
        public string TemplateScriptsPath
        {
            get { return currentItem["Template Scripts Path"]; }
        }
        public string CollegeStylesPath
        {
            get { return currentItem["College Styles Path"]; }
        }
        public string CollegeScriptsPath
        {
            get { return currentItem["College Scripts Path"]; }
        }
        // END - Main Global Field Values - Master Data Template


        // BEGIN - Custom Field Values - Department-Master Data Template
        public string CustomScriptsPath
        {
            get { return currentItem["Custom Scripts Path"]; }
        }
        public string CustomStylesPath
        {
            get { return currentItem["Custom Styles Path"]; }
        }
        public string CustomHead
        {
            get { return currentItem["Custom Head"]; }
        }
        public string CustomFooter
        {
            get { return currentItem["Custom Footer"]; }
        }
        // END - Custom Field Values - Department-Master Data Template

        // Update Meta Title
        public string UpdateMetaTitle()
        {
            if (!String.IsNullOrEmpty(currentItem.Fields["__Display name"].Value) && String.IsNullOrEmpty(currentItem.Fields["Meta Title"].Value))
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    currentItem.Editing.BeginEdit();
                    currentItem.Fields["Meta Title"].Value = currentItem.Fields["__Display name"].Value;
                    currentItem.Editing.EndEdit();
                }
            }
            
            if (currentItem.Fields["__Display name"].Value != currentItem.Fields["Meta Title"].Value)
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    currentItem.Editing.BeginEdit();
                    currentItem.Fields["Meta Title"].Value = currentItem.Fields["__Display name"].Value;
                    currentItem.Editing.EndEdit();
                }
            }
            return "";
        }

        // For Google Analytics Tracking
        public string cookiePath = string.Empty;
        public bool UseGoogleAnalyticsTracking()
        {
            string itemPath = currentItem.Paths.Path;
            if (itemPath.StartsWith("/sitecore/content/Home/is"))
            {
                cookiePath = "/is";
                return true;
            }
            else if (itemPath.StartsWith("/sitecore/content/Home/nursing"))
            {
                cookiePath = "/nursing";
                return true;
            }
            else if (itemPath.StartsWith("/sitecore/content/Home/medicine"))
            {
                cookiePath = "/medicine";
                return true;
            }
            else if (itemPath.StartsWith("/sitecore/content/Home/publichealth"))
            {
                cookiePath = "/publichealth";
                return true;
            }
            return false;
        }
        
        public bool UseGoogleTagManager()
        {
            if (currentItem.Paths.Path.StartsWith("/sitecore/content/Home/shimberg-library")) { return true; }          
            return false;
        }

        public bool UseOpenGraphMetaTags()
        {
            if (currentItem.Paths.Path.StartsWith(HealthIS.Apps.MVC.Blog.BlogSettings.ArticleRootPath.Paths.Path + "/20")) { return true; }
            return false;
        }
    }
}