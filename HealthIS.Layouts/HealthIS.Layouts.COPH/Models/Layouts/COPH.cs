using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Web;
using HealthIS.Layouts.Models;
using System;
using Sitecore.SecurityModel;

namespace HealthIS.Layouts.Models
{
    public class COPH
    {
        public Item currentItem = PageContext.Current.Item;
        
        // BEGIN - COPH Master Field Values - COPH Master Template
        public string FooterGmap
        {
            get { return FieldRenderer.Render(currentItem, "Footer-gmap"); }
        }
        public string FooterLinks
        {
            get { return FieldRenderer.Render(currentItem, "Footer-Links"); }
        }
        public string vCardTitle
        {
            get { return FieldRenderer.Render(currentItem, "vCard Title"); }
        }
        public string vCardImagePath
        {
            get { return FieldRenderer.Render(currentItem, "vCard Image Path"); }
        }
        public string vCardImageAlt
        {
            get { return FieldRenderer.Render(currentItem, "vCard Image Alt"); }
        }
        public string vCardAddress
        {
            get { return FieldRenderer.Render(currentItem, "vCard Address"); }
        }
        public string vCardPhoneNumber
        {
            get { return FieldRenderer.Render(currentItem, "vCard Phone Number"); }
        }
        public string vCardFaxNumber
        {
            get { return FieldRenderer.Render(currentItem, "vCard Fax Number"); }
        }
        public string vCardEmail
        {
            get { return FieldRenderer.Render(currentItem, "vCard Email"); }
        }
        // END - COPH Master Field Values - COPH Master Template

        // BEGIN - Headline Tag
        public string headlineOpen(string className) 
        {
            return "<h1 class='" + className + "'>";
        }
        public string headlineClose
        {
            get { return "</h1>"; }
        }    
        // END - Headline Tag

        public HtmlString executeAutosave()
        {
            if (Sitecore.Context.PageMode.IsExperienceEditor)
            {
                return new HtmlString("<script> jQuery(document).ready(function () { autoSave(); }); </script>");
            }
            return new HtmlString("");
        }
    }
}