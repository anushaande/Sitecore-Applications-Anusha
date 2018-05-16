using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Models
{
    public class Redirect : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item item { get; set; }
        public bool isDatasourceNull { get; set; }
        public string targetLocation { get; set; }
        public string statusList { get; set; }
        public string status { get; set; }

        public string DataItems(Item item, string fieldName)
        {
            return FieldRenderer.Render(item, fieldName);
        }

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);

            try
            {
                item = Item.Database.GetItem(Rendering.DataSource);
                isDatasourceNull = item == null ? true : false;

                if (isDatasourceNull)
                {
                    throw new ArgumentNullException("args.Item");
                }
                else
                {
                    status = item.Fields["Status"].Value;
                    targetLocation = item.Fields["Target Location"].Value;
                    if (String.IsNullOrEmpty(item.Fields["Status"].Value))
                    {
                        status = "Permanent";
                    }
                }
            }
            catch
            {
                isDatasourceNull = true;
            }
        }
    }
}