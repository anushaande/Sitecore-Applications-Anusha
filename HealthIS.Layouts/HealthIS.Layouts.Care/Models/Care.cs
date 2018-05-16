using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Web;

namespace HealthIS.Layouts.Models
{
    public class Care
    {
        public Item currentItem = PageContext.Current.Item;

        public string vCardAddress
        {
            get { return FieldRenderer.Render(currentItem, "Footer vCard Address"); }
        }

        public string FooterLinks
        {
            get { return FieldRenderer.Render(currentItem, "Footer Links"); }
        }

        public string PageItemName
        {
            get { return this.currentItem["Page Title"]; }
        }

        public string ApptPhoneNumber
        {
            get { return FieldRenderer.Render(currentItem, "Appointment Phone Number"); }
        }

        // END - Care-Master Field Values - Care Master Templates

        public HtmlString executeAutosave()
        {
            if (Sitecore.Context.PageMode.IsExperienceEditor)
            {
                return new HtmlString("<script> jQuery(document).ready(function () { autoSave(); }); </script>");
            }
            return new HtmlString("");
        }

        public string updateMetaTitle()
        {
            if (!String.IsNullOrEmpty(currentItem.Fields["__Display name"].Value) && String.IsNullOrEmpty(currentItem.Fields["Meta Title"].Value))
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    currentItem.Editing.BeginEdit();
                    currentItem.Fields["Meta Title"].Value = currentItem.Fields["__Display name"].Value + " | USF Health";
                    currentItem.Editing.EndEdit();
                }
            }
            return "";
        }

        public string headlineOpen(string className)
        {
            return "<h1 class='" + className + "'>";
        }
        public string headlineClose
        {
            get { return "</h1>"; }
        }
    }
}