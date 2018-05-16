using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Security;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HealthIS.Apps.MVC.Blog.Models
{
    public class ListPage
    {
        public string CatFilter { get; set; }
        public string TagFilter { get; set; }
        public string IsBlogArticleRoot { get; set; }
        public string ParameterName { get; set; }
        public List<Item> AllArticles { get; set; }
        public int PageSize { get; set; }
        
        public bool IsExperiencePage
        {
            get
            {
                return Sitecore.Context.PageMode.IsExperienceEditor;
            }
        }
        public Database CurrentDb
        {
            get
            {
                return CurrentPage.Database;
            }
        }
        public Item CurrentPage
        {
            get
            {
                return RenderingContext.Current.ContextItem;

                //return Sitecore.Context.Item;
            }
        }

        public Item ArticleRootPath
        {
            get
            {
                return BlogSettings.ArticleRootPath;
            }
        }
        public string ArticleTemplateName
        {
            get
            {
                return BlogSettings.TemplatePath.Name;
            }
        }

        public Item CategoryRepository 
        {
            get 
            {
                return BlogSettings.CategoryRepository;
            }
        }

        public Pagination GetPagination
        {
            get
            {
                return new Pagination();
            }

        }
    }

    public class BlogSearch
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<Item> SearchResults { get; set; }
        public string SearchQuery { get; set; }
        public int PageSize { get; set; }
        public Pagination GetPagination
        {
            get
            {
                return new Pagination();
            }

        }
        public string SearchIndexName
        {
            get
            {
                return BlogSettings.SearchIndexName;
            }
        }
        //public string ConvertListItemToItemName(Item item, string fieldName)
        //{
        //    string fieldValue = item.Fields[fieldName].Value;
        //    if (!String.IsNullOrEmpty(fieldValue))
        //    {
        //        List<string> nameList = new List<string>();
        //        List<string> valueList = fieldValue.Split('|').ToList();
        //        valueList.ForEach(delegate(String catName)
        //        {
        //            nameList.Add(item.Database.GetItem(catName).Name);
        //        });

        //        return nameList.Aggregate((i, j) => i + ", " + j);
        //    }
        //    return "";
        //}
    }

    public class ResultItem : SearchResultItem
    {
        [IndexField("Title")]
        public string PageTitle { get; set; }

        [IndexField("Content")]
        public string PageContent { get; set; }

        [IndexField("Description")]
        public string PageDescription { get; set; }

        [IndexField("_latestversion")]
        public bool IsLatestVersion { get; set; }
    }
}