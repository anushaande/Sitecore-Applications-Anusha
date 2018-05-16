using System.ServiceModel.Syndication;
using Sitecore.Diagnostics;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.Syndication;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using Sitecore.Shell.Feeds;
using Sitecore.Sites;

//// This config file must be added into /App_Include/HealthIS.Apps.Blog.Settings.config
//<configuration xmlns:patch="http://www.sitecore.net/xmlconfig/">
//    <sitecore>
//        <settings>
//            <setting name="HealthIS.Apps.RSS.Fields.FieldNames" value="thumbnailImage,content,categories,tags,pubDate,author" />
//            <setting name="HealthIS.Apps.RSS.Pipelines.RenderField" value="renderField" />
//        </settings>
//    </sitecore>
//</configuration>
 
namespace HealthIS.Apps.RSS
{
    public class RSSFeedAdditionalFields : PublicFeed
    {
        private static List<string> FieldNames { get; set; }
        private static string RenderFieldPipelineName { get; set; }
        private int NumberOfLimit = 1;

        static RSSFeedAdditionalFields()
        {
            FieldNames = Settings.GetSetting("HealthIS.Apps.RSS.Fields.FieldNames").Split(',').ToList();
            RenderFieldPipelineName = Settings.GetSetting("HealthIS.Apps.RSS.Pipelines.RenderField");
        }

        protected override SyndicationItem RenderItem(Item item)
        {
            SyndicationItem syndicationItem = null;
            Item rssFeedItem = Sitecore.Context.Item;
            int limit = 0;
            bool isInt = int.TryParse(rssFeedItem.Fields["Number of Limits"].Value.Trim(), out limit);
            if (rssFeedItem.Fields["Number of Limits"] != null) {
                if (!String.IsNullOrEmpty(rssFeedItem.Fields["Number of Limits"].Value.Trim()) && isInt)
                {
                    limit = Int32.Parse(rssFeedItem.Fields["Number of Limits"].Value.Trim());
                }
            }

            if (item.Template.FullName.Contains("Detail View Template") && (limit == 0 || this.NumberOfLimit <= limit))
            {
                syndicationItem = base.RenderItem(item);

                AddHtmlToContent(syndicationItem, GetFieldHtmls(item));
                if (limit != 0)
                {
                    this.NumberOfLimit++;
                }
            }

            return syndicationItem;
        }

        protected virtual Dictionary<string, string> GetFieldHtmls(Item item)
        {
            if (FieldNames.Count <= 0)
            {
                return null;
            }
            return GetFieldHtmls(item, FieldNames);
        }

        private static Dictionary<string, string> GetFieldHtmls(Item item, List<string> FieldNames)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNullOrEmpty(RenderFieldPipelineName, "renderField");
            Dictionary<string, string> argsResult = new Dictionary<string, string>();


            foreach (var fieldName in FieldNames)
            {
                if (item == null)
                {
                    return argsResult;
                }
                RenderFieldArgs args = new RenderFieldArgs { Item = item, FieldName = fieldName };
                CorePipeline.Run(RenderFieldPipelineName, args);
                
                if (!args.Result.IsEmpty)
                {
                    string contents = args.Result.ToString();
                    string multilistItems = "";

                    // Image Field Type - Get only image path
                    if (args.FieldTypeKey == "image")
                    {
                        Sitecore.Data.Fields.ImageField iFld = args.Item.Fields[fieldName];
                        Sitecore.Resources.Media.MediaUrlOptions opt = new Sitecore.Resources.Media.MediaUrlOptions();
                        // Absolute Path works as well. So either use AbsolutePath or AlwaysIncludeServerUrl
                        //opt.AlwaysIncludeServerUrl = true;
                        opt.AbsolutePath = true;
                        string mediaUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(iFld.MediaItem, opt);
                        contents = mediaUrl;
                    }

                    // Multilist Field Type - Convert Item ID to Item Name
                    if (args.FieldTypeKey == "multilist" || args.FieldTypeKey == "multilist with search")
                    {
                        List<string> itemId = args.Result.ToString().Split('|').ToList();
                        int count = 1;
                        foreach (string i in itemId)
                        {
                            Item theItem = item.Database.GetItem(Sitecore.Data.ID.Parse(i));
                            if (theItem != null)
                            {
                                multilistItems += theItem.Name;
                                if (count != itemId.Count)
                                {
                                    multilistItems += "|";
                                }
                            }
                            count++;
                        }
                        contents = multilistItems;
                    }
                    argsResult.Add(fieldName, contents);
                }
            }

            return argsResult;
        }

        protected virtual void AddHtmlToContent(SyndicationItem syndicationItem, Dictionary<string, string> getHtml)
        {
            if (!(syndicationItem.Content is TextSyndicationContent))
            {
                return;
            }
            foreach (KeyValuePair<string, string> pair in getHtml)
            {
                string value = pair.Value;
                if (pair.Key == "categories" || pair.Key == "tags")
                {
                    value = pair.Value;
                }
                syndicationItem.ElementExtensions.Add(new XElement(pair.Key, new XCData(value)).CreateReader());
            }

            TextSyndicationContent content = syndicationItem.Content as TextSyndicationContent;
            string contentText = content.Text;
            if (!String.IsNullOrEmpty(contentText))
            {
                contentText = content.Text;
            }
            
            syndicationItem.Content = new TextSyndicationContent(contentText, TextSyndicationContentKind.Html);
        }
    }
}