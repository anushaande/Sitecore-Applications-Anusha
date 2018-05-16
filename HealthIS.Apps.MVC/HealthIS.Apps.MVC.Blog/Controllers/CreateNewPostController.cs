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

namespace HealthIS.Apps.MVC.Blog.CreateNewPost
{
    public class Blog_HealthISController : SitecoreController
    {
        public ActionResult HealthIS_Blog_CreateNewPost()
        {
            if (Sitecore.Context.PageMode.IsExperienceEditor)
            {
                return Redirect("/?sc_mode=preview&sc_itemid=" + Sitecore.Context.Item.ID.ToString() + "&sc_lang=en");
            }

            HealthIS.Apps.MVC.Blog.Models.CreateNewPost cnp = new HealthIS.Apps.MVC.Blog.Models.CreateNewPost();
            cnp.BlogArticleItems = cnp.MasterDb.SelectItems(cnp.ArticleRootPath.Paths.FullPath + "//*[@@templatename='" + cnp.BlogTemplateName + "']").OrderBy(x => x[Sitecore.FieldIDs.Created]).Reverse().Take(10);

            // Get current user's articles
            string currentUserName = cnp.UserInformation.Profile.UserName;
            cnp.GetCurrentUserArticles = cnp.BlogArticleItems.Where(item => item.Fields["__Created by"].Value.Equals(currentUserName)).ToList();

            return View(cnp);
        }

        [HttpPost]
        public ActionResult HealthIS_Blog_CreateNewPost(HealthIS.Apps.MVC.Blog.Models.CreateNewPost writeBlogPost)
        {
            if (ModelState.IsValid)
            {
                Item newArticle = null;

                //Database masterDb = Sitecore.Configuration.Factory.GetDatabase("master");
                Item articleRoot = writeBlogPost.MasterDb.GetItem(writeBlogPost.ArticleRootPath.ID);
                Item authorProfileRoot = writeBlogPost.MasterDb.GetItem(BlogSettings.AuthorProfileLibrary.ID);
                TemplateItem blogTemplate = writeBlogPost.MasterDb.GetTemplate(writeBlogPost.TemplatePath.ID);
                TemplateItem authorProfileTemplate = writeBlogPost.MasterDb.GetTemplate(BlogSettings.AuthorProfileTemplate.ID);
                string currentUsername = writeBlogPost.UserInformation.Profile.UserName.Replace("\\", "");

                using (new SecurityDisabler())
                {
                    if (blogTemplate == null)
                    {
                        Sitecore.Diagnostics.Log.Audit(this, "Template: '" + writeBlogPost.ArticleRootPath.ID + "' is not available");
                        return View(writeBlogPost);
                    }
                    if (articleRoot == null)
                    {
                        Sitecore.Diagnostics.Log.Audit(this, "Root Item: '" + writeBlogPost.ArticleRootPath.ID + "' is not available");
                        return View(writeBlogPost);
                    }
                    if (authorProfileRoot != null && !authorProfileRoot.Axes.GetDescendants().Where(c => c.Name.Equals(currentUsername)).Any())
                    {
                        Item newAuthor = authorProfileRoot.Add(currentUsername.Trim(), authorProfileTemplate);
                    }

                    // Convert item name
                    string convertItemName = writeBlogPost.Title.Trim();
                    string ex = @"[$@&+,:;/=?@#|~{}'`=+<>\[\].^*()\%!’" + "\"" + "]";
                    var regexItem = new System.Text.RegularExpressions.Regex(ex);
                    if (regexItem.IsMatch(convertItemName))
                    {
                        convertItemName = System.Text.RegularExpressions.Regex.Replace(convertItemName, ex, "");
                    }

                    string preg = Sitecore.Configuration.Settings.GetSetting("ItemNameValidation");
                    Sitecore.Diagnostics.Error.Assert(System.Text.RegularExpressions.Regex.IsMatch(convertItemName, preg), "Item name is invalid.");

                    // Update Title field
                    newArticle = articleRoot.Add(convertItemName, blogTemplate);
                    newArticle.Editing.BeginEdit();
                    newArticle.Fields["__Display name"].Value = convertItemName + "_Draft";
                    newArticle.Fields["Title"].Value = writeBlogPost.Title;
                    newArticle.Fields["Meta Title"].Value = writeBlogPost.Title;
                    newArticle.Fields["Author"].Value = writeBlogPost.GetUserFullName(writeBlogPost.UserInformation.Profile.UserName);
                    newArticle.Fields["pubDate"].Value = newArticle.Fields["__Created by"].Value;
                    Sitecore.Workflows.IWorkflow workflow = writeBlogPost.MasterDb.WorkflowProvider.GetWorkflow(newArticle.Fields["__Default workflow"].Value);
                    workflow.Start(newArticle);
                    newArticle.Editing.EndEdit();
                }
                if (newArticle != null)
                {
                    //// Redirect to new article item as relative path
                    //string newArticlePath = newArticle.Paths.Path.ToString().ToLower();
                    //newArticlePath = newArticlePath.Replace(Sitecore.Context.Data.Site.RootPath.ToLower(), "");
                    //newArticlePath = newArticlePath.Replace(Sitecore.Context.Data.Site.StartItem.ToLower(), "");

                    return Redirect("/?sc_mode=edit&sc_itemid=" + newArticle.ID.ToString() + "&sc_lang=en");
                }
            }
            else
            {
                return Redirect("/?sc_mode=preview&sc_itemid=" + Sitecore.Context.Item.ID.ToString() + "&sc_lang=en");
            }
            return View(writeBlogPost);
        }
    }
}