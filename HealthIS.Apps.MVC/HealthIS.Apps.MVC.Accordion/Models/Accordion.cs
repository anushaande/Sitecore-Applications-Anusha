using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class Accordion : IRenderingModel
    {
        public string ID { get; set; }
        public string Class { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public string DivId { get; set; }
        public string FolderItem { get; set; }

        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public HtmlString Error { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Item = rendering.Item;
            PageItem = Sitecore.Mvc.Presentation.PageContext.Current.Item;
            Rendering = rendering;
            
            ID = FieldRenderer.Render(Item, "ID");
            Class = FieldRenderer.Render(Item, "Class");
            Error = new HtmlString("<center><br /><h3>Accordion<br />Please set associated content or edit related item</h3><br /></center>");
            FolderItem = Item.Name;

        }

        public string DataItems(Item item2, string fieldName)
        {
            return FieldRenderer.Render(item2, fieldName);
        }

        public HtmlString ActionItemButton(string action, string buttonName, Item parentItem)
        {
            return new HtmlString(string.Format(@"<input type=""button"" class=""button"" value=""{1}"" onclick=""javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:{0}(id={2})')"" />", action, buttonName, parentItem.ID));
        }

        public HtmlString MoveItemButton(string action, string buttonName, Item parentItem)
        {
            return new HtmlString(string.Format(@"<input type=""button"" class=""button"" value=""{1}"" onclick=""javascript:Sitecore.PageModes.PageEditor.postRequest('item:{0}(id={2})')"" />", action, buttonName, parentItem.ID));
        }
    }
}