using System;
using System.Web.Mvc;
using HealthIS.Apps.MVC.Models;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.SecurityModel;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HealthIS.Apps.MVC.Controllers
{
    public class Components : Controller
    {
        public ActionResult SlideShow(string changedValue, string fieldName)
        {
            SlideShow ss = new SlideShow();
            ss.Rendering = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering;
            ss.Item = ss.Rendering.Item;
            ss.PageItem = Sitecore.Mvc.Presentation.PageContext.Current.Item;

            try
            {
                ss.datasourceItem = ss.PageItem.Database.GetItem(ss.Rendering.DataSource);
                ss.isDatasourceSet = (String.IsNullOrEmpty(ss.datasourceItem.Key)) ? false : true;
            }
            catch
            {
                ss.isDatasourceSet = false;
            }

            if (ss.isDatasourceSet)
            {
                ss.chSlideShowHideArrow = ss.Item.Fields["Hide Arrow"];
                ss.chHideArrowTemplateField = ss.Item.Fields["Hide Arrow"].GetTemplateField();
                ss.getHideArrowTemplateId = ss.chHideArrowTemplateField.ID.Guid.ToString().ToUpper();
                ss.SlideShowHideArrow = (ss.chSlideShowHideArrow.Checked) ? "slideshow-no-arrows" : "";
                ss.Error = new HtmlString("<center><br /><h3>Slide Show<br />Please set associated content or edit related item</h3><br /></center>");
                ss.FolderItem = ss.Item.Name;
            }
            else
            {
                return PartialView(ss);
            }

            return PartialView(ss);
        }
    }
}
