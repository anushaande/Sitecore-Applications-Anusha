using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;

namespace HealthIS.Apps.MVC.FAQ.Models
{
    public class FAQ : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public List<FAQSection> Sections { get; set; }
        public bool dsSet = false;

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            try
            {
                //Set model specific objects
                Rendering = rendering;
                dsSet = !String.IsNullOrEmpty(rendering.DataSource);
                if (dsSet)
                {
                    DataSourceFolder = Sitecore.Context.Database.GetItem(rendering.DataSource);
                    if (DataSourceFolder == null)
                    {
                        dsSet = false;
                    }
                    else
                    {
                        Sections = new List<FAQSection>();
                        foreach (Item s in DataSourceFolder.Children)
                        {
                            FAQSection section = new FAQSection(s);
                            foreach (Item i in s.Children)
                            {
                                section.Listings.Add(new FAQItem(i));
                            }
                            Sections.Add(section);
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



    public class FAQSection
    {
        public string Heading { get; set; }
        public Item SCItem { get; set; }
        public List<FAQItem> Listings { get; set; }
        public IEnumerable<SelectListItem> options { get; set; }  // holds combo box items
        public string templateFieldName = "First Item Open By Default";  // combo box field name on template
        public FAQSection(Item scItem)
        {
            SCItem = scItem;
            Heading = Helpers.getStrField(scItem, "Heading");
            Listings = new List<FAQItem>();
        }

        // local method to call getComboBoxItems()
        public void populateComboBox()
        {
            // returns combo box items as a List<SelectListItem>
            options = HealthIS.Apps.CustomFields.Helpers.getComboBoxItems(SCItem.ID.ToString(), templateFieldName);
        }
    }

    public class FAQItem
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public Item SCItem { get; set; }
        public FAQItem(Item scItem)
        {
            SCItem = scItem;
            Question = Helpers.getStrField(scItem, "Question");
            Answer = Helpers.getStrField(scItem, "Answer");
        }
    }
}