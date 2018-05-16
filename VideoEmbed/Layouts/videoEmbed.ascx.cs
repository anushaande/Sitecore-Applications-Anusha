using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;


namespace Sitecore.videoEmbed {
  
	/// <summary>
	/// Summary description for VideoembedSublayout
	/// </summary>
    /// 

    public class vidEmbed
    {
        private Item _item = null;
        public List<Item> itms = null;

        public Item InnerItem
        {
            get { return _item; }
        }

        public vidEmbed(Item inn)
        {
            _item = inn;
            itms = new List<Item>();

            Database db = Sitecore.Context.Database;
            Item vidEmbedLink = db.Items["/sitecore/templates/User Defined/vidEmbed"];

            foreach (Item chi in _item.GetChildren())
            {
                if (chi.TemplateID.ToString() == vidEmbedLink.ID.ToString())
                {
                    itms.Add(chi);
                }
            }
        }
    }

    public partial class VideoEmbedSublayout : System.Web.UI.UserControl 
	{
      private vidEmbed myItem = null;
      public vidEmbed MyItem
        {
            get
            {
                if (myItem == null)
                {
                    Database db = Sitecore.Context.Database;
                    Item item = Sitecore.Context.Item;

                    Item vidEmbedSublayout = db.Items["/sitecore/Layout/Sublayouts/videoEmbed"];

                    Item feedItem = vidEmbedSublayout;
                    Sublayout par = ((Sublayout)this.Parent);
                    string dsource = par.DataSource;
                    if (!string.IsNullOrEmpty(dsource))
                    {
                        feedItem = db.GetItem(dsource);
                    }

                    if (feedItem != vidEmbedSublayout)
                    {
                        myItem = new vidEmbed(feedItem);
                    }
                }
                return myItem;
            }
        }


    private void Page_Load(object sender, EventArgs e) 
    {
        if (Sitecore.Context.PageMode.IsPageEditorEditing)
			{
				mvVideoEmbed.SetActiveView(viewPageEditor);
			}
			else
			{
				mvVideoEmbed.SetActiveView(viewNormal);
			}
    }
      protected void mvVideoEmbed_ActiveViewChanged(object sender, EventArgs e)
		{
			switch (mvVideoEmbed.ActiveViewIndex)
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
      private void RenderConfigurationView()
  {
       if(MyItem == null){
           lblError.Text += "Data Source is not defined. Add a data Source to videoEmbed Sublayout by selecting it under Advanced > Details > Controls.";
       }else{
           configItemName.Text = MyItem.InnerItem.Name;
           vidTitle.Item = MyItem.InnerItem;
           vidLightboxTitle.Item = MyItem.InnerItem;
           vidcssClass.Item = MyItem.InnerItem;
           vidURL.Item = MyItem.InnerItem;
       }
  }
      private void RenderNormalView()
   {
       if (MyItem != null)
       {
           VideoEmbedTitle.Text = MyItem.InnerItem.Fields["Title"].Value;
           VideoUrl.Attributes["href"] = ParseVidUrl(MyItem.InnerItem.Fields["URL"].Value);
           VideoUrl.Attributes["Class"] = MyItem.InnerItem.Fields["css"].Value;
           videoImg.Attributes["src"] = ParseImgUrl(MyItem.InnerItem.Fields["URL"].Value);
       }
       else
       {
           lblError.Text += "Error Rendering Navigation";
       }
   }

      
  public string ParseVidUrl(string url)
  {
      string vID = null;
      string vidURL;
      Regex regex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
      Match match = regex.Match(url);
      
          vID = match.Groups[1].Value;
         return vidURL = "http://www.youtube.com/embed/" + vID + "?rel=0&amp;wmode=transparent&amp;autoplay=1";
   }

  public string ParseImgUrl(string url)
  {
      string vID = null;
      string imgURl;
      Regex regex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
      Match match = regex.Match(url);
      vID = match.Groups[1].Value;
      return imgURl = "http://img.youtube.com/vi/" + vID + "/0.jpg";
  }
 }
}