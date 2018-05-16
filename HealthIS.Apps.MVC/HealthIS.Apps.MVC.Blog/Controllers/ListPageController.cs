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

namespace HealthIS.Apps.MVC.Blog.ListPage
{
    public class Blog_HealthISController : SitecoreController
    {
        #region ======= HealthIS Blog Hompage =======
        [HttpGet]
        public ActionResult HealthIS_Blog_HomePage(int? page)
        {
            HealthIS.Apps.MVC.Blog.Models.ListPage lp = new HealthIS.Apps.MVC.Blog.Models.ListPage();
            lp.PageSize = Int32.Parse(Sitecore.Context.Item.Fields["Page Size"].Value);

            lp.AllArticles = lp.ArticleRootPath.Axes.GetDescendants()
                                .Where(l => l.Versions.IsLatestVersion())
                                .Where(t => t.TemplateName == lp.ArticleTemplateName)
                                .OrderBy(o => o.Statistics.Created)
                                .Reverse()
                                .ToList();
            return PartialView(lp);
        }
        #endregion

        #region ======= HealthIS Blog Search Page =======
        [HttpGet]
        public ActionResult HealthIS_Blog_SearchPage(int? page)
        {
            if (Request.QueryString["q"] == null || String.IsNullOrEmpty(Request.QueryString["q"].Trim()))
            {
                return Redirect("/is/blog");
            }
            else
            {
                HealthIS.Apps.MVC.Blog.Models.BlogSearch bs = new HealthIS.Apps.MVC.Blog.Models.BlogSearch();
                HealthIS.Apps.MVC.Blog.Models.ResultItem ri = new HealthIS.Apps.MVC.Blog.Models.ResultItem();

                bs.SearchQuery = Request.QueryString["q"]; // search query
                bs.SearchResults = new List<Item>();

                // sitecore search index
                var index = ContentSearchManager.GetIndex(bs.SearchIndexName);
                bs.PageSize = Int32.Parse(Sitecore.Context.Item.Fields["Page Size"].Value);
                using (var context = index.CreateSearchContext())
                {
                    //var results =
                    //    context.GetQueryable<HealthIS.Apps.MVC.Models.BlogSearchResult>()
                    //        .Where(
                    //            resultItem =>
                    //                resultItem.PageTitle.Like(searchQuery) ||
                    //                resultItem.PageContent.Contains(searchQuery))
                    //        .GetResults();

                    var results =
                        context.GetQueryable<HealthIS.Apps.MVC.Blog.Models.ResultItem>()
                            .Where(resultItem =>
                                    (resultItem.PageContent.Contains(bs.SearchQuery) ||
                                    resultItem.PageTitle.Contains(bs.SearchQuery) ||
                                    resultItem.PageDescription.Contains(bs.SearchQuery)) &&
                                    resultItem.IsLatestVersion
                                )
                            .OrderByDescending(o => o.CreatedDate);
                    foreach (var result in results)
                    {
                        Item item = result.GetItem();
                        bs.SearchResults.Add(item);
                    }
                }

                return PartialView(bs);
            }
        }
        #endregion

        #region ======= HealthIS Blog Category and Tag Page =======
        [HttpGet]
        public ActionResult HealthIS_Blog_CategoryAndTagPage(int? page)
        {
            HealthIS.Apps.MVC.Blog.Models.ListPage lp = new HealthIS.Apps.MVC.Blog.Models.ListPage();
            lp.PageSize = Int32.Parse(lp.CurrentPage.Fields["Page Size"].Value);

            if (Request["name"] == null || String.IsNullOrEmpty(Request["name"].Trim()))
            {
                lp.ParameterName = "No Result Found";
                //return PartialView(lp);
                return Redirect("/is/blog");
            }

            string searchTerm = Request["name"].ToLower().Trim();
            string filterName = "";

            if (lp.CurrentPage.Name.ToLower().Trim() == "category" && Request["name"] != null)
            {
                filterName = "healthisblogcategory";
                Item categoryItem = lp.CategoryRepository.Axes.GetDescendants().Where(d => d.Fields["Key"].Value.ToLower() == searchTerm).FirstOrDefault();
                if (categoryItem != null)
                {
                    lp.ParameterName = categoryItem.Fields["Phrase"].Value;
                }
                else
                {
                    lp.ParameterName = searchTerm;
                }
            }
            else if (lp.CurrentPage.Name.ToLower().Trim() == "tag" && Request["name"] != null)
            {
                filterName = "healthisblogtag";
                lp.ParameterName = searchTerm;
            }
            else
            {
                return PartialView(lp);
            }

            if (!String.IsNullOrEmpty(filterName))
            {
                if (String.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = "";
                }

                lp.AllArticles = TaxonomyFilter.GetSearchPredicate(lp.CurrentPage, filterName, searchTerm)
                                .Where(l => l != null && l.Versions.IsLatestVersion())
                                //.Distinct()
                                .OrderBy(o => o.Statistics.Created)
                                .Reverse()
                                .ToList();
            }

            return PartialView(lp);
        }
        #endregion

        #region ======= HealthIS Blog Author Page =======
        [HttpGet]
        public ActionResult HealthIS_Blog_AuthorPage(int? page)
        {
            HealthIS.Apps.MVC.Blog.Models.ListPage lp = new HealthIS.Apps.MVC.Blog.Models.ListPage();
            string searchTerm = Request["username"] == null ? "" : Request["username"].ToLower().Trim();
            lp.PageSize = Int32.Parse(Sitecore.Context.Item.Fields["Page Size"].Value);

            if (String.IsNullOrEmpty(Request["username"]) || Request["username"] == null)
            {
                return Redirect("/is/blog");
            }
            else
            {
                lp.AllArticles = lp.ArticleRootPath.Axes.GetDescendants()
                                .Where(l => l.Versions.IsLatestVersion())
                                .Where(t => t.TemplateName == lp.ArticleTemplateName)
                                .Where(c => c.Statistics.CreatedBy.Replace("\\", "").Equals(searchTerm))
                                .OrderBy(o => o.Statistics.Created)
                                .Reverse()
                                .ToList();
            }

            return PartialView(lp);
        }
        #endregion
    }
}