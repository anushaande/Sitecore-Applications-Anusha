using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class Image : Sitecore.Mvc.Presentation.RenderingModel
    {
        public string dataSourceID { get; set; }
        public bool dsIsSet { get; set; }
        public Sitecore.Data.Fields.ImageField image { get; set; }
        public string datasourcePath { get; set; }
        public string currentPagePath { get; set; }
        public bool dsEditable = false;
        public bool isInRestrictedPlaceholder = false;

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            dsIsSet = !String.IsNullOrEmpty(rendering.DataSource);
            if (dsIsSet)
            {
                try
                {
                    Item ds = Sitecore.Context.Database.GetItem(rendering.DataSource);

                    if (ds == null)
                    {
                        dataSourceID = ""; 
                        dsIsSet = false;
                    }
                    else
                    {
                        dataSourceID = ds.ID.Guid.ToString();
                        datasourcePath = ds.Paths.Path;
                        currentPagePath = PageItem.Paths.Path;
                        dsEditable = Helpers.IsDatasourceEditable(ds, currentPagePath);
                        string[] restricted = { "Media", "Image" };
                        isInRestrictedPlaceholder = Helpers.CheckRenderingPlaceholder(rendering.Placeholder, restricted);
                    }
                }
                catch { dataSourceID = ""; }
                image = Helpers.getImgField(Item, "image");
                
            }
        }
    }
}