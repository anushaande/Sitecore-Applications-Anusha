using log4net;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthIS.Apps.MVC.UserActivityLog
{
    public static class GetUserActivity
    {
        private static ILog log;
        public static ILog Log
        {
            get
            {
                return log ?? (log = LogManager.GetLogger(typeof(GetUserActivity)));
            }
        }
    }

    public class UserEvents
    {
        public Item GetEventItem(EventArgs args)
        {
            Sitecore.Events.SitecoreEventArgs eventArgs = args as Sitecore.Events.SitecoreEventArgs;
            Item item = eventArgs.Parameters[0] as Item;
            return item;
        }
        public void ItemAdded(object sender, EventArgs args)
        {
            Item item = GetEventItem(args);
            GetUserActivity.Log.Info("[" + (object)Context.User.Name + "] created item ::: " + item.Paths.FullPath);
        }
        public void ItemCopied(object sender, EventArgs args)
        {
            Sitecore.Events.SitecoreEventArgs eventArgs = args as Sitecore.Events.SitecoreEventArgs;
            Item orgItem = eventArgs.Parameters[0] as Item;
            Item movedItem = eventArgs.Parameters[1] as Item;

            GetUserActivity.Log.Info("[" + (object)Context.User.Name + "] copied item from \"" + orgItem.Paths.FullPath + "\" to \"" + movedItem.Paths.FullPath + "\"");
        }
        public void ItemSaved(object sender, EventArgs args)
        {
            Item item = GetEventItem(args);
            if (!item.Paths.FullPath.ToLower().StartsWith("/sitecore/system"))
            {
                GetUserActivity.Log.Info("[" + (object)Context.User.Name + "] saved item ::: " + item.Paths.FullPath);
            }
        }
        public void ItemDeleted(object sender, EventArgs args)
        {
            Sitecore.Events.SitecoreEventArgs eventArgs = args as Sitecore.Events.SitecoreEventArgs;
            Item item = eventArgs.Parameters[0] as Item;

            GetUserActivity.Log.Info("[" + (object)Context.User.Name + "] deleted item ::: " + item.Name);
        }
        public void ItemMoved(object sender, EventArgs args)
        {
            Item item = GetEventItem(args);
            GetUserActivity.Log.Info("[" + (object)Context.User.Name + "] moved item ::: " + item.Paths.FullPath);
        }

        public void ItemRenamed(object sender, EventArgs args)
        {
            Item item = GetEventItem(args);
            GetUserActivity.Log.Info("[" + (object)Context.User.Name + "] renamed item ::: " + item.Paths.FullPath);
        }

        public void ItemVersionAdded(object sender, EventArgs args)
        {
            Item item = GetEventItem(args);
            GetUserActivity.Log.Info("[" + (object)Context.User.Name + "] added Item Version ::: " + item.Paths.FullPath);
        }

        public void PublishEnd(object sender, EventArgs args)
        {
            Sitecore.Events.SitecoreEventArgs eventArgs = args as Sitecore.Events.SitecoreEventArgs;
            var publisher = eventArgs.Parameters[0] as Sitecore.Publishing.Publisher;
            var item = publisher.Options.RootItem;

            GetUserActivity.Log.Info("[" + (object)Context.User.Name + "] finished publishing ::: " + item.Paths.FullPath);
        }
    }
}
