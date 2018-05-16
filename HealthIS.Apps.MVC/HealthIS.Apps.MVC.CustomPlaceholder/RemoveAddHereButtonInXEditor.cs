using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Layouts;
using Sitecore.Mvc.Pipelines.Response.RenderPlaceholder;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthIS.Apps.MVC.CustomPlaceholder
{
    public class RemoveAddHereButtonInXEditor
    {
        public void Process(GetPlaceholderRenderingsArgs args)
        {
            Assert.IsNotNull(args, "args");

            LayoutDefinition layoutDefinition = LayoutDefinition.Parse(args.LayoutDefinition);
            var device = layoutDefinition.GetDevice(args.DeviceId.ToString());

            // Initial repository path is "/sitecore/system/Modules/USF Health Restriction Manager/Remove AddHere Button"
            Item removeAddHereButtonItem = Sitecore.Data.Database.GetDatabase("master").GetItem("/sitecore/system/Modules/USF Health Restriction Manager/Remove AddHere Button");

            if (removeAddHereButtonItem == null)
            {
                Assert.IsNull(removeAddHereButtonItem, "Item is null");
                return;
            }

            MultilistField listOfRenderingItemsAddHere = removeAddHereButtonItem.Fields["List of Rendering Items"];
            string listOfUserRolesAddHere = removeAddHereButtonItem.Fields["List of User Roles"].Value.Trim();
            List<string> eachListOfUserRolesAddHere = listOfUserRolesAddHere.Split(';').ToList();

            // Remove "Add Here" button if there are duplicated rendering items in the same placeholder, for all users, except super user
            if (eachListOfUserRolesAddHere.Where(e => User.Current.IsInRole(e)).Any())
            {
                args.CustomData["removeAddHereButton"] = "true";
                List<Item> placeholderRenderings = args.PlaceholderRenderings;

                if (placeholderRenderings != null && !args.PlaceholderKey.Contains("/Dynamic"))
                {
                    List<Item> allChildItems = new List<Item>();

                    // Main control or folder
                    foreach (ID selectedRenderingItem in listOfRenderingItemsAddHere.TargetIDs)
                    {
                        // Apeend all children
                        Item childItems = Sitecore.Data.Database.GetDatabase("master").GetItem(selectedRenderingItem);
                        AppendAllChildItems(allChildItems, childItems);

                        IEnumerable<Item> foundRenderingItem = placeholderRenderings.Where(x => x.ID == selectedRenderingItem);
                        foreach (var eachRendering in foundRenderingItem.ToList())
                        {
                            if (device.GetRenderings(eachRendering.ID.ToString()).Count() >= 1)
                            {
                                placeholderRenderings.Remove(eachRendering);
                            }
                        }
                    }

                    // Children from main control or folder
                    foreach (var eachChild in allChildItems)
                    {
                        IEnumerable<Item> foundRenderingItem = placeholderRenderings.Where(x => x.ID == eachChild.ID);
                        foreach (var eachRendering in foundRenderingItem.ToList())
                        {
                            if (device.GetRenderings(eachRendering.ID.ToString()).Count() >= 1)
                            {
                                placeholderRenderings.Remove(eachRendering);
                            }
                        }
                    }
                }
            }
        }

        public void AppendAllChildItems(List<Item> itemList, Item item)
        {
            foreach (Item childItem in item.Children)
            {
                itemList.Add(childItem);
                AppendAllChildItems(itemList, childItem);
            }
        }
    }
}
