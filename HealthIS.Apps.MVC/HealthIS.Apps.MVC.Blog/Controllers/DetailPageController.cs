using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Maintenance;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Templates;
using Sitecore.Mvc.Controllers;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HealthIS.Apps.MVC.Blog.DetailPage
{
    public class Blog_HealthISController : SitecoreController
    {
        public ActionResult HealthIS_Blog_DetailPage(string sync)
        {
            return View(new HealthIS.Apps.MVC.Blog.Models.DetailPage());
        }

        [HttpPost]
        public ActionResult HealthIS_Blog_DetailPage(HealthIS.Apps.MVC.Blog.Models.DetailPage detailPage, string categories, string tags, string sync = null, string lengthLimit = null)
        {
            if (detailPage.IsExperiencePage)
            {

                string descriptionValue = detailPage.CurrentPage.Fields["Description"].Value.Trim();
                string contentValue = detailPage.CurrentPage.Fields["Content"].Value.Trim();

                // Update description field from content value
                //if ((String.IsNullOrEmpty(descriptionValue) && !String.IsNullOrEmpty(contentValue)) || sync == "true")
                //{
                //    var regex = new System.Text.RegularExpressions.Regex("<.*?>");
                //    var plainString = regex.Replace(contentValue, String.Empty);
                //    int length = 500;
                //    int n;
                //    bool isNumeric = int.TryParse(lengthLimit, out n);
                //    if (isNumeric)
                //    {
                //        length = Int32.Parse(lengthLimit);
                //    }
                //    string updatedDescription = string.Concat(plainString.Take(length)) + "..."; ;

                //    using (new SecurityDisabler())
                //    {
                //        detailPage.CurrentPage.Editing.BeginEdit();
                //        detailPage.CurrentPage.Fields["Description"].Value = updatedDescription;
                //        detailPage.CurrentPage.Editing.EndEdit();
                //        detailPage.CurrentPage.Editing.AcceptChanges();
                //    }
                //}

                var regex = new System.Text.RegularExpressions.Regex("<.*?>");
                var plainString = regex.Replace(contentValue, String.Empty);
                string updatedDescription = string.Concat(plainString.Take(300)) + "..."; ;

                if (detailPage.CurrentPage.Fields["Description"].Value != updatedDescription)
                {
                    using (new SecurityDisabler())
                    {
                        detailPage.CurrentPage.Editing.BeginEdit();
                        detailPage.CurrentPage.Fields["Description"].Value = updatedDescription;
                        detailPage.CurrentPage.Editing.EndEdit();
                        detailPage.CurrentPage.Editing.AcceptChanges();
                    }
                }

                Item tagRepository = detailPage.CurrentDb.GetItem(detailPage.TagRepository.ID);
                if (tagRepository != null && !String.IsNullOrEmpty(tags))
                {
                    // List of all assigned tags and update the field
                    detailPage.UpdateTagField(tags, "Tags", tagRepository);
                }
            }

            return View(detailPage);
        }
    }
}