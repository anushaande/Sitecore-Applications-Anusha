using System;
using System.Web.UI;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Web.Mvc;


namespace HealthIS.Apps.CustomFields
{
    public class UnboundComboBox : Sitecore.Web.UI.HtmlControls.Combobox
    {
        private string Source { get; set; }
        protected override void OnLoad(EventArgs e)
        {
            if (Controls.Count == 0)
            {
                try
                {
                    if(!String.IsNullOrEmpty(Source))
                    {                        
                        foreach (string s in Source.Split('|'))
                        {
                            Controls.Add(new ListItem
                            {
                                Header = s,
                                Value = s
                            });

                        }                                                
                    }
                    else { Controls.Add(new ListItem { Header = "Could not load source: No source specified for field" }); }
                }                
                catch (Exception exc)
                {
                    Controls.Add(new ListItem { Header = "Could not load source: " + exc });
                }
            }
            base.OnLoad(e);
        }
    }

    public class Helpers
    {        
        // gets items that populate combo box
        public static List<SelectListItem> getComboBoxItems(string itemID, string templateFieldName)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            string source = string.Empty;
            Item dsItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(itemID);
            if(dsItem != null)
            {
                string dsTemplateItemID = dsItem.TemplateID.ToString();
                Item templateItem = Sitecore.Context.Database.GetItem(dsTemplateItemID);
                if(templateItem != null)
                {
                    string templatePath = templateItem.Paths.Path;
                    string query = "fast:" + templatePath + "//*[@@templateid='{455A3E98-A627-4B40-8035-E683A0331AC7}' and @@name='" + templateFieldName + "']";
                    Item comboBox = Sitecore.Context.Database.SelectSingleItem(query);
                    if (comboBox != null && comboBox.Fields["Type"].Value.Equals("Unbound Combo Box"))
                    {
                        source = comboBox.Fields["Source"].Value;
                        if (!String.IsNullOrEmpty(source))
                        {
                            foreach (string s in source.Split('|'))
                            {
                                selectList.Add(new SelectListItem
                                {
                                    Text = s,
                                    Value = s
                                });
                            }
                        }
                    }
                }                
            }
            return selectList;
        }
    }
}