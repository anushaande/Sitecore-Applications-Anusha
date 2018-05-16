using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Items;
using System;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class TwoColumnsServiceBox : IRenderingModel
    {
        public string LeftHeaderIcon { get; set; }
        public string LeftHeaderTitle { get; set; }
        public string LeftBoxContent { get; set; }
        public string RightHeaderIcon { get; set; }
        public string RightHeaderTitle { get; set; }
        public string RightBoxContent { get; set; }
        public Item Item { get; set; }

        public string Error { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item PageItem { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            try
            {
                Rendering = rendering;
                Item = rendering.Item;
                PageItem = Sitecore.Mvc.Presentation.PageContext.Current.Item;

                LeftHeaderIcon = Item["Left Header Icon"];
                LeftHeaderTitle = FieldRenderer.Render(Item, "Left Header Title");
                LeftBoxContent = FieldRenderer.Render(Item, "Left Box Content");
                RightHeaderIcon = Item["Right Header Icon"];
                RightHeaderTitle = FieldRenderer.Render(Item, "Right Header Title");
                RightBoxContent = FieldRenderer.Render(Item, "Right Box Content");
            }
            catch (Exception ex)
            {
                Error += ex.Message + "<br/>" + ex.StackTrace + "<br/>";
            }
        }

        public HtmlString ActionItemButton(string action, string buttonName, Item parentItem)
        {
            return new HtmlString(string.Format(@"<input type=""button"" class=""button"" value=""{1}"" onclick=""javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:{0}(id={2})')"" />", action, buttonName, parentItem.ID));
        }
    }
}