using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Security.AccessControl;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class RichTextEditor : IRenderingModel
    {
        public bool dsIsSet { get; set; }
        public string richTextEditor { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Item = rendering.Item;
            Rendering = rendering;
            PageItem = Sitecore.Mvc.Presentation.PageContext.Current.Item;
            dsIsSet = (!String.IsNullOrEmpty(rendering.DataSource) ? true : false);
            richTextEditor = FieldRenderer.Render(Item, "RichTextContent");
        }
    }
}