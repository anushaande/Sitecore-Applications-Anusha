using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.ContentSearch;
using System.Text;

namespace HealthIS.Apps.MVC.Blog.Models
{
    public class Pagination
    {
        public IEnumerable<Item> Items { get; set; }
        public Pager Pager { get; set; }

        public StringBuilder HtmlMarkup(Pager pager, string pageUrl, string pageNumber)
        {
            StringBuilder sb = new StringBuilder();
            int pageNum = 0;
            if (!String.IsNullOrEmpty(pageNumber))
            {
                int.TryParse(pageNumber, out pageNum);
            }

            string parameterKey = (HttpContext.Current.Request.QueryString.Keys.Count <= 0 || (HttpContext.Current.Request.QueryString.Keys.Count == 1 && HttpContext.Current.Request["page"] != null) ? "?page=" : "&page=");

            sb.Append("<ul class='pagination'>");
            if (pager.CurrentPage > 1)
            {
                sb.Append("<li><a href='" + pageUrl + parameterKey + (pageNum - 1) + "'>&#171;</a></li>");
            }

            for (var page = pager.StartPage; page <= pager.EndPage; page++)
            {
                sb.Append("<li class='" + (page == pager.CurrentPage || (pageNum == 0 && page == 1) ? "active" : "") + "'><a href='" + pageUrl + parameterKey + page + "'>" + page.ToString() + "</a></li>");
            }

            if (pager.CurrentPage < pager.TotalPages)
            {
                sb.Append("<li><a href='" + pageUrl + parameterKey + (pageNum + 1) + "'>&#187;</a></li>");
            }
            sb.Append("</ul>");
            return sb;
        }
    }

    public class Pager
    {
        public Pager(int totalItems, int? page, int pageSize = 10)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        //public Pagination ListPagination(List<Item> searchResults, int pageSize)
        //{
        //    // List Pagination
        //    string blogRoot = BlogSettings.ArticleRootPath.Paths.GetFriendlyUrl();
        //    int activePage;
        //    int.TryParse(HttpContext.Current.Request.QueryString["Page"], out activePage);
        //    var pager = new Pager(searchResults.Count(), activePage, pageSize);
        //    var pagination = new Pagination
        //    {
        //        Items = searchResults.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
        //        Pager = pager
        //    };

        //    return pagination;
        //}

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }
}
