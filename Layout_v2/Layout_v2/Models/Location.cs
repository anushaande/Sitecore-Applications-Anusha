using Sitecore.Data.Items;
using Sitecore.Data.Query;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace Layout_v2.Models
{
    public class Location : IRenderingModel
    {
        // Location Template
        public string Name { get; set; }
        public string Address { get; set; }
        public string MapURL { get; set; }
        public string LocationID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string DefaultImage { get; set; }

        // PhysicalLocationParameter Template
        public string Locations { get; set; }
        public string AddressLine2 { get; set; }
        public string Phone { get; set; }
        public string URL { get; set; }
        public string Image { get; set; }

        public Item Item { get; set; }
        public RenderingParameters Parameter { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Parameter = rendering.Parameters;
            Locations = Parameter["Location"];

            if (!String.IsNullOrEmpty(Locations))
            {
                Sitecore.Data.ID Id = Sitecore.Data.ID.Parse(Locations);
                Item = Sitecore.Context.Database.GetItem(Id);
                
                AddressLine2 = Parameter["Address Line 2"];
                Phone = Parameter["Phone"];
                URL = Parameter["URL"];
                Image = Parameter["Image"];

                if (Locations != "{3CF19799-D782-4708-B5F3-09EFFC0C30B5}")
                {
                    Name = FieldRenderer.Render(Item, "Name");
                    Address = FieldRenderer.Render(Item, "Address");
                    MapURL = FieldRenderer.Render(Item, "MapURL");
                    LocationID = FieldRenderer.Render(Item, "LocationID");
                    City = FieldRenderer.Render(Item, "City");
                    State = FieldRenderer.Render(Item, "State");
                    Zip = FieldRenderer.Render(Item, "Zip");
                    DefaultImage = FieldRenderer.Render(Item, "DefaultImage", "bc=lightgray&as=1");
                }
            }
        }
    }
}