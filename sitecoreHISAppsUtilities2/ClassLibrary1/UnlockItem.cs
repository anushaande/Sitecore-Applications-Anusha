using System;
using Sitecore.Web.Authentication;
using System.Collections.Generic;
using Sitecore.Pipelines.Logout;
using Sitecore;
using Sitecore.Data.Items;

//// Config file: /Website/App_Config/Include/HealthIS.UnlockItem.config
//<?xml version="1.0" encoding="UTF-8"?>
//    <configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
//    <sitecore>
//        <scheduling>
//            <!-- Agent to kick users who are idle for 55 minutes, HealthIS.UnlockItem.UnlockItemWhenIdle -->
//            <agent type="HealthIS.UnlockItem.UnlockItemWhenIdle, HealthIS.Apps.Util" method="Run" interval="00:05:00">
//                <param desc="maximumIdleTime">00:55:00</param>
//            </agent>
//        </scheduling>
//    </sitecore>
//</configuration>
////

namespace HealthIS.UnlockItem
{
    public class UnlockItemWhenIdle
    {
        private TimeSpan maximumIdleTime;

        public UnlockItemWhenIdle(string maximumIdleTime)
        {
            this.maximumIdleTime = TimeSpan.Parse(maximumIdleTime);
        }

        public void Run()
        {
            List<DomainAccessGuard.Session> userSessionList = DomainAccessGuard.Sessions;

            if (userSessionList != null && userSessionList.Count > 0)
            {
                foreach (DomainAccessGuard.Session userSession in userSessionList.ToArray())
                {
                    TimeSpan span = (TimeSpan)(DateTime.Now - userSession.LastRequest.AddHours(-5));

                    if (span > this.maximumIdleTime)
                    {
                        string currentUserName = userSession.UserName;
                        Sitecore.Diagnostics.Log.Audit("Kicked out user is : fast:/sitecore/content//*[@__lock='%" + currentUserName + "%']", this);

                        var database = Sitecore.Configuration.Factory.GetDatabase("master");
                        Item[] items = database.SelectItems("fast:/sitecore/content//*[@__lock='%" + currentUserName + "%']");
                        
                        foreach (var item in items)
                        {
                            item.Editing.BeginEdit();
                            item.Locking.Unlock();
                            item.Editing.EndEdit();

                            Sitecore.Diagnostics.Log.Info("Item unlocked: \"" + item.Paths.FullPath + "\" by idle user (" + currentUserName + ") for " + this.maximumIdleTime.Minutes + "min", this);
                        }
                    }
                }
            }
        }
    }

    public class UnlockItemWhenLogout
    {
        public void Process(LogoutArgs args)
        {
            string currentUserName = Context.User.Profile.UserName;
            var database = Sitecore.Configuration.Factory.GetDatabase("master");
            Item[] items = database.SelectItems("fast:/sitecore/content//*[@__lock='%" + currentUserName + "%']");
            foreach (var item in items)
            {
                item.Editing.BeginEdit();
                item.Locking.Unlock();
                item.Editing.EndEdit();

                Sitecore.Diagnostics.Log.Info("Item unlocked: " + item.Paths.FullPath + " by user (" + currentUserName + ") logout", this);
            }
        }
    }
}
