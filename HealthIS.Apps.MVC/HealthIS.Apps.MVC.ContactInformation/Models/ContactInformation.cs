using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;

namespace HealthIS.Apps.MVC.Models
{
    public class ContactInformation : IRenderingModel
    {
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string address5 { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string datasourceSet { get; set; }
        public Item item { get; set; }
        public Item PageItem { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public string datasourcePath { get; set; }
        public string currentPagePath { get; set; }
        public bool dsEditable = false;

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            item = rendering.Item;
            PageItem = PageContext.Current.Item;
            Rendering = rendering;
            datasourceSet = rendering.DataSource;

            name = FieldRenderer.Render(item, "Name");
            address1 = FieldRenderer.Render(item, "Address1");
            address2 = FieldRenderer.Render(item, "Address2");
            address3 = FieldRenderer.Render(item, "Address3");
            address4 = FieldRenderer.Render(item, "Address4");
            address5 = FieldRenderer.Render(item, "Address5");
            phone = FieldRenderer.Render(item, "Phone");
            email = FieldRenderer.Render(item, "Email");

            if (!String.IsNullOrEmpty(datasourceSet))
            {
                datasourcePath = rendering.Item.Paths.Path.ToString();
                currentPagePath = PageItem.Paths.Path.ToString();
                dsEditable = Helpers.IsDatasourceEditable(item, currentPagePath);
            }
        }

        public string addBreak(string fieldValue)
        {
            if (!String.IsNullOrEmpty(fieldValue))
            {
                fieldValue += "<br />";
            }
            return fieldValue;
        }
    }
}