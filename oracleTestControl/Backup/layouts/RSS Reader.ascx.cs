using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
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

namespace Sitecore.RSS
{
	public partial class RSS_Reader : System.Web.UI.UserControl
	{
		private static readonly string _renderingsFieldName = "__renderings";
		private static readonly string _rssReaderSublayoutPath = "/sitecore/layout/Sublayouts/RSS Reader";
		private static readonly string _rssReaderSublayoutID = "{A8778CD7-E6CD-446F-A200-AE1A8F121D13}";

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

					//try to find RSS Reader sublayout settings on the current item
					string renderingsField = GetPresentationComponents(item);

					LayoutDefinition layout = LayoutDefinition.Parse(renderingsField);
					DeviceDefinition device = layout.GetDevice(Sitecore.Context.Device.ID.ToString());

					Item rssReaderSublayout = db.Items[_rssReaderSublayoutPath];
					RenderingDefinition rssReader = device.GetRendering(rssReaderSublayout.ID.ToString());
					rssReader = device.GetRendering(_rssReaderSublayoutID);

					Sitecore.Diagnostics.Log.Info("RSS Reader sublayout DataSource: " + rssReader.Datasource, this);

					Item dataSource = db.GetItem(rssReader.Datasource);

					Item feedItem = !String.IsNullOrEmpty(rssReader.Datasource) ? db.GetItem(rssReader.Datasource) : item;
					_feedSetting = new RSSFeed(feedItem);
				}
				return _feedSetting;
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			lblError.Text = null;

			if (Sitecore.Context.PageMode.IsPageEditorEditing)
			{
				mvRSSReader.SetActiveView(viewPageEditor);
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

		/// <summary>
		/// Binds the feeds.
		/// </summary>
		/// <param name="feedUrl">The feed URL.</param>
		private void BindFeeds(string feedUrl)
		{

			try
			{
				XNamespace dcNameSpace = XNamespace.Get("http://purl.org/dc/elements/1.1/");
				XDocument feedSource = XDocument.Load(feedUrl);

				var feeds = from myFeed in feedSource.Descendants("channel")
							select new
							{
								feedTitle = myFeed.Element("title").Value,
								feedDescription = myFeed.Element("description").Value,
								feedLink = myFeed.Element("link").Value,
								feedItems = myFeed.Descendants("item")
							};

				lvFeedReader.DataSource = feeds;
				lvFeedReader.DataBind();

			}
			catch (WebException ex)
			{
				string errorMessage = "External RSS feed possibly not accessible: " + ex.Message;
				Sitecore.Diagnostics.Log.Error(errorMessage, this);
				lblError.Text = errorMessage;
			}
		}

		/// <summary>
		/// Renders the configuration view.
		/// </summary>
		private void RenderConfigurationView()
		{
			if (FeedSetting == null)
			{
				//TODO: render view that allows selection of feed settings item (data source)
				lblError.Text = "//TODO: render view that allows selection of feed settings item (data source)";
			}
			else
			{
				//render a view that allows setting current feed settings
				fldFeedUrl.Item = FeedSetting.InnerItem;
				fldFeedCSSClass.Item = FeedSetting.InnerItem;
				fldFeedItemCSSClass.Item = FeedSetting.InnerItem;
			}
		}

		/// <summary>
		/// Renders the normal view.
		/// </summary>
		private void RenderNormalView()
		{
			viewNormal.DataBind();
			if (FeedSetting.InnerItem.TemplateID.ToString() == "{12E424AE-1790-4409-8CDD-B667F08C1DB2}")
			{
				LinkField feedUrl = FeedSetting.FeedUrl;
				BindFeeds(feedUrl.Url);
			}
			else
			{
				string errorMessage = "Source item is not a feed configuration item";
				Sitecore.Diagnostics.Log.Error(errorMessage, this);
				lblError.Text = errorMessage;
			}
		}

		#region Helper Methods

		/// <summary>
		/// Returns a string value of __Renderings field of item or its data 
		/// template's standard values or its base template's standard values.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		private static string GetPresentationComponents(Item item)
		{
			string renderingsField = item[_renderingsFieldName];
			if (renderingsField == string.Empty)
			{
				//check template's standar values
				renderingsField = item.Template.StandardValues[_renderingsFieldName];
			}
			return renderingsField;
		}

		public string xEval(object x, String xPath)
		{
			XmlDocument xDocument = new XmlDocument();
			xDocument.LoadXml(x.ToString());

			x = xDocument.SelectSingleNode(xPath).InnerText;
			return x.ToString();
		}
		#endregion
	}
}