using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Web.Mvc;
using HealthIS.Apps.MVC.Models;
using Sitecore.Mvc.Presentation;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace HealthIS.Apps.MVC.Controllers
{
    public class Components : Controller
    {
        public ActionResult HorizontalNavigation(string parentPath, string templatePath)
        {
            HorizontalNavigation hn = new HorizontalNavigation();
            hn.Rendering = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering;
            hn.Item = hn.Rendering.Item;
            hn.PageItem = PageContext.Current.Item;

            string dbName = "master";
            if (HealthIS.Apps.Util.isOnProduction()) { dbName = "web"; }
            //hn.masterDb = Sitecore.Configuration.Factory.GetDatabase(dbName);
            hn.masterDb = hn.PageItem.Database;

            try
            {
                hn.datasourceItem = hn.masterDb.Items.GetItem(hn.Rendering.DataSource);
                hn.isDatasourceSet = (String.IsNullOrEmpty(hn.datasourceItem.Key)) ? false : true;
            }
            catch
            {
                hn.isDatasourceSet = false;
            }

            if (hn.isDatasourceSet)
            {
                // NavBarHorizontal Template
                hn.H_NavBarClass = hn.Item["Nav Bar Class"];
                hn.H_LabelH1Class = hn.Item["Label H1 Class"];
                hn.H_LabelIClass = hn.Item["Label i Class"];
                hn.H_NavBarLabel = hn.Item["Nav Bar Label"];
                hn.H_UlClass = hn.Item["ul Class"];

                hn.LiList = new List<HtmlString>();
                hn.HsTag = new HtmlString("");
                hn.ListCount = 0;
                hn.NewTab = "";

                if (!String.IsNullOrEmpty(parentPath) && !String.IsNullOrEmpty(templatePath))
                {
                    //Sitecore.Data.Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                    Item newItem = hn.masterDb.Items["/sitecore/content/home"];
                    using (new SecurityDisabler())
                    {
                        string itemName = "NewLinkItem";
                        Item parentItem = hn.masterDb.Items[parentPath];

                        if (templatePath.StartsWith("/sitecore/templates/Branches/Horizontal Navigation/", StringComparison.OrdinalIgnoreCase))
                        {
                            BranchItem branch = hn.masterDb.GetItem(templatePath);
                            itemName = (branch.Name.Contains("Vertical") ? "NewVerticalItems" : "NewSectionItems");
                            newItem = parentItem.Add(itemName, branch);
                        }
                        else
                        {
                            TemplateItem template = hn.masterDb.GetTemplate(templatePath);
                            newItem = parentItem.Add(itemName, template);
                        }
                    }

                    hn.newAddedItem = newItem;
                    hn.newAddedItemPopUp = hn.newAddedItem.ID.ToString();

                    // Place new item after last item in the tree
                    hn.newAddedItem.Editing.BeginEdit();
                    Item lastItem = hn.newAddedItem.Parent.GetChildren().Last();
                    int lastItemSortOrder = lastItem.Appearance.Sortorder;
                    hn.newAddedItem.Appearance.Sortorder = lastItemSortOrder + 100;
                    hn.newAddedItem.Editing.EndEdit();

                    return Content("<div class='newItemId'>" + hn.newAddedItemPopUp + "</div>");
                }
            }

            return PartialView(hn);
        }
    }
}
