using System;

namespace HealthIS.PatientToolbox
{

    /// <summary>
    /// Summary description for PatienttoolboxSublayout
    /// </summary>
    public partial class PatientToolboxSublayout : System.Web.UI.UserControl
    {
        public string PatientToolboxNumber;
        public string Title1;
        public string Title2;
        public string Title3;
        public string URL1;
        public string URL2;
        public string URL3;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            PatientToolboxNumber = Sitecore.Context.Item["PatientToolbox Number"];
            Title1 = Sitecore.Context.Item["Title 1"];
            Title2 = Sitecore.Context.Item["Title 2"];
            Title3 = Sitecore.Context.Item["Title 3"];
            URL1 = Sitecore.Context.Item["URL 1"];
            URL2 = Sitecore.Context.Item["URL 2"];
            URL3 = Sitecore.Context.Item["URL 3"];
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here

        }
    }
}