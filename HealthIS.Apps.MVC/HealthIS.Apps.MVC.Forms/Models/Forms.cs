using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;

namespace HealthIS.Apps.MVC.Models
{
    public class Forms : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool dsSet = false;
        public Item ds { get; set; }
        public string location = "";
        public List<string> ErrLog = new List<string>();
        public List<Item> forms { get; set; }
        public bool isModal = false;

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            try
            {
                //Set model specific objects
                Rendering = rendering;
                dsSet = !string.IsNullOrEmpty(rendering.DataSource);
                if (dsSet)
                {
                    ds = Sitecore.Context.Database.GetItem(rendering.DataSource);
                    dsSet = ds != null;
                    if (dsSet)
                    {
                        isModal = Helpers.getBoolField(ds, "Is Modal");
                        string dsFormID = Helpers.getStrField(ds, "Form ID");
                        if(!string.IsNullOrEmpty(dsFormID)){
                            Item dsForm = Sitecore.Context.Database.GetItem(dsFormID);
                            if (dsForm != null)
                            {
                                location = Helpers.getStrField(dsForm, "Location");
                            }
                        }
                    }
                }
                forms = GetAvailForms(ErrLog);
            }
            catch (Exception ex)
            {
                HealthIS.Apps.Util.LogError(ex.Message, ex, this);
            }
        }

        public static List<Item> GetAvailForms(List<string> errLog = null)
        {
            if (errLog == null) { errLog = new List<string>(); }
            List<Item> forms = new List<Item>();
            Item formsFolder = Sitecore.Context.Database.GetItem("/sitecore/content/Data Components/Forms");
            if (formsFolder != null)
            {
                Sitecore.Collections.ChildList deptFolders = formsFolder.Children;
                if (deptFolders != null)
                {
                    foreach (Item dept in deptFolders)
                    {
                        foreach (Item form in dept.Children)
                        {
                            if (form.TemplateName.Contains("Form")
                            && form.Fields.Where(f => f.Name == "Location").ToList().Count > 0
                            && form.Fields["Location"].Value.Length > 0)
                            {
                                forms.Add(form);
                            }
                        }                        
                    }
                }
            }
            else
            {
                errLog.Add("Forms folder not found. Can not get a list of available forms.");
            }
            return forms;
        }

        public string getCheckedAttr(bool fieldVal)
        {
            if (fieldVal) { return "checked='checked'"; }
            else { return ""; }
        }
    }
}