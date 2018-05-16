using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HealthIS.Apps.MVC.Blog.Lucene
{
    #region Health IS Blog - Category and Tag Facets
    public class HealthISBlogCategory : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;
            string categoryFacet = BlogSettings.CategoryFacetSource;
            ID healthIsBlogCategory = indexableItem.Item.Database.GetItem(categoryFacet).ID;

            return indexableItem == null ? null : indexableItem.Item.GetMultiListValues(healthIsBlogCategory).Select(category => category.DisplayName).ToList();
        }

        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }

    public class HealthISBlogTag : IComputedIndexField
    { 
        public object ComputeFieldValue(IIndexable indexable)
        {
            var indexableItem = indexable as SitecoreIndexableItem;
            string tagFacet = BlogSettings.TagFacetSource;
            ID healthIsBlogTag = indexableItem.Item.Database.GetItem(tagFacet).ID;

            return indexableItem == null ? null : indexableItem.Item.GetMultiListValues(healthIsBlogTag).Select(tag => tag.DisplayName).ToList();
        }
        
        public string FieldName { get; set; }
        public string ReturnType { get; set; }
    }
    #endregion

    public static class HelperMethods
    {
        public static IEnumerable<Item> GetMultiListValues(this Item item, ID fieldId)
        {
            return (new MultilistField(item.Fields[fieldId])).GetItems();
        }
    }
}

