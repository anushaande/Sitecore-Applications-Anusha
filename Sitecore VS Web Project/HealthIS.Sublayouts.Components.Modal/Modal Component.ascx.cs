using System;

namespace HealthIS.Sublayouts.Components.Modal_component {
  
    public partial class Modal_componentSublayout : System.Web.UI.UserControl 
    {
        public string ModalID;
        public string ModalHeaderClass;        
        public string ModalHeaderText;
        public string ModalBodyClass;

        private void Page_Load(object sender, EventArgs e) 
        {
            var sublayout = Parent as Sitecore.Web.UI.WebControls.Sublayout;
            var parameters = Sitecore.Web.WebUtil.ParseUrlParameters(sublayout.Parameters);

            if (parameters["Modal ID"] == "")
            {
                ModalID = "myModal";
            }
            else 
            {
                ModalID = Server.UrlDecode(parameters.Get("Modal ID"));
            }

            if (parameters["Modal Header Class"] == "")
            {
                ModalHeaderClass = "";
            }
            else 
            {
                ModalHeaderClass = Server.UrlDecode(parameters.Get("Modal Header Class"));
            }

            if (parameters["Modal Header Text"] == "")
            {
                return;
            }
            else
            {
                ModalHeaderText = Server.UrlDecode(parameters.Get("Modal Header Text"));
                ModalHeader.Text = ModalHeaderText;
            }

            if (parameters["Modal Body Class"] == "")
            {
                return;
            }
            else 
            {
                ModalBodyClass = Server.UrlDecode(parameters.Get("Modal Body Class"));
            }
        }
    }
}