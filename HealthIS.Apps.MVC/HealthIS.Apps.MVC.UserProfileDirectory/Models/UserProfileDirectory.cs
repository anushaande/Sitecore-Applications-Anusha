using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace HealthIS.Apps.MVC.UserProfileDirectory.Models
{
    public class UserProfileDirectory : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public Item ListingRootFolder { get; set; }
        public Item ViewRootFolder { get; set; }
        public List<Item> ListingsFolders { get; set; }
        //public Item ViewsFolder { get; set; }
        public List<Item> ViewsFolders { get; set; }
        public Sitecore.Data.Fields.CheckboxField chEnableAccordion { get; set; }
        public Sitecore.Data.Templates.TemplateField chEnableAccordionTemplateField { get; set; }
        public string chEnableAccordionTemplateId { get; set; }
        public bool dsSet = false;
        public string dsID = "";

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            ListingsFolders = new List<Item>();
            ViewsFolders = new List<Item>();
            try
            {
                //Set model specific objects
                Rendering = rendering;
                dsSet = !String.IsNullOrEmpty(rendering.DataSource);
                if (dsSet)
                {
                    DataSourceFolder = Sitecore.Context.Database.GetItem(rendering.DataSource);
                    
                    if (DataSourceFolder != null)
                    {
                        chEnableAccordion = Rendering.Item.Fields["Enable Accordion"];
                        chEnableAccordionTemplateField = Rendering.Item.Fields["Enable Accordion"].GetTemplateField();
                        chEnableAccordionTemplateId = chEnableAccordionTemplateField.ID.Guid.ToString().ToUpper();

                        foreach (Item i in DataSourceFolder.Children)
                        {
                            //if (i.Name.ToLower().Contains("_updvf"))
                            //{
                            //    ViewsFolder = i;
                            //}
                            if (i.Name.ToLower().Contains("_updvfi"))
                            {
                                ViewRootFolder = i;
                                foreach (Item vf in i.Children)
                                {
                                    if (vf.Name.ToLower().Contains("_updvfs"))
                                    {
                                        ViewsFolders.Add(vf);
                                    }
                                }
                            }
                            else if (i.Name.ToLower().Contains("_updlfi"))
                            {
                                ListingRootFolder = i;
                                foreach (Item lf in i.Children)
                                {
                                    if (lf.Name.ToLower().Contains("_updlfs"))
                                    {
                                        ListingsFolders.Add(lf);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HealthIS.Apps.Util.LogError(ex.Message, ex, this);
            }
        }
    }
}