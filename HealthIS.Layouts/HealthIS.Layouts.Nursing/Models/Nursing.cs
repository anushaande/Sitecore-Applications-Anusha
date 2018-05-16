using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Web;
using HealthIS.Layouts.Models;
using System;
using Sitecore.SecurityModel;

namespace HealthIS.Layouts.Models
{
    public class Nursing
    {
        public Item currentItem = PageContext.Current.Item;

        // BEGIN - Nursing Master Field Values - Nursing Master Template
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
        // END - Nursing Master Field Values - Nursing Master Template

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
    }
}