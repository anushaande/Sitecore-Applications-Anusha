using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class MultipleLocation
    {
        // Initialize component's rendering info
        public Item Item { get; set; }
        public Rendering Rendering { get; set; }
        public Item pageItem { get; set; }
        public bool setDatasource { get; set; }
        public string FolderItem { get; set; }
        public Database dbName { get; set; }
        public Item newAddedItem { get; set; }
        public string newAddedItemPopUp { get; set; }
        public bool isThumbnailTypeAdded = false;
        public bool isListTypeAdded = false;
        public bool isPatientCarePage { get; set; }

        public HtmlString fieldRender(Item item, string fieldName, string parm = "")
        {
            return new HtmlString(Sitecore.Web.UI.WebControls.FieldRenderer.Render(item, fieldName, parm));
        }

        public HtmlString ActionItemButton(string className, string action, string buttonName, Item parentItem)
        { 
            return new HtmlString(string.Format(@"<input type=""button"" class=""{3}"" value=""{1}"" onclick=""javascript:Sitecore.PageModes.PageEditor.postRequest('webedit:{0}(id={2})')"" />", action, buttonName, parentItem.ID, className));
        }

        public HtmlString MoveItemButton(string className, string action, string buttonName, Item parentItem)
        {
            return new HtmlString(string.Format(@"<input type=""button"" class=""{3}"" value=""{1}"" onclick=""javascript:Sitecore.PageModes.PageEditor.postRequest('item:{0}(id={2})')"" />", action, buttonName, parentItem.ID, className));
        }
    }
}