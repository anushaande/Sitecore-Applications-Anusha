using System;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using Sitecore.Layouts;


namespace Sitecore.VerticalNav {
  
	/// <summary>
	/// Summary description for VerticalnavSublayout
	/// </summary>
    /// 

    public class vertNav
    {
        private Item _item = null;
        public List<Item> itms = null;

        public Item InnerItem
        {
            get { return _item; }
        }

        public vertNav(Item inn)
        {
            _item = inn;
            itms = new List<Item>();

            Database db = Sitecore.Context.Database;
            Item vertNavLink = db.Items["/sitecore/templates/User Defined/Nav Link"];

            foreach (Item chi in _item.GetChildren())
            {
                if (chi.TemplateID.ToString() == vertNavLink.ID.ToString())
                {
                    itms.Add(chi);
                }
            }
        }
    }

    

  public partial class VerticalNavSublayout : System.Web.UI.UserControl 
	{

      private vertNav myItem = null;
      public vertNav MyItem
      {
          get
          {
              if (myItem == null)
              {
                  Database db = Sitecore.Context.Database;
                  Item item = Sitecore.Context.Item;

                  Item vertNavSublayout = db.Items["/sitecore/Layout/Sublayouts/VerticalNav"];

                  Item feedItem = vertNavSublayout;
                  Sublayout par = ((Sublayout)this.Parent);
                  string dsource = par.DataSource;
                  if (!string.IsNullOrEmpty(dsource))
                  {
                      feedItem = db.GetItem(dsource);
                  }

                  if (feedItem != vertNavSublayout)
                  {
                      myItem = new vertNav(feedItem);
                  }
              }
              return myItem;
          }
      }


        protected void Page_Load(object sender, EventArgs e) {
             if (Sitecore.Context.PageMode.IsPageEditorEditing)
			    {
				    mvVerticalNav.SetActiveView(viewPageEditor);
			    }
			    else
			    {
				    mvVerticalNav.SetActiveView(viewNormal);
			    }
        }
        protected void mvVerticalNav_ActiveViewChanged(object sender, EventArgs e)
		{
			switch (mvVerticalNav.ActiveViewIndex)
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
               lblError.Text += "Data Source is not defined. Add a data Source to VerticalNav Sublayout by selecting it under Advanced > Details > Controls.";
           }else{
               configItemName.Text = MyItem.InnerItem.Name;
               cssClass.Item = MyItem.InnerItem;
               NavTitle.Item = MyItem.InnerItem;
               editLinks.DataSource = MyItem.itms;
               editLinks.DataBind();
           }
      }

      private void RenderNormalView()
       {
           if (MyItem != null)
           {
               lvFeedReader.DataSource = MyItem.itms;
               lvFeedReader.DataBind();
               VertNavTitle.Text = MyItem.InnerItem.Fields["NavTitle"].Value;
               vertNavOutDiv.Attributes["Class"] = MyItem.InnerItem.Fields["css"].Value;
           }
           else
           {
               lblError.Text += "Error Rendering Navigation";
           }
       }

      public string ProperUrl(string url)
      {
          string lurl = url.ToLower();
          if (lurl.IndexOf("http") == 0 || lurl.IndexOf("ftp") == 0 || lurl.IndexOf("/")==0 || lurl.IndexOf("mailto")==0)
          {
              return url;
          }
          return "http://" + url;
      }
  }
}