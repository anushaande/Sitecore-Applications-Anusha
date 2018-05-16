using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Data.Templates;

namespace HealthIS.Apps.MVC.Models
{
    public class ColumnBuilder : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool IsDatasourceNull { get; set; }

        public string DataItems(Item item2, string fieldName)
        {
            return FieldRenderer.Render(item2, fieldName);
        }

        public HtmlString ActionItemButton(string className, string action, string buttonName, Item parentItem)
        {
            return new HtmlString(string.Format(@"<input type=""button"" class=""{3}"" value=""{1}"" onclick=""javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:{0}(id={2})')"" />", action, buttonName, parentItem.ID, className));
        }

        public HtmlString MoveItemButton(string className, string action, string buttonName, Item parentItem)
        {
            return new HtmlString(string.Format(@"<input type=""button"" class=""{3}"" value=""{1}"" onclick=""javascript:Sitecore.PageModes.PageEditor.postRequest('item:{0}(id={2})')"" />", action, buttonName, parentItem.ID, className));
        }

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
                }
            }
            catch
            {
                IsDatasourceNull = true;
            }
        }
    }
}