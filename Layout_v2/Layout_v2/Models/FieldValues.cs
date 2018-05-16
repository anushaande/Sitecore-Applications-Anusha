using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web.UI.WebControls;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;

namespace Layout_v2.Models
{
    // Context Page Field Values, Not Rendering Components
    public class FieldValues
    {
        public Item currentItem = PageContext.Current.Item;
        //public Item renderingItem = RenderingContext.Current.ContextItem;

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
        public string PageStylesPath
        {
            get { return currentItem["Page Styles Path"]; }
        }
        public string PageScriptsPath
        {
            get { return currentItem["Page Scripts Path"]; }
        }
        // END - Main Global Field Values - Master Data Template


        // BEGIN - Doctor-Master Field Values - Doctor Master Template
        public string FooterVCard
        {
            get { return FieldRenderer.Render(currentItem, "Footer-vCard"); }
        }
        public string FooterGmap
        {
            get { return FieldRenderer.Render(currentItem, "Footer-gmap"); }
        }
        public string FooterLinks
        {
            get { return FieldRenderer.Render(currentItem, "Footer-Links"); }
        }
        // END - Doctor-Master Field Values - Doctor Master Template
        

        public string PageMainTitle
        {
            get { return FieldRenderer.Render(currentItem, "Page Main-Title"); }
        }
        public string PageSubTitle
        {
            get { return FieldRenderer.Render(currentItem, "Page Sub-Title"); }
        }
        public string Content1
        {
            get { return FieldRenderer.Render(currentItem, "Content1"); }
        }
        public string Content2
        {
            get { return FieldRenderer.Render(currentItem, "Content2"); }
        }
        public string Content3
        {
            get { return FieldRenderer.Render(currentItem, "Content3"); }
        }
        public string AdBox
        {
            get { return FieldRenderer.Render(currentItem, "AdBox"); }
        }
        public string CustomScriptsPath
        {
            get { return currentItem["Custom Scripts Path"]; }
        }
        public string CustomStylesPath
        {
            get { return currentItem["Custom Styles Path"]; }
        }
        public string PatientToolBox
        {
            get { return currentItem["PatientToolBox Phone Number"]; }
        }
    }
}