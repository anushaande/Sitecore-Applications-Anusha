using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class GoogleMap : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public string Zoom { get; set; }
        public string LocationTitle { get; set; }
        public string LocationDescription { get; set; }
        public bool dsIsSet { get; set; }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            dsIsSet = !String.IsNullOrEmpty(rendering.DataSource);
            if (dsIsSet)
            {
                try
                {
                    DataSourceFolder = Sitecore.Context.Database.GetItem(rendering.DataSource);
                }
                catch { }
            }
            Latitude = Helpers.getStrField(DataSourceFolder, "Latitude");
            Longitude = Helpers.getStrField(DataSourceFolder, "Longitude");
            Address = Helpers.getStrField(DataSourceFolder, "Address");
            Zoom = Helpers.getStrField(DataSourceFolder, "Zoom");
            LocationTitle = Helpers.getStrField(DataSourceFolder, "Location Title");
            LocationDescription = Helpers.getStrField(DataSourceFolder, "Location Description");
        }
    }
}