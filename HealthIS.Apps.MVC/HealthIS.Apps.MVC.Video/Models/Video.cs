using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class Video : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool dsIsSet { get; set; }
        public string dataSourceID { get; set; }
        public Sitecore.Data.Fields.ImageField image { get; set; }
        public string URL { get; set; }

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
                    }
                }
                catch { dataSourceID = ""; }
                image = Helpers.getImgField(Item, "Thumbnail");
                URL = Helpers.getStrField(Item, "URL");
            }
        }
    }
}