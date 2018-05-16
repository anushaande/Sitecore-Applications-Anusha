using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web.UI.WebControls;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Data.Fields;


namespace Layout_v2.Models
{
    public class MeetOurDoctors : IRenderingModel
    {
        //public string Name { get; set; }
        //public string ProfileImage { get; set; }
        //public HtmlString Content { get; set; }
        //public string ProfileLink { get; set; }

        // MeetOurDoctors Template Main
        public string Subject { get; set; }

        public Item Item { get; set; }
        public Item Child { get; set; }
        public List<Item> ChildItems { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;

            Subject = FieldRenderer.Render(Item, "Subject");
        }

        public string Datasource(Item item2, string fieldName)
        {
            return FieldRenderer.Render(item2, fieldName);
        }
    }
}