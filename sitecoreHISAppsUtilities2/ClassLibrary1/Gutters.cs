using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor.Gutters;
using System;
using System.Linq;

namespace HealthIS.Apps.Gutters
{
    class PublishStatus : GutterRenderer
    {
        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            Assert.ArgumentNotNull(item, "item");

            GutterIconDescriptor descriptor = new GutterIconDescriptor();

            if (Database.GetDatabase("web").GetItem(item.ID) == null)
            {
                if (Helpers.IsPageItem(item) && ItemSetToUnpublishNow(item))
                {
                    // item has been unpublished
                    descriptor.Icon = "applications/32x32/delete2.png"; // RED "DELETED" X
                    descriptor.Tooltip = "Item Unpublished";
                }
            }          
            return descriptor;
        }

        // check to see if unpublish date has been set on item, and is in the past
        private bool ItemSetToUnpublishNow(Item item)
        {
            if (item != null)
            {               
                DateTime unpubDate = item.Publishing.UnpublishDate; // UTC time
                if(unpubDate != DateTime.MaxValue)
                {                    
                    if (DateTime.UtcNow > unpubDate) { return true; } // item unpublish date set in the past                                       
                    else { return false; } // item unpublish date set in the future
                }
                else
                {
                    // unpublish date not set on item, check parent for unpublish date
                    if (ItemSetToUnpublishNow(item.Parent)) { return true; }
                }
            } return false;
        }
    }

    public class Helpers
    {
        // check to see if item is a page item (has layout)
        public static bool IsPageItem(Item item)
        {
            if (item != null)
            {
                DeviceRecords devices = item.Database.Resources.Devices;
                DeviceItem defaultDevice = devices.GetAll().Where(d => d.Name.ToLower() == "default").First();
                LayoutItem layout = item.Visualization.GetLayout(defaultDevice);
                if (layout == null) { return false; } // item does not have page layout                
                else { return true; } // item has page layout                
            }
            return false;
        }
    }
}