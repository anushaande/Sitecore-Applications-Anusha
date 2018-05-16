using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;

namespace HealthIS.Apps.MVC.Models
{
    public class HeadingTag : IRenderingModel
    {
        public string smallHeadline { get; set; }
        public string largeHeadline { get; set; }
        public string headline { get; set; }
        public string isSetDs { get; set; }
        public Item Item { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item PageItem
        {
            get
            {
                return PageContext.Current.Item;
            }
        }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Item = rendering.Item;
            isSetDs = rendering.DataSource;
            Rendering = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering;
            if (!String.IsNullOrEmpty(isSetDs))
            {
                try {
                    smallHeadline = FieldRenderer.Render(Item, "Small Headline");
                    largeHeadline = FieldRenderer.Render(Item, "Large Headline");
                    headline = FieldRenderer.Render(Item, "Headline");
                }
                catch
                {
                    isSetDs = "";
                }
            }
        }
    }
}