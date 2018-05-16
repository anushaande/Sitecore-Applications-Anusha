using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Web;
using Sitecore.SecurityModel;
using Sitecore.Web.UI.WebControls;



namespace HealthIS.Layouts.Models
{
    public class Doctors
    {
        public Item currentItem = PageContext.Current.Item;
              
        public string vCardAddress
        {
            get { return FieldRenderer.Render(currentItem, "Footer vCard Address"); }
        }        
        public string FooterGmap
        {
            get { return FieldRenderer.Render(currentItem, "Footer Gmap"); }
        }

        public string FooterLinks
        {
            get { return FieldRenderer.Render(currentItem, "Footer Links"); }
        }

        public string PageItemName
        {
            get { return this.currentItem["Page Title"]; }
        }

        public string PatientToolboxNumber
        {
            get { return this.currentItem["PatientToolbox Phone Number"]; }
        }

        // END - Doctor-Master Field Values - Doctor Master Template
        
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

        public HtmlString GetPatientToolBox()
        {
            Item dataItem = currentItem.Database.GetItem("/sitecore/content/Data Components/Doctors/Doctors PatientToolBox");
            string data = "";
            if (dataItem == null)
            {
                dataItem = currentItem.Database.GetItem(Sitecore.Data.ID.Parse("{D7A1535D-F499-427C-8A88-F5448BC06D33}"));
            }
            if (dataItem != null)
            {
                data = dataItem.Fields["Data Container"].Value.Replace("{{PatientToolboxNumber}}", PatientToolboxNumber);
            }
            return new HtmlString(data);
        }
    }
}


