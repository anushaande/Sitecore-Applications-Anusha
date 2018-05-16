using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class WelcomeLetter : IRenderingModel
    {
        public bool dsIsSet { get; set; }
        public Sitecore.Data.Fields.ImageField image { get; set; }
        public string dataSourceID { get; set; }
        public string captionText { get; set; }
        public string captionUrl { get; set; }
        public string heading { get; set; }
        public string textContent { get; set; }
        public Item PageItem { get; set; }
        public Item Item { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Item = rendering.Item;
            Rendering = rendering;
            PageItem = Sitecore.Mvc.Presentation.PageContext.Current.Item;
            dsIsSet = false;
            if (!String.IsNullOrEmpty(Rendering.DataSource))
            {
                captionText = FieldRenderer.Render(Item, "Caption Text");
                captionUrl = FieldRenderer.Render(Item, "Caption Link");
                heading = FieldRenderer.Render(Item, "Heading");
                textContent = FieldRenderer.Render(Item, "Text Content");
                image = Helpers.getImgField(Item, "Image");

                try
                {
                    Item ds = Sitecore.Context.Database.GetItem(Rendering.DataSource);
                    if (ds == null)
                    {
                        dataSourceID = "";
                    }
                    else { dataSourceID = ds.ID.Guid.ToString(); }
                    dsIsSet = ds != null;
                }
                catch { dataSourceID = ""; }
            }
        }
    }
}