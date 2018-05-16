using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class AdvancedTab : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool IsDatasourceNull { get; set; }
        public MultilistField layouts { get; set; }
        public Item availableLayout { get; set; }

        // Layout Data Fields
        public string tabTitle = String.Empty;
        public string videoEmbedUrl = String.Empty;
        public string description = String.Empty;
        public string descriptionLeft = String.Empty;
        public string descriptionRight = String.Empty;
        public ImageField videoThumbnail = null;
        public ImageField imagePath = null;

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);
            try
            {
                Item item = Item.Database.GetItem(Rendering.DataSource);
                IsDatasourceNull = (item != null ? false : true);
                if (IsDatasourceNull)
                {
                    throw new ArgumentNullException("args.Item");
                }
                else
                {
                    layouts = item.Fields["Layout"];
                }
            }
            catch
            {
                IsDatasourceNull = true;
            }
        }
    }
}