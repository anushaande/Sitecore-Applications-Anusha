using Sitecore.Collections;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Search;
using Sitecore.Security.Accounts;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Blog.List.Models
{
    public class ListView : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item Item { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item PageItem { get; set; }
        public string url { get; set; }
        public Pagination Pag { get; set; }
        public List<Item> allBlogArticles = new List<Item>();
        public List<Item> filteredArticles = new List<Item>();
        public Item topStory { get; set; }
        public List<Item> otherArticles = new List<Item>();  // allBlogArticles, excluding topStory

        public Item articleRootPath = HealthIS.Apps.BlogSettings.ArticleRootPath;
        private string searchIndexName = HealthIS.Apps.BlogSettings.SearchIndexName;        
        private Item categoryRepository = HealthIS.Apps.BlogSettings.CategoryRepository;

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);

            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;

            url = Sitecore.Links.LinkManager.GetItemUrl(PageItem);
        }

        // gets all articles that belong to this blog
        public void SetAllBlogArticles()
        {
            var getSearchIndex = Sitecore.ContentSearch.ContentSearchManager.GetIndex(searchIndexName);
            using (var context = getSearchIndex.CreateSearchContext())
            {
                var results = context.GetQueryable<SearchResultItem>();
                foreach (var result in results)
                {
                    Item item = result.GetItem();
                    allBlogArticles.Add(item);
                }
            }
        }
        
        // sets top story for IS blog homepage
        public void SetTopStory()
        {
            if (allBlogArticles != null)
            {
                otherArticles = allBlogArticles.OrderByDescending(item => item.Fields["pubDate"].Value).ToList();
                topStory = otherArticles.First();
                otherArticles.RemoveAt(0);
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

        // gets all articles written by specified author
        public void GetArticlesByAuthor(string username)
        {
            if (!String.IsNullOrEmpty(username))
            {
                filteredArticles = allBlogArticles.Where(item => item.Statistics.CreatedBy.Replace("\\", "").ToLower().Equals(username))
                                    .OrderByDescending(item => item.Fields["pubDate"].Value).ToList();
            }
        }

        // get author's full name from username        
        public string GetAuthorFullName(string username)
        {
            string fullName = string.Empty;
            if (!String.IsNullOrEmpty(username))
            {
                username = username.Insert(6, "\\"); // add backslash back so 'hscnetusername' => 'hscnet\username'
                string name = User.FromName(username, false).Profile.FullName;
                if (!String.IsNullOrEmpty(name))
                {
                    string[] str = name.Split(','); // format name from 'Last, First' to 'First Last'
                    fullName = str[1].TrimStart() + " " + str[0];
                }                  
            }
            return fullName;
        }

        // will be a global function, can be deleted later
        public List<Item> GetSearchPredicate(string filterName, string searchTerm)
        {
            List<Item> allRelatedItems = new List<Item>();
            if (!String.IsNullOrEmpty(searchTerm))
            {
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
    }

    // this class will also be global later
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