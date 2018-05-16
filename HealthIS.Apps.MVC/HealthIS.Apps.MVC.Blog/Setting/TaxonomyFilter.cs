using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthIS.Apps.MVC.Blog
{
    public static class TaxonomyFilter
    {
        // Get related posts by category or tag
        public static List<Item> GetSearchPredicate(Item currentPage, string filterName, string searchTerm)
        {
            var query = PredicateBuilder.True<SearchResultItem>();
            query = query.And(i => i[filterName].Equals(searchTerm)).And(i => i.ItemId != currentPage.ID);
            //var indexName = String.Format("sitecore_{0}_index", currentPage.Database.Name);
            var indexName = String.Format("health_is_blog_search_{0}", currentPage.Database.Name);
            var index = ContentSearchManager.GetIndex(indexName);
            List<Item> allRelatedItems = new List<Item>();

            using (var context = index.CreateSearchContext())
            {
                var results = context.GetQueryable<SearchResultItem>().Where(query);
                foreach (var r in results)
                {
                    allRelatedItems.Add(r.GetItem());
                }
                return allRelatedItems;
            }
        }
    }
}
