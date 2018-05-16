using Sitecore.Collections;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Search;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Blog.Models
{
    public class List : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item Item { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item PageItem { get; set; }
        public string url { get; set; }
        public int articlesPerPage { get; set; }
        public string listType { get; set; }
        public Pagination Pag { get; set; }

        public List<Item> allArticles = new List<Item>();

        private string catFilter = string.Empty;
        private string tagFilter = string.Empty;

        private string searchIndexName = HealthIS.Apps.BlogSettings.SearchIndexName;
        public Item articleRootPath = HealthIS.Apps.BlogSettings.ArticleRootPath;
        private Item templatePath = HealthIS.Apps.BlogSettings.TemplatePath;
        private Item categoryRepository = HealthIS.Apps.BlogSettings.CategoryRepository;
        private Item tagRepository = HealthIS.Apps.BlogSettings.TagRepository;
        private string categoryFacetSource = HealthIS.Apps.BlogSettings.CategoryFacetSource;
        private string tagFacetSource = HealthIS.Apps.BlogSettings.TagFacetSource;
        private Item tagTemplatePath = HealthIS.Apps.BlogSettings.TagTemplatePath;
        private string imageLibrary = HealthIS.Apps.BlogSettings.ImageLibrary;


        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);

            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;

            url = PageItem.Paths.Path.Replace("/sitecore/content", "");

            int x = 0;
            string value = articleRootPath.Fields["Articles Per Page"].Value;
            articlesPerPage = Int32.TryParse(value, out x) ? x : 8;

            listType = string.Empty;

            if (IsBlogArticleRoot()) // article root
            {
                catFilter = PageItem.Fields["Category Filter"].Value;
                tagFilter = PageItem.Fields["Tag Filter"].Value;
            }

            var getSearchIndex = Sitecore.ContentSearch.ContentSearchManager.GetIndex(searchIndexName);
            using (var context = getSearchIndex.CreateSearchContext())
            {
                var results = context.GetQueryable<SearchResultItem>();
                foreach (var result in results)
                {
                    Item item = result.GetItem();
                    allArticles.Add(item);
                }
            }
        }

        // category phrase lookup by key
        public string GetCategoryByKey(string key)
        {
            string phrase = string.Empty;            
            if (categoryRepository != null)
            {
                phrase = categoryRepository.Axes.GetDescendants().FirstOrDefault(x => x["Key"] == key).Fields["Phrase"].Value;
            }
            return phrase;
        }

        public List<Item> GetRelatedPosts(string searchTerm)
        {
            List<Item> allRelatedItems = new List<Item>();
            if (!String.IsNullOrEmpty(searchTerm))
            {
                string filterName = listType.Equals("category") ? catFilter : tagFilter;
                var query = PredicateBuilder.True<SearchResultItem>();
                query = query.And(i => i[filterName].Equals(searchTerm)).And(i => i.ItemId != PageItem.ID);
                string index = String.Format("sitecore_{0}_index", Sitecore.Context.Database.Name);
                using (var context = ContentSearchManager.GetIndex(index).CreateSearchContext())
                {
                    var results = context.GetQueryable<SearchResultItem>().Where(query);
                    foreach (var r in results)
                    {
                        allRelatedItems.Add(r.GetItem());
                    }
                }
            }
            return allRelatedItems;
        }

        public bool IsBlogArticleRoot()
        {
            if (articleRootPath.Paths.Path.Equals(PageItem.Paths.Path)) { return true; }
            else { return false; }
        }
    }


    public class Pagination
    {
        public Pagination(int totalNumArticles, int? renPageNum, int maxNumArticles)
        {
            int articleEnd = 0, articleStart = 0;
            int totalNumberOfArticles = totalNumArticles;
            int maxNumberOfArticlesOnPage = maxNumArticles;
            int totalPages = totalNumberOfArticles / maxNumberOfArticlesOnPage;
            if (totalNumberOfArticles % maxNumberOfArticlesOnPage != 0)
            {
                totalPages = totalPages + 1;
            }            
            if (renPageNum != null && renPageNum > 0 && renPageNum <= totalPages)
            {               
                articleEnd = (int)renPageNum * maxNumberOfArticlesOnPage;
                articleStart = (articleEnd - maxNumberOfArticlesOnPage) + 1;
                if (((int)renPageNum == totalPages) && (totalNumberOfArticles < articleEnd))
                {
                    articleEnd = totalNumberOfArticles;
                }
                RenderedPageNumber = (int)renPageNum;
            }
            else
            {
                articleEnd = maxNumberOfArticlesOnPage;
                articleStart = (articleEnd - maxNumberOfArticlesOnPage) + 1;
                RenderedPageNumber = 1;
            }
            
            TotalNumberOfArticles = totalNumberOfArticles;
            MaxNumberOfArticleOnPage = maxNumberOfArticlesOnPage;
            TotalPages = totalPages;
            ArticleStart = articleStart;
            ArticleEnd = articleEnd;
        }

        public int TotalNumberOfArticles { get; private set; }
        public int RenderedPageNumber { get; private set; }
        public int MaxNumberOfArticleOnPage { get; private set; }
        public int TotalPages { get; private set; }
        public int ArticleStart { get; private set; }
        public int ArticleEnd { get; private set; } 
    }
}