using System;
using System.Web.UI.HtmlControls;

namespace HealthIS.Sublayouts.Academic {

  public partial class Dpt_sublayoutSublayout : System.Web.UI.UserControl 
    {
        private string cssPath = "/sublayouts/medicine/dpt/styles/coptrs.css";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            HtmlGenericControl link = new HtmlGenericControl("link");
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("href", cssPath);
            Controls.Add(link);
            Page.Header.Controls.Add(link);
        }

        private void Page_Load(object sender, EventArgs e) 
        {
          // Put user code to initialize the page here
        }       
    }
}