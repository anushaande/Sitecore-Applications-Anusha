using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HealthIS.Apps.MVC
{
    public class GetDynamicKeyAllowedRenderings : GetAllowedRenderings
    {
        public new void Process(GetPlaceholderRenderingsArgs args)
        {
            string placeholderKey = args.PlaceholderKey;

            //if (placeholderKey.Contains("_dph_"))
            //{
            //    placeholderKey = placeholderKey.Substring(0, placeholderKey.LastIndexOf("_dph_"));
            //    if (placeholderKey.LastIndexOf("/") >= 0)
            //    {
            //        placeholderKey = placeholderKey.Substring(placeholderKey.LastIndexOf("/") + 1);
            //    }
            //}
            //else
            //{
            //    return;
            //}

            Assert.IsNotNull((object)args, "args");
            Item placeholderItem = (Item)null;
            if (ID.IsNullOrEmpty(args.DeviceId))
            {
                placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase, args.LayoutDefinition);
            }
            else
            {
                using (new DeviceSwitcher(args.DeviceId, args.ContentDatabase))
                    placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase, args.LayoutDefinition);
            }
            List<Item> list = (List<Item>)null;
            if (placeholderItem != null)
            {
                args.HasPlaceholderSettings = true;
                bool allowedControlsSpecified;
                list = this.GetRenderings(placeholderItem, out allowedControlsSpecified);
                if (allowedControlsSpecified)
                {
                    args.CustomData["allowedControlsSpecified"] = true;
                    args.Options.ShowTree = false;
                }
            }

            if (list != null)
            {
                if (args.PlaceholderRenderings == null)
                {
                    args.PlaceholderRenderings = new List<Item>();
                }
                args.PlaceholderRenderings.AddRange(list);
            }
        }
    }
}
