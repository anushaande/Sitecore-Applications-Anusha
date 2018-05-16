using Sitecore.Collections;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Pipelines.ExpandInitialFieldValue;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HealthIS.Apps.MVC
{
    public class Helpers
    {
        public static bool getBoolField(Item item, string fieldName)
        {
            try
            {
                return item.Fields[fieldName].Value.ToString() == "1" ? true : false;
            }
            catch { return false; }

        }
        public static bool getBoolFieldWithDefault(Item item, string fieldName, bool defaultValue)
        {
            try
            {
                return item.Fields[fieldName].Value.ToString() == "1" ? true : false;
            }
            catch { return defaultValue; }

        }

        public static string getStrField(Item item, string fieldName)
        {
            try
            {
                return item.Fields[fieldName].Value;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static int getIntField(Item item, string fieldName)
        {
            try
            {
                return Int32.Parse(item.Fields[fieldName].Value);
            }
            catch { return -1; }
        }

        public static Sitecore.Data.Fields.ImageField getImgField(Item parentItem, string fieldName)
        {
            try
            {
                Sitecore.Data.Fields.ImageField iFld = parentItem.Fields[fieldName];
                return iFld;
            }
            catch { return null; }
        }

        public static Sitecore.Data.Fields.FileField getFileField(Item parentItem, string fieldName)
        {
            try
            {
                Sitecore.Data.Fields.FileField iFld = parentItem.Fields[fieldName];
                return iFld;
            }
            catch { return null; }
        }

        public static Sitecore.Data.Fields.CheckboxField getCBField(Item parentItem, string fieldName)
        {
            try
            {
                Sitecore.Data.Fields.CheckboxField iFld = parentItem.Fields[fieldName];
                return iFld;
            }
            catch { return null; }
        }
        
        public static String getLinkUrl(Sitecore.Data.Fields.LinkField lf)
        {
            switch (lf.LinkType.ToLower())
            {
                case "internal":
                    // Use LinkMananger for internal links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Links.LinkManager.GetItemUrl(lf.TargetItem) : string.Empty;
                case "media":
                    // Use MediaManager for media links, if link is not empty
                    return lf.TargetItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(lf.TargetItem) : string.Empty;
                case "external":
                    // Just return external links
                    return lf.Url;
                case "anchor":
                    // Prefix anchor link with # if link if not empty
                    return !string.IsNullOrEmpty(lf.Anchor) ? "#" + lf.Anchor : string.Empty;
                case "mailto":
                    // Just return mailto link
                    return lf.Url;
                case "javascript":
                    // Just return javascript
                    return lf.Url;
                default:
                    // Just please the compiler, this
                    // condition will never be met
                    return lf.Url;
            }
        }

        // Check if placeholder is empty when it is preview mode
        public static HtmlString placeholderRequired(string placeholderName)
        {
            string msg = "";
            // When preview mode, check if required placeholder doesn't have any embeded root-element
            var renderingReferences = Sitecore.Context.Item.Visualization.GetRenderings(Sitecore.Context.Device, true);
            int renderingsInPlaceholder = renderingReferences.Where(r => r.Placeholder.EndsWith('/' + placeholderName, StringComparison.OrdinalIgnoreCase)).Count(r => !string.IsNullOrWhiteSpace(r.Settings.DataSource));

            //// Updated by Jihyun Kim to enable it in both xEditor and Preview modes, 12/12/2017
            //if (!Sitecore.Context.PageMode.IsExperienceEditor)
            //{
                if (renderingsInPlaceholder < 1)
                {
                    msg = "<div>" + placeholderName + "</div>";
                }
            //}
            return new HtmlString(msg);
        }

        // Check if there is more than one root-element and get RenderingID
        public static string getRenderingId(string placeholderName)
        {
            var renderingReferences = Sitecore.Context.Item.Visualization.GetRenderings(Sitecore.Context.Device, true);
            var renderingsInPlaceholder = renderingReferences.Where(r => r.Placeholder.EndsWith('/' + placeholderName, StringComparison.OrdinalIgnoreCase));
            int rnederingInPlaceholderCount = renderingsInPlaceholder.Count();

            //if (rnederingInPlaceholderCount >= 2)
            //{
            //}
            try
            {
                var renderingIdsInPlaceholder = renderingsInPlaceholder.Where(r => r.Placeholder.Contains('/' + placeholderName)).Last();
                return renderingIdsInPlaceholder.RenderingID.ToString();
            } 
            catch
            {
                return "";
            }
        }

        // Remove rendering item in Layout
        public static bool removeRendering(Item item, string placeholderName)
        {
            bool isUpdated = false;

            if (getRenderingId(placeholderName) != "")
            {
                string xml = item[Sitecore.FieldIDs.LayoutField];
                string replaceThis = getRenderingId(placeholderName);
                string withThat = "";

                if (xml.Contains(replaceThis))
                {
                    using (new Sitecore.Data.Items.EditContext(item))
                    {
                        string[] splitById = xml.Split(new string[] { replaceThis }, StringSplitOptions.None);

                        int lastIndex = splitById[0].LastIndexOf("<r uid=");
                        string firstPart = splitById[0].Substring(lastIndex + "<r uid=".Length);

                        int lastIndex2 = splitById[1].LastIndexOf("\" />");
                        string lastPart = splitById[1].Substring(0, lastIndex2);

                        item[Sitecore.FieldIDs.LayoutField] = xml.Replace("<r uid=" + firstPart + replaceThis + lastPart + "\" />", withThat);
                        isUpdated = true;
                    }
                }
            }
            return isUpdated;
        }        

        // method to check available styles in button generator
        public static string getStyles(string style)
        {
            string className = "bg-usfGreen";
            switch (style)
            {
                case ("USF_Green"):
                    className = "bg-usfGreen";
                    break;
                case ("Sky_Blue"):
                    className = "bg-usfSky";
                    break;
                case ("Storm"):
                    className = "bg-usfStorm";
                    break;
                case ("Teal_Green"):
                    className = "bg-usfTealGreen";
                    break;
                case ("Slate"):
                    className = "bg-usfSlate";
                    break;
                case ("Seaglass"):
                    className = "bg-usfSeaGlass";
                    break;
                default:
                    className = style;
                    break;
            }
            return className;
        }

        // Convert to wiki code to generate button
        public static string GenerateContents(string rtc)
        {
            string str = rtc;
            string btnText = string.Empty;
            string linkUrl = string.Empty;
            string btnStyle = string.Empty;
            bool spNewTab = false;
            var reg = new System.Text.RegularExpressions.Regex(@"\[\[(.*?)\]\]");
            System.Text.RegularExpressions.Match matches = reg.Match(str);

            if (matches.Success)
            {
                // find syntax
                foreach (var item in reg.Matches(str))
                {
                    var data = item.ToString();
                    data = System.Text.RegularExpressions.Regex.Replace(data, @"\[\[|\]\]", "");
                    var sp = data.ToString().Split('|');
                    string showArrow = "<span class='icon-chevron-right'></span>";
                    string newTab = string.Empty;

                    if (sp.Length >= 3)
                    {
                        btnText = sp[0].ToString();
                        linkUrl = sp[1].ToString();
                        btnStyle = getStyles(sp[2].ToString());
                        spNewTab = linkUrl.ToLower().Contains("::new");
                    }

                    if (sp.Length >= 4)
                    {
                        if (sp[3].ToString().ToLower().Trim() == "false")
                        {
                            showArrow = "";
                        }
                    }

                    if (!String.IsNullOrEmpty(linkUrl) && spNewTab)
                    {
                        var spLinkUrl = linkUrl.Split(new string[] { "::" }, StringSplitOptions.None);
                        linkUrl = spLinkUrl[0];
                        newTab = " target='_blank' ";
                    }

                    string button = "<a href='" + linkUrl + "' " + newTab + " class='btn btn-usf-health btn-icon " + btnStyle + "' role='button'>" + btnText + showArrow + "</a>";
                    str = str.Replace(item.ToString(), button);
                }
            }
            return str;
        }

        // check to see if datasource was created under current page
        public static bool IsDatasourceEditable(Item dsItem, string currentPagePath)
        {
            string datasourcePath = dsItem.Paths.Path;
            if (datasourcePath.StartsWith(currentPagePath)) // dsItem is under current page item
            {
                string folderPath = datasourcePath.Substring(0, datasourcePath.LastIndexOf("/")); 
                Item folder = Sitecore.Context.Database.GetItem(folderPath); // item directly under current page in dsItem's path
                Item dsParentItem = Sitecore.Context.Database.GetItem(dsItem.ParentID); // dsItem's parent item           
                Item resourcesFolderTemplate = Sitecore.Context.Database.GetItem("/sitecore/templates/User Defined2/Component Template/Resources Folder");

                if (dsParentItem.ID == folder.ID) // dsItem's parent is directly under current page
                {
                    if (dsParentItem.TemplateID == resourcesFolderTemplate.ID && dsParentItem.Name.EndsWith("Resources"))
                    {
                        // dsItem's parent is a UserDef2 Resources Folder and ends in "Resources"
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }

        // display message to user that root element is not editable from current page
        public static string DisplayDatasourceUneditableMessage(string datasourcePath)
        {
            string mainRoot = datasourcePath.Substring(0, datasourcePath.LastIndexOf("Resources/")).Replace("/sitecore/content", "");
            mainRoot = mainRoot.Substring(0, mainRoot.LastIndexOf("/"));
            return "<div style=\"text-align:center;padding-bottom:4px\">Rendering only editable from:<br><span style=\"font-size:10px\">" + mainRoot + "</span></div>";
        }

        // Check to see if rendering item is in a specified placeholder
        public static bool CheckRenderingPlaceholder(string renderingPlaceholder, string[] placeholderKeys)
        {
            if (!String.IsNullOrEmpty(renderingPlaceholder) && placeholderKeys.Length > 0)
            {
                foreach (string pk in placeholderKeys)
                {
                    if (renderingPlaceholder.EndsWith("/" + pk)) return true;
                }                
            }
            return false;
        }

        // Find Rendering item in page
        public static int FindRenderingItemOnPage(string renderingItemName)
        {
            var renderingReferences = Sitecore.Context.Item.Visualization.GetRenderings(Sitecore.Context.Device, true);
            var renderingsInPlaceholder = renderingReferences.Where(r => r.RenderingItem.Name.Equals(renderingItemName));
            
            return renderingsInPlaceholder.Count();
        }

        // Check to see if a specific rendering item is in a page item's layout
        public static bool IsRenderingInLayout(string renderingItemName)
        {
            bool inLayout = false;
            Sitecore.Layouts.RenderingReference[] renderings = Sitecore.Context.Item.Visualization.GetRenderings(Sitecore.Context.Device, true);
            int numOfRenderings = renderings.Where(r => r.Placeholder.EndsWith('/' + renderingItemName, StringComparison.OrdinalIgnoreCase)).Count();

            if (numOfRenderings >= 1) { inLayout = true; }            
            return inLayout;
        }

        // Format strings on individual Faculty Directory research profiles
        public static string FormatString(string type, string str)
        {
            string newString = "";
            switch (type)
            {
                case ("Appt"):  // appointment & positions
                    newString = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower())
                            .Replace(" Of ", " of ").Replace(" And ", " and ").Replace(" For ", " for ").Replace(" The ", " the ")
                            .Replace(" At ", " at ").Replace(" In ", " in ").Replace(" In, ", " in, ")
                            .Replace(" Usf ", " USF ").Replace("Usf-", "USF-").Replace(" Coph ", " COPH ").Replace("Mcom", "MCOM")
                            .Replace("Md ", "MD ").Replace("Dpt ", "DPT ").Replace("Pa ", "PA ")
                            .Replace(" Arnp ", " ARNP ").Replace(" Crna ", " CRNA ").Replace(" Dnp ", " DNP ").Replace(" Np ", " NP ")
                            .Replace("Idpodcasts ", "IDPodcasts ")
                            .Replace(" Ii", " II").Replace(" Hr ", " HR ")
                            .Replace("Lvhn ", "LVHN ").Replace("Ume ", "UME ").Replace("Select ", "SELECT ").Replace("Lv ", "LV ")
                            .Replace("Evu ", "EVU ").Replace("Rise/Scp ", "RISE/SCP ").Replace("Cps ", "CPS ")
                            .TrimEnd(new Char[] {',',' '});
                    break;
                case ("Deg"):  // degrees
                    newString = str.Replace(" Of ", " of ").Replace(" And ", " and ").Replace(" For ", " for ").Replace(" The ", " the ").Replace(" At ", " at ").Replace(" In ", " in ");
                    break;
                default:
                    break;
            }
            return newString;
        }

        // Get full list of icons from Font Awesome collection in sitecore
        public static List<string> GetIcons()
        {
            List<string> getIcons = null;            
            Item dataItem = Sitecore.Context.Database.GetItem("/sitecore/content/Data Components/Font Awesome Icons");
            if (dataItem != null)
            {
                string iconLists = dataItem.Fields["Data Container"].Value;
                if (!String.IsNullOrEmpty(iconLists))
                {
                    getIcons = new List<string>(iconLists.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
                }                
            }
            return getIcons;            
        }
    }
}
