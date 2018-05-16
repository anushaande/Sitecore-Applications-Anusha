using System;

namespace HealthIS.Sublayouts.Components.Marquee
{

    public partial class MarqueeSublayout : System.Web.UI.UserControl 
    {
        public string marqueeText;

        private void Page_Load(object sender, EventArgs e)
        {
            var sublayout = Parent as Sitecore.Web.UI.WebControls.Sublayout;
            var parameters = Sitecore.Web.WebUtil.ParseUrlParameters(sublayout.Parameters);

            marqueeText = Server.UrlDecode(parameters.Get("marqueeText"));
        }
    }
}