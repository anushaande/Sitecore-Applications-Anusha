using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Web;
using System.Xml.Linq;
using System.Linq;

namespace HealthIS.Apps.MVC
{
    public class ComponentModel : IRenderingModel //Update class name
    {
        public string _myDataSourceTemplatePath = "User Defined/Sublayouts/sublayoutContentArea";
        public string _myDataSourceTemporaryValue = "SublayoutContentAreaNewDataSource";

        public string Error { get; set; }
        public Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public bool dsIsSet { get; set; }
        public Item DataSource { get; set; }
        public IList<RSSItem> Feed { get; set; }

        public void SetDataSource(Rendering rendering)
        {
            dsIsSet = false; 
            try
            {
                //Item dataSource = HealthIS.Apps.Util.createDatasourceItemMVC(_myDataSourceTemplatePath, "_CO", rendering);
                //DataSource = dataSource;
                //DataSource = HealthIS.Apps.Util.UpdateDatasource(rendering.Item.ID.ToString(), _myDataSourceTemplatePath, "_CO", rendering.Id.ToString(), Sitecore.Context.Database.Name, Sitecore.Context.Device.ID.ToString());
                
                //Item dataSource = HealthIS.Apps.Util.createDatasourceItem(_myDataSourceTemplatePath, _myDataSourceTemporaryValue, "_CO");
                //Rendering.DataSource = dataSource.ID.ToString();
                dsIsSet = true;
            }
            catch (Exception ex)
            {
                Error += ex.Message + "<br/>" + ex.StackTrace + "<br/>";
            }
        }

        public void Initialize(Rendering rendering)
        {
            try
            {
                Rendering = rendering;
                PageItem = PageContext.Current.Item;
                Item = rendering.Item;
                dsIsSet = !String.IsNullOrEmpty(rendering.DataSource);
                SetFeed();
                //SetDataSource(rendering);
                //Rendering.DataSource = "{87A309E1-CBC9-4432-9147-01BAC8CA1B5F}";           
            }
            catch (Exception ex)
            {
                Error += ex.Message + "<br/>" + ex.StackTrace + "<br/>";
            }
        }

        private void SetFeed()
        {
            string feedURL = "http://hscweb3.hsc.usf.edu/blog/category/new_release/feed/";

            Feed = ParseRss(feedURL);            
            
        }

        public virtual IList<RSSItem> ParseRss(string url)
        {
            try
            {
                XDocument doc = XDocument.Load(url);
                // RSS/Channel/item
                var entries = from item in doc.Root.Descendants().First(i => i.Name.LocalName == "channel").Elements().Where(i => i.Name.LocalName == "item")
                              select new RSSItem
                              {
                                  Content = item.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Link = item.Elements().First(i => i.Name.LocalName == "link").Value,
                                  PublishDate = ParseDate(item.Elements().First(i => i.Name.LocalName == "pubDate").Value),
                                  Title = item.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList();
            }
            catch
            {
                return new List<RSSItem>();
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

    public class RSSItem
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }

        public RSSItem()
        {
            Link = "";
            Title = "";
            Content = "";
            PublishDate = DateTime.Today;
        }

    }
}