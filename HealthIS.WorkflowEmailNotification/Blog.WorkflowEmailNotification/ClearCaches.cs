using System;
using HealthIS.Apps;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Caching;



namespace Blog.WorkflowEmailNotification
{
    public static class ClearCaches
    {
        public static void ClearDatabaseCache(Database db, Item item)
        {
            // Clear item's Item Cache
            Sitecore.Context.Database.Caches.ItemCache.RemoveItem(item.ID);

            // Clear item's Data Cache
            Sitecore.Context.Database.Caches.DataCache.RemoveItemInformation(item.ID);

            // Clear item's Standard Value Cache
            Sitecore.Context.Database.Caches.StandardValuesCache.RemoveKeysContaining(item.ID.ToString());

            // Clear item's Prefetch Cache
            GetSqlPrefetchCache(item.Database.Name).Remove(item);
        }

        public static Cache GetSqlPrefetchCache(string database)
        {
            return Sitecore.Caching.CacheManager.FindCacheByName("SqlDataProvider - Prefetch data(" + database + ")");
        }
    }
}
