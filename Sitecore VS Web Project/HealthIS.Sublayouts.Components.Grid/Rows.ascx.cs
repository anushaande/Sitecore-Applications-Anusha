using System;

namespace HealthIS.Sublayouts.Components.Grid
{
    public partial class RowsSublayout : System.Web.UI.UserControl
    {
        public string ContainerClass;
        public string ContainerID;
        public string Row1Class;
        public string Row1ID;
        public string Row2Class;
        public string Row2ID;
        public string Row3Class;
        public string Row3ID;

        private void Page_Load(object sender, EventArgs e)
        {

            var sublayout = Parent as Sitecore.Web.UI.WebControls.Sublayout;
            var parameters = Sitecore.Web.WebUtil.ParseUrlParameters(sublayout.Parameters);

            ContainerClass = Server.UrlDecode(parameters.Get("ContainerClass"));
            ContainerID = Server.UrlDecode(parameters.Get("ContainerID"));
            Row1Class = Server.UrlDecode(parameters.Get("Row1Class"));
            Row1ID = Server.UrlDecode(parameters.Get("Row1ID"));
            Row2Class = Server.UrlDecode(parameters.Get("Row2Class"));
            Row2ID = Server.UrlDecode(parameters.Get("Row2ID"));
            Row3Class = Server.UrlDecode(parameters.Get("Row3Class"));
            Row3ID = Server.UrlDecode(parameters.Get("Row3ID"));
        }
    }
}