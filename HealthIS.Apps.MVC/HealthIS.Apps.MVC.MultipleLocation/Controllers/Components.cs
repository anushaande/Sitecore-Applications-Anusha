using System.Web.Mvc;
using Sitecore.Mvc.Presentation;
using System;
using Sitecore.Web.UI.WebControls;
using HealthIS.Apps.MVC.Models;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System.Linq;
using System.Collections.Generic;

namespace HealthIS.Apps.MVC.Controllers
{
    public class Components : Controller
    {
        public ActionResult MultipleLocation(string parentPath_ML, string templatePath_ML, string typeName)
        {
            MultipleLocation ml = new MultipleLocation();
            ml.Rendering = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering;
            ml.Item = ml.Rendering.Item;
            ml.pageItem = PageContext.Current.Item;
            ml.setDatasource = false;
            ml.dbName = ml.pageItem.Database;
            ml.isPatientCarePage = ml.pageItem.Paths.FullPath.ToLower().Contains("/content/home/doctors/locations") ||
                ml.pageItem.Paths.FullPath.ToLower().Contains("/content/home/care/locations");

            try
            {
                ml.setDatasource = (!String.IsNullOrEmpty(Sitecore.Context.Database.GetItem(ml.Rendering.DataSource).Key) ? true : false);
            }
            catch
            {
                return PartialView(ml);
            }

            if (ml.setDatasource)
            {                
                if (!String.IsNullOrEmpty(parentPath_ML) && !String.IsNullOrEmpty(templatePath_ML))
                {
                    Item newItem = ml.dbName.Items["/sitecore/content/home"];
                    using (new SecurityDisabler())
                    {                        
                        Item parentItem = ml.dbName.Items[parentPath_ML];
                        TemplateItem template = ml.dbName.GetTemplate(templatePath_ML);
                        string itemName = "New Location";
                        string newItemType = "New Type";
                        if (typeName == "Thumbnail" || typeName == "List")
                        {
                            newItemType = typeName + " Type";
                        }
                        else if (typeName == "Phone Number")
                        {
                            newItemType = typeName;
                        }
                        List<Item> c = parentItem.GetChildren().ToList();
                        Item[] children = c.Where(x => x.Name.StartsWith(newItemType)).ToArray();
                        itemName = children.Length > 0 ? newItemType + nextItemNum(children, newItemType) : newItemType;
                        newItem = parentItem.Add(itemName, template);
                    }

                    ml.newAddedItem = newItem;
                    ml.newAddedItemPopUp = ml.newAddedItem.ID.ToString();

                    return Content("<div class='newItemId'>" + ml.newAddedItemPopUp + "</div>");
                }
            }
            return PartialView(ml);
        }

        // returns string value of the next number to append on newItem's name, to allow sorting
        public string nextItemNum(Item[] list, string type)
        {            
            if (list != null && !String.IsNullOrEmpty(type))
            {
                List<int> values = new List<int>() { 0 };
                int num = 0;
                foreach (Item item in list)
                {
                    string hasNum = item.Name.Replace(type, "");
                    if (!String.IsNullOrEmpty(hasNum) && int.TryParse(hasNum, out num))
                    {
                        values.Add(Convert.ToInt32(num));
                    }
                }
                int highestValue = values.Max();
                highestValue++;
                return highestValue.ToString();
            }
            return null;
        }
    }
}