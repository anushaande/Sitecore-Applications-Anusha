using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace HealthIS.Apps.MVC.Models
{
    public class SlideShow : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool IsDatasourceNull { get; set; }
        public Sitecore.Data.Fields.CheckboxField chSlideShowHideArrow { get; set; }
        public Sitecore.Data.Templates.TemplateField chHideArrowTemplateField { get; set; }
        public Sitecore.Data.Fields.ImageField imagePath { get; set; }
        public Sitecore.Data.Items.MediaItem image { get; set; }
        public string getHideArrowTemplateId { get; set; }
        public string SlideShowHideArrow { get; set; }
        public string imageHeight { get; set; }
        public Sitecore.Data.Fields.CheckboxField chHorizontal { get; set; }
        public Sitecore.Data.Templates.TemplateField chHorizontalTemplateField { get; set; }
        public string getHorizontalTemplateId { get; set; }
        public string HorizontalSlider { get; set; }

        public string DataItems(Item item2, string fieldName, string parm = "")
        {
            return FieldRenderer.Render(item2, fieldName, parm);
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
                    chSlideShowHideArrow = item.Fields["Hide Arrow"];
                    imageHeight = item.Fields["Image Height"].ToString();
                    chHideArrowTemplateField = item.Fields["Hide Arrow"].GetTemplateField();
                    getHideArrowTemplateId = chHideArrowTemplateField.ID.Guid.ToString().ToUpper();
                    SlideShowHideArrow = chSlideShowHideArrow.Checked ? "slideshow-no-arrows" : "";
                    chHorizontal = item.Fields["Horizontal"];
                    chHorizontalTemplateField = item.Fields["Horizontal"].GetTemplateField();
                    getHorizontalTemplateId = chHorizontalTemplateField.ID.Guid.ToString().ToUpper();
                    HorizontalSlider = chHorizontal.Checked ? "hz-slider" : "";
                }
            }
            catch
            {
                IsDatasourceNull = true;
            }
        }
    }
}