using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class VerticalNav : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public bool dsIsSet { get; set; }
        public int itemCount { get; set; }
        public int itemCtMax { get; set; }
        public string currentPagePath { get; set; }
        public string datasourcePath { get; set; }
        public bool dsEditable = false;

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            dsIsSet = !String.IsNullOrEmpty(rendering.DataSource);
            itemCount = 0;
            itemCtMax = int.MaxValue;    
            if (dsIsSet)
            {
                try
                {
                    DataSourceFolder = Sitecore.Context.Database.GetItem(rendering.DataSource);
                    if (DataSourceFolder != null)
                    {
                        foreach (Item i in DataSourceFolder.Children)
                        {
                            itemCount++;
                            foreach (Item ii in i.Children)
                            {
                                itemCount++;
                            }
                        }
                    }
                    datasourcePath = rendering.Item.Paths.Path.ToString();
                    currentPagePath = PageItem.Paths.Path.ToString();
                    dsEditable = Helpers.IsDatasourceEditable(rendering.Item, currentPagePath);                    
                }
                catch {
                }
            }
        }
        
        // Remove special characters and rename Item
        public HtmlString RenameItem(Item item)
        {
            try
            {
                if (!String.IsNullOrEmpty(item.Fields["Title"].Value))
                {
                    if (item.Name != item.Fields["Title"].Value)
                    {
                        string title = item.Fields["Title"].Value;
                        string updatedName = title;

                        string ex = "<.*?>|&.*?;"; // specifically strips html tags
                        string ex1 = @"[$@&+,:;/=?@#|~{}'`=+<>\[\].^*()\%!’" + "\"" + "]"; // strips other special characters                        
                        var regexItem = new System.Text.RegularExpressions.Regex(ex);
                        var regexItem1 = new System.Text.RegularExpressions.Regex(ex1);
                        if (regexItem.IsMatch(title) || regexItem1.IsMatch(title))
                        {
                            updatedName = System.Text.RegularExpressions.Regex.Replace(title, ex, "");
                            updatedName = System.Text.RegularExpressions.Regex.Replace(updatedName, ex1, "");
                        }
                        if (item.Name != updatedName)
                        {
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {
                                item.Editing.BeginEdit();
                                item.Name = updatedName;
                                item.Editing.EndEdit();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Warn("Vertical Nav Error: " + e.Message, this);
            }
            return new HtmlString("");
        }
    }    
}
