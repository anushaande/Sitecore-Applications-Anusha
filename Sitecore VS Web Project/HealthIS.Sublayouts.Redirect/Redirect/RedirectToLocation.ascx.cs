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
using HealthIS.Apps;

namespace HealthIS.apps.SubLayouts
{
    public partial class RedirectToLocation : System.Web.UI.UserControl
    {
        private void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here

            Sitecore.Data.Items.Item MyItem = null;
            Sitecore.Web.UI.WebControls.Sublayout myLayout = ((Sitecore.Web.UI.WebControls.Sublayout)this.Parent);
            
            if (String.IsNullOrEmpty(myLayout.DataSource) || myLayout.DataSource.Equals("RedirectDataNewDataSource"))
            {
                MyItem = HealthIS.Apps.Util.createDatasourceItem("User Defined/Sublayouts/RedirectData", "RedirectDataNewDataSource", "_RD");
                //Response.Write("<script type='text/javascript'>setTimeout(function(){location.reload();},3000);</script>");
            }
            else
                MyItem = Sitecore.Context.Database.GetItem(myLayout.DataSource);
            if (MyItem == null)
            {
                Response.Write("No DatasourceDefined");
                return;
            }

            if (Sitecore.Context.PageMode.IsPageEditorEditing)
            {
                loc.Item = MyItem;
                sc.Item = MyItem;
            }
            else
            {
                string urlEnding = MyItem.Fields["New Location"].Value.ToString();
                /*if (HealthIS.Apps.Util.isOnProduction())
                {
                    Response.Redirect(urlEnding, false);

                }
                else
                {
                    Response.Redirect( urlEnding, false);
                }*/
                Response.Redirect( urlEnding, false);
                Response.StatusCode = System.Convert.ToInt16(MyItem.Fields["Status Code"].Value.ToString());
                Response.End();
                Response.Write("url:" + urlEnding);
            }

        }
    }

}