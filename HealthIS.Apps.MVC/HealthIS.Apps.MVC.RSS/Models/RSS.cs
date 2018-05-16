using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.ServiceModel.Syndication;
using Sitecore.Syndication;
using System.Net;
using System.Text;

namespace HealthIS.Apps.MVC
{
    public class RSS : Sitecore.Mvc.Presentation.RenderingModel
    {
        public string feedURL, feedHeading, feedTheme;
        public string feedHeadingFontSize = "h3";
        public int descriptionCharacterLimit = 500, numberOfItems = 10, numberOfImages = 1;
        public int maxNumberOfItems = 10, maxCharLimit = 500;
        public bool displayOnMobile = true, displayShowMoreLink = false, isPanoptoFeed = false;
        public List<RSSFeedItem> feedItems = new List<RSSFeedItem>();
        public string feedTitleLinkURL = "", feedDescription = "";
        public ImageField defaultThumbnail;
        public MediaItem defaultMediaItem;
        public string defaultThumbnailSrc;
        public List<string> errorLog = new List<string>();
        public bool feedSet = false;
        public bool dsSet = false;


        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            try
            {   
                //Set model specific objects
                Rendering = rendering;
                dsSet = !String.IsNullOrEmpty(rendering.DataSource);
                if (dsSet)
                {
                    //Get all the fields from the datasource item
                    feedURL = Helpers.getStrField(Item, "Feed URL");
                    feedHeadingFontSize = Helpers.getStrField(Item, "Feed Heading Font Size");
                    feedHeading = Helpers.getStrField(Item, "Feed Heading");
                    feedTheme = Helpers.getStrField(Item, "Theme");
                    descriptionCharacterLimit = Helpers.getIntField(Item, "Feed Item Description Character Limit");
                    if (descriptionCharacterLimit < 0) { descriptionCharacterLimit = 100; }
                    numberOfItems = Helpers.getIntField(Item, "Number of Items");                    
                    displayOnMobile = Helpers.getBoolField(Item, "Show on Mobile");                   
                    displayShowMoreLink = Helpers.getBoolField(Item, "Display Show More Link");
                    isPanoptoFeed = Helpers.getBoolField(Item, "Is Panopto Feed");
                    defaultThumbnail = Helpers.getImgField(Item, "Default Thumbnail");
                    defaultMediaItem = new Sitecore.Data.Items.MediaItem(defaultThumbnail.MediaItem);
                    defaultThumbnailSrc = Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(defaultMediaItem));

                    if (String.IsNullOrEmpty(feedHeadingFontSize))
                    {
                        using (new Sitecore.SecurityModel.SecurityDisabler())
                        {
                            Item.Editing.BeginEdit();
                            Item.Fields["Feed Title Font Size"].Value = "h3";
                            Item.Editing.EndEdit();
                        }
                    }
                    
                    //Get the RSS Feed content if the feedURL is defined
                    if (!String.IsNullOrEmpty(feedURL)) { setRSSFeed(); }
                }
            }
            catch (Exception ex)
            {
                HealthIS.Apps.Util.LogError(ex.Message, ex, this);
                errorLog.Add(ex.Message);
            }
        }

        public string getNodeNames(List<XElement> elements)
        {
            List<string> names = new List<string>();
            foreach (XElement x in elements)
            {
                names.Add(x.Name.LocalName);
            }
            return String.Join(", ", names);
        }        

        public void setRSSFeed()
        {            
            try
            {
                if (!String.IsNullOrEmpty(this.feedURL))
                {
                    string feedUrl = this.feedURL.StartsWith("/sitecore/content/") ? GenerateSitecoreRssFeedUrl(this.feedURL) : this.feedURL;
                    XDocument doc = XDocument.Load(feedUrl);
                    
                    if (!isPanoptoFeed)
                    {
                        string feedContent = !feedUrl.Contains("health.usf.edu/") ? "encoded" : "content"; // if sitecore blog, use "content" instead of "encoded"

                        // RSS/Channel/item
                        var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                                    select new RSSFeedItem(                                          
                                        GetNodeValue(item, "link"),
                                        GetNodeValue(item, "title"),
                                        GetNodeValue(item, "pubDate"),
                                        GetNodeValue(item, "description"),
                                        item.Elements().FirstOrDefault(i => i.Name.LocalName == feedContent)
                                    );
                        feedItems = entries.ToList<RSSFeedItem>();
                        feedSet = true;
                        feedTitleLinkURL = doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().First(i => i.Name.LocalName == "link").Value;
                        feedDescription = doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().First(i => i.Name.LocalName == "description").Value;
                    }
                    else if (isPanoptoFeed)
                    {
                        // RSS/Channel/item
                        var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                                        select new RSSFeedItem(
                                            GetNodeValue(item, "guid"),
                                            GetNodeValue(item, "title"),
                                            GetNodeValue(item, "pubDate"),
                                            "",
                                            item.Elements().FirstOrDefault(i => i.Name.LocalName == "encoded")
                                        );
                        feedItems = entries.OrderByDescending(x => x.PublishDate).ToList<RSSFeedItem>();
                        feedSet = true;
                        feedTitleLinkURL = doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().First(i => i.Name.LocalName == "link").Value;
                    }                    
                }
                else {
                    feedSet = false;
                    this.errorLog.Add("Feed URL not defined.");
                }
            }
            catch (Exception ex)
            {
                feedSet = false;
                HealthIS.Apps.Util.LogError(ex.Message, ex, this);
                errorLog.Add("setRSSFeed ERROR: " + ex.Message);
                errorLog.Add("Stack Trace: " + ex.StackTrace);
            }
        }

        public string GetCheckedAttr(bool fieldVal)
        {
            if (fieldVal) { return "checked='checked'"; }
            else { return ""; }
        }

        public string TransformPanoptoFeedURL()
        {
            if ((!String.IsNullOrEmpty(this.feedURL)))
            {
                string url = this.feedURL;
                url = url.Remove(url.IndexOf("&")).Substring(url.IndexOf("://")+3);
                if (url.Trim().StartsWith("hsccapture1.health.usf.edu/Panopto/Podcast/Podcast.ashx?courseid="))
                {
                    url = "https://" + url.Replace("Podcast/Podcast.ashx?courseid=", "Pages/Sessions/List.aspx#folderID=\"") + "\"";
                }return url;
            }return this.feedURL;
        }

        public string GenerateSitecoreRssFeedUrl(string url)
        {
            string feed = string.Empty;
            if (!String.IsNullOrEmpty(url))
            {
                Item rssFeed = Sitecore.Context.Database.GetItem(url);                    
                if (rssFeed != null && rssFeed.TemplateName.Equals("RSS Feed"))
                {
                    feed = "https://cmstest.health.usf.edu/" + FeedManager.GetFeedUrl(rssFeed, false);
                    //feed = "https://health.usf.edu/" + FeedManager.GetFeedUrl(rssFeed, false);
                }                             
            }
            return feed;
        }

        public string GetNodeValue(XElement parent, string name)
        {
            if (parent != null && !String.IsNullOrEmpty(name))
            {
                XElement node = parent.Element(name);
                if (node != null) { return node.Value; } 
            }
            return "";
        }

        public bool DisplayCSSButton()
        {
            // developer-only option, do not display button for authors, contributors or moderators
            if (!Sitecore.Context.User.IsInRole("hscnet\\SC_Role_Author") && !Sitecore.Context.User.IsInRole("hscnet\\SC_Role_Contributor") && !Sitecore.Context.User.IsInRole("hscnet\\SC_Role_Moderator"))               
            {
                return true;
            }
            return false;            
        }
    }

    public class RSSFeedItem
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string strPubDate { get; set; }
        public string EncodedContent { get; set; }
        public string Description { get; set; }
        public List<string> Images { get; set; }

        public RSSFeedItem(string link, string title, string pubDate, string desc, XElement cont)
        {
            Link = link;
            Title = title;
            PublishDate = ParseDate(pubDate);            
            strPubDate = pubDate;
            Description = "";
            Images = new List<string>();
            EncodedContent = cont == null ? "" : cont.Value;
            if (String.IsNullOrEmpty(EncodedContent)) { parseSetDesc(desc); } 
            else { parseSetDesc(EncodedContent); }
        }

        private string removeTrailingSubstring(string str, string toRemove)
        {
            int lastInd = str.LastIndexOf(toRemove);
            if (lastInd == str.Length - toRemove.Length)
            {
                str = str.Substring(0, str.Length - lastInd);
                if (str.LastIndexOf(toRemove) == str.Length - toRemove.Length) { str = removeTrailingSubstring(str, toRemove); }
            }
            return str;
        }

        public string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        //pulls out img tags from string and adds them to the images list
        private void parseSetDesc(string desc)
        {
            try
            {
                List<string> imgTags = new List<string>();
                string imgPattern = @"<img.+?src=[\""'](.+?)[\""'].*?>";
                foreach (Match m in Regex.Matches(desc, imgPattern))
                {
                    if (m.Groups.Count >= 1)
                    {
                        this.Images.Add(m.Groups[1].Value);//add image to collection
                    }
                }
                desc = desc.Replace("[&#8230;]", "");
                desc = StripTagsRegex(desc);
                this.Description = desc;
                
            }
            catch (Exception ex)
            {
                HealthIS.Apps.Util.LogError(ex.Message, ex, this);
            }
        }

        private DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;
            else
                return DateTime.MinValue;
        }

    }
}