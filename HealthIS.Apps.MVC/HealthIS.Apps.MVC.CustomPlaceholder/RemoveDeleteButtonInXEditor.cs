using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Pipelines.GetChromeData;
using System.Text.RegularExpressions;
using Sitecore.Diagnostics;
using Sitecore.Data.Items;
using Sitecore.Web.UI.PageModes;
using Sitecore.Security.Accounts;
using Sitecore.Layouts;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.XslControls;

namespace HealthIS.Apps.MVC.CustomPlaceholder
{
    public class RemoveDeleteButtonInXEditor : GetPlaceholderChromeData
    {
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");
            if ("placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
            {
                string placeholderKey = args.CustomData["placeHolderKey"] as string;

                Sitecore.Data.Fields.LayoutField layoutField = new Sitecore.Data.Fields.LayoutField(Context.Item.Fields[Sitecore.FieldIDs.FinalLayoutField]);
                LayoutDefinition layoutDefinition = LayoutDefinition.Parse(layoutField.Value);
                DeviceDefinition deviceDefinition = layoutDefinition.GetDevice(Context.Device.ID.ToString());

                // Initial repository path is "/sitecore/system/Modules/USF Health Restriction Manager/Remove Delete Button"
                Item removeDeleteButtonItem = Context.Item.Database.GetItem("/sitecore/system/Modules/USF Health Restriction Manager/Remove Delete Button");

                if (removeDeleteButtonItem == null)
                {
                    Assert.IsNull(removeDeleteButtonItem, "Item is null");
                    return;
                }

                MultilistField listOfRenderingItems = removeDeleteButtonItem.Fields["List of Rendering Items"];
                string listOfUserRoles = removeDeleteButtonItem.Fields["List of User Roles"].Value.Trim();
                List<string> eachListOfUserRoles = listOfUserRoles.Split(';').ToList();
                

                // Remove "Delete" button in placeholder
                if (eachListOfUserRoles.Where(e => User.Current.IsInRole(e)).Any())
                {
                    args.ChromeData.Custom["removeAddHereButton"] = true;
                    foreach (ID renderingItemId in listOfRenderingItems.TargetIDs)
                    {
                        RenderingItem r = RenderingItem.GetItem(renderingItemId, Context.Data.Database, true);
                        string renderingName = Regex.Replace(r.Name.ToLower(), @"\s+", "");
                        string displayName = Regex.Replace(args.ChromeData.DisplayName.ToLower(), @"\s+", "");

                        // Only when rendering name matches to its display name
                        // Only when rendering item's display name contains rendering name
                        // Only when master rendering item has 'Main Layout' placeholder key
                        if (renderingName == displayName || displayName.Contains(renderingName) || (r.Name.ToLower().Contains(" master") && displayName == "mainlayout"))
                        {
                            args.ChromeData.Custom["editable"] = false;
                        }
                    }
                }
            }
        }
    }
}
