using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Data;
using Sitecore.Layouts;
using System.Net;
using Sitecore.Web.UI.WebControls;
using Sitecore.RSSReader.Data;
using System.Text.RegularExpressions;

namespace Sitecore.RSS
{
	public partial class RSS_Reader : System.Web.UI.UserControl
	{
		private static readonly string _rssReaderSublayoutPath = "/sitecore/Layout/Sublayouts/RSS Reader/RSS Reader";
        private static readonly string _rssReaderDataTemplate = "/sitecore/templates/RSS Reader/RSS Feed";
        public bool editMode = false;
        private Item _myDataSourceItem;
        private XNamespace contentNameSpace;
        //private XmlNamespaceManager myNSM;
		
		#region Properties
		private RSSFeed _feedSetting = null;
		/// <summary>
		/// Gets feed setting item.
		/// </summary>
		/// <remarks>Feed setting item could either be item set as Data Source on the RSS Feed Reader sublayout or current context item.</remarks>
		public RSSFeed FeedSetting
		{
			get
			{
				if (_feedSetting == null)
				{
					Database db = Sitecore.Context.Database;
					Item item = Sitecore.Context.Item;

					Item rssReaderSublayout = db.Items[_rssReaderSublayoutPath];

                    Item feedItem = rssReaderSublayout;

                    string dsource = ((Sublayout)this.Parent).DataSource;
                    if (!string.IsNullOrEmpty(dsource))
                    {
                        feedItem = db.GetItem(dsource);
                    }

                    _feedSetting = new RSSFeed(feedItem);
				}
				return _feedSetting;
			}
		}

        protected Sublayout mySublayout
        {
            get
            {
                return (Sublayout)this.Parent;
            }
        }

        protected Item myDataSourceItem
        {
            get
            {
                if (this._myDataSourceItem == null)
                {
                    if (string.IsNullOrEmpty(this.mySublayout.DataSource) || this.mySublayout.DataSource.Equals("SublayoutRSSFeedNewDataSource"))
                    {
                        this._myDataSourceItem = (Item)null;
                    }
                    else
                    {
                        try
                        {
                            this._myDataSourceItem = Sitecore.Context.Database.GetItem(this.mySublayout.DataSource);
                        }
                        catch
                        {
                            this._myDataSourceItem = (Item)null;
                        }
                    }
                }
                return this._myDataSourceItem;
            }
            set
            {
                this._myDataSourceItem = value;
            }
        }

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			lblError.Text = null;

            if (Sitecore.Context.PageMode.IsPageEditorEditing)
			{
				mvRSSReader.SetActiveView(viewPageEditor);
                editMode = true;
			}
			else
			{
				mvRSSReader.SetActiveView(viewNormal);
			}
		}

        /// <summary>
		/// Handles the ActiveViewChanged event of the mvRSSReader control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected void mvRSSReader_ActiveViewChanged(object sender, EventArgs e)
		{
			switch (mvRSSReader.ActiveViewIndex)
			{
				case 0:
					//anything else but PageEditor
					RenderNormalView();
					break;
				case 1:
					//PageEditor
					RenderConfigurationView();
					break;
				default:
					break;
			}
		}

        static bool UrlIsReachable(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10000;
            request.Method = "HEAD";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    HttpWebResponse hwp = (HttpWebResponse)response;
                    return hwp.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

        private XDocument CacheXml(string feedUrl, string filePath)
        {
            XDocument feed = null;
            try
            {
                feed = XDocument.Load(feedUrl);
                
            }
            catch { feed = null; }
            try
            {
                System.Threading.Tasks.Task cacheFeedTask = new System.Threading.Tasks.Task(() => feed.Save(filePath));
                cacheFeedTask.Start();//cache file
            }
            catch (Exception ex)
            {
                using (System.Diagnostics.EventLog elog = new System.Diagnostics.EventLog("Application"))
                {
                    elog.Source = "Application";
                    elog.WriteEntry("Sitecore RSS Error: " + ex.Message, System.Diagnostics.EventLogEntryType.Error, 101, 1);
                }
            }
            return feed;
        }

        private XDocument GetFeed(string filePath, string feedUrl)
        {
            if (System.IO.File.Exists(filePath))//cache file found
            {
                int cacheHrLimit = 2;
                if (!Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["RSSCacheHrLimit"], out cacheHrLimit)) cacheHrLimit = 2;
                if ((DateTime.Now - System.IO.File.GetLastWriteTime(filePath)).TotalHours > cacheHrLimit)//cache file is old
                {
                    //attempt to get new feed to cache
                    if (UrlIsReachable(feedUrl))
                    {
                        return CacheXml(feedUrl, filePath);//cache file and return it 
                    }
                    else { return XDocument.Load(filePath); }//can't reach url, serve up cache
                }
                else //cache file not old, return it
                {
                    return XDocument.Load(filePath);
                }
            }
            else if(UrlIsReachable(feedUrl))//new rss feed
            {
                return CacheXml(feedUrl, filePath);//cache file and return it 
            }
            return null;
        }

		/// <summary>
		/// Binds the feeds.
		/// </summary>
		/// <param name="feedUrl">The feed URL.</param>
		private void BindFeeds(string feedUrl)
        {
			try
			{
                string feedFileName = ParseFeedUrlToFileName(feedUrl) + ".xml";
                if (!string.IsNullOrEmpty(feedFileName))
                {
                    string feedFilePath = System.Configuration.ConfigurationManager.AppSettings["RSSCacheFilePath"];
                    feedFilePath = System.IO.Path.Combine(feedFilePath, feedFileName);
                    XDocument feedSource = GetFeed(feedFilePath, feedUrl);
                    if (feedSource != null)
                    {
                        try { contentNameSpace = feedSource.Root.GetNamespaceOfPrefix("content"); }
                        catch { }

                        var feeds = from myFeed in feedSource.Descendants("channel")
                                    select new
                                    {
                                        feedTitle = myFeed.Element("title").Value,
                                        feedDescription = myFeed.Element("description").Value,
                                        feedLink = myFeed.Element("link").Value,
                                        feedItems = myFeed.Descendants("item")
                                    };

                        if (_feedSetting.NumberOfItems > 0)
                        {
                            feeds = from myFeed in feedSource.Descendants("channel")
                                    select new
                                    {
                                        feedTitle = myFeed.Element("title").Value,
                                        feedDescription = myFeed.Element("description").Value,
                                        feedLink = myFeed.Element("link").Value,
                                        feedItems = myFeed.Descendants("item").Take<XElement>(_feedSetting.NumberOfItems)
                                    };
                        }

                        lvFeedReader.DataSource = feeds;
                        lvFeedReader.DataBind();
                    }
                    else
                    {
                        string errorMessage = "Error: RSS feed not accessible";
                        Sitecore.Diagnostics.Log.Error(errorMessage, this);
                        lblError.Text += errorMessage + "<br/>";
                    }
                }
			}
			catch (Exception ex)
			{
				string errorMessage = "Error with RSS feed binding: " + ex.Message;
				Sitecore.Diagnostics.Log.Error(errorMessage, this);
				lblError.Text += errorMessage;
			}
		}

        private string ParseFeedUrlToFileName(string feedUrl)
        {
            string fileName = "";
            fileName = feedUrl.Replace("http", "").Replace("www", "");
            if (feedUrl.Contains("hscweb3"))
            {
                fileName = feedUrl.Substring(feedUrl.IndexOf("edu") + 3).Replace("feed", "");//eliminate un-needed domain name and grab content after .edu
            }
            char[] arr = fileName.Where(c => char.IsLetterOrDigit(c)).ToArray();//get only alphanumeric characters
            fileName = new string(arr);
            if (fileName.Length > 100) fileName = fileName.Substring(0, 100);
            if (!string.IsNullOrEmpty(fileName) 
                && fileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) < 0)
            {
                return fileName;
            }
            else return null;
        }

		/// <summary>
		/// Renders the configuration view.
		/// </summary>
        private void RenderConfigurationView()
        {
            if (this.myDataSourceItem == null)
            {
                this.myDataSourceItem = HealthIS.Apps.Util.createDatasourceItem("RSS Reader/RSS Feed", "SublayoutRSSFeedNewDataSource", "_RSS");
                if (this.myDataSourceItem == null)
                    ;
            }
            else if (this.FeedSetting == null)
            {
                this.lblError.Text = "//TODO: render view that allows selection of feed settings item (data source)";
            }
            else
            {
                this.configItemName.Text = this.FeedSetting.FeedItemTitle;
                this.fldFeedUrl.Item = this.FeedSetting.InnerItem;
                this.fldFeedCSSClass.Item = this.FeedSetting.InnerItem;
                this.fldFeedItemCSSClass.Item = this.FeedSetting.InnerItem;
                this.fldItemDescriptionCharLimit.Item = this.FeedSetting.InnerItem;
                this.fldNumberOfItems.Item = this.FeedSetting.InnerItem;
                this.fldNumberOfImages.Item = this.FeedSetting.InnerItem;
                this.fldFeedTitle.Item = this.FeedSetting.InnerItem;
                this.cbShowFeedTitle.Checked = this.FeedSetting.DisplayFeedTitle;
                this.cbShowTitleFirst.Checked = this.FeedSetting.DisplayTitleFirst;
                this.cbShowFeedDesc.Checked = this.FeedSetting.DisplayFeedDesc;
                Sublayout sublayout = (Sublayout)this.Parent;
                string str1 = this.FeedSetting.InnerItem.ID.ToShortID().ToString();
                string str2 = "";
                string str3 = "";
                string str4 = "";
                try
                {
                    str2 = this.FeedSetting.InnerItem.Fields["display feed title"].ID.ToShortID().ToString();
                    str3 = this.FeedSetting.InnerItem.Fields["display title first"].ID.ToShortID().ToString();
                    str4 = this.FeedSetting.InnerItem.Fields["display feed description"].ID.ToShortID().ToString();
                }
                catch
                {
                }
                string str5 = this.FeedSetting.InnerItem.Language.ToString();
                string str6 = this.FeedSetting.InnerItem.Version.ToString();
                string str7 = this.FeedSetting.InnerItem[FieldIDs.Revision].Replace("-", string.Empty);
                this.cbShowFeedTitle.Attributes.Add("onclick", string.Format("var val = ''; if(this.checked) val='1';  var itemURI = new Sitecore.ItemUri('{0}','{1}','{2}','{3}');Sitecore.WebEdit.setFieldValue(itemURI,'{4}',val);", (object)str1, (object)str5, (object)str6, (object)str7, (object)str2, (object)sublayout.ClientID.ToString()));
                this.cbShowTitleFirst.Attributes.Add("onclick", string.Format("var val = ''; if(this.checked) val='1';  var itemURI = new Sitecore.ItemUri('{0}','{1}','{2}','{3}');Sitecore.WebEdit.setFieldValue(itemURI,'{4}',val);", (object)str1, (object)str5, (object)str6, (object)str7, (object)str3, (object)sublayout.ClientID.ToString()));
                this.cbShowFeedDesc.Attributes.Add("onclick", string.Format("var val = ''; if(this.checked) val='1';  var itemURI = new Sitecore.ItemUri('{0}','{1}','{2}','{3}');Sitecore.WebEdit.setFieldValue(itemURI,'{4}',val);", (object)str1, (object)str5, (object)str6, (object)str7, (object)str4, (object)sublayout.ClientID.ToString()));
            }
        }

		/// <summary>
		/// Renders the normal view.
		/// </summary>
		private void RenderNormalView()
		{
			viewNormal.DataBind();
            if (this.FeedSetting.InnerItem.TemplateID.ToString() == Sitecore.Context.Database.GetItem(RSS_Reader._rssReaderDataTemplate).ID.ToString())
            {
                this.BindFeeds(this.FeedSetting.FeedUrl.Url);
            }
            else
            {
                string message = "Source item is not a feed configuration item";
                Sitecore.Diagnostics.Log.Error(message, (object)this);
                this.lblError.Text = message;
            }
		}

		#region Helper Methods

		/// <summary>
		/// Returns a string value of __Renderings field of item or its data 
		/// template's standard values or its base template's standard values.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		

		public string xEval(object x, String xPath)
		{
            try
            {
                XmlDocument xDocument = new XmlDocument();
                xDocument.LoadXml(x.ToString());

                x = xDocument.SelectSingleNode(xPath).InnerText;
            }
            catch
            {
                try
                {
                    XmlDocument xDocument = new XmlDocument();
                    xDocument.LoadXml(x.ToString());
                    
                    XmlNamespaceManager nm = new XmlNamespaceManager(xDocument.NameTable);
                    nm.AddNamespace("content",contentNameSpace.NamespaceName);
                    
                    XmlNode n = xDocument.SelectSingleNode(xPath, nm);

                    x = n.InnerText;
                }
                catch (Exception ex)
                {
                    x = "<span class=\"feed-error-msg\">Error parsing feed item</span><div style='display:none;'>" + ex.Message +"</div>";
                    Sitecore.Diagnostics.Error.LogError(ex.Message);
                }
            }
			return x.ToString();
		}

        public Object SafeEval(object container, string expression) //SafeEval(Container.DataItem, "XML")
        {
            try
            {
                return DataBinder.Eval(container, expression);
            }
            catch { return null; }
        }

        public ArrayList getImages(object x, String xPath)
        {
            ArrayList arrayList = new ArrayList();
            foreach (Match match in Regex.Matches(this.xEval(x, xPath), "src\\s*=\\s*\"(.+?)\""))
            {
                int startIndex = match.Value.IndexOf('"') + 1;
                int length = match.Value.LastIndexOf('"') - startIndex;
                if (length < 1)
                    length = 0;
                arrayList.Add((object)match.Value.Substring(startIndex, length));
            }
            if (arrayList.Count < 1)
                arrayList.Add((object)"http://hsccf.hsc.usf.edu/images/rt/health_is.png");
            return arrayList;
        }

        public string outputImages(object x, String xPath)
        {
            string str1 = "";
            MatchCollection matchCollection = Regex.Matches(this.xEval(x, xPath), "src\\s*=\\s*\"(.+?)\"");
            int num = 0;
            foreach (Match match in matchCollection)
            {
                if (this._feedSetting.NumberOfImages > num)
                {
                    int startIndex = match.Value.IndexOf('"') + 1;
                    int length = match.Value.LastIndexOf('"') - startIndex;
                    if (length < 1)
                        length = 0;
                    string str2 = match.Value.Substring(startIndex, length);
                    str1 = str1 + "<img src=\"" + str2 + "\" class=\"" + this._feedSetting.FeedItemImageCSSClass + "\" />";
                    ++num;
                }
                else
                    break;
            }
            return str1;
        }

        public string getFeedDescription(object x, String xPath)
        {
            string str1 = new Regex("\\<[^\\>]*\\>").Replace(this.xEval(x, xPath), string.Empty);
            string str2 = str1;
            if (this._feedSetting.DescriptionCharacterLimit > 0 && str1.Length > this._feedSetting.DescriptionCharacterLimit)
                str2 = str1.Substring(0, this._feedSetting.DescriptionCharacterLimit) + "[...]";
            return str2;
        }

		#endregion
	}
}