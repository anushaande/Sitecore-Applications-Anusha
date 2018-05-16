using System;
using System.Web.UI.HtmlControls;
using Sitecore;

namespace HealthIS.Layouts {

    public partial class CorporateSublayout : System.Web.UI.UserControl 
    {

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        private void Page_Load(object sender, EventArgs e) 
        {
            SetModifiedDate();
        }

        public void SetModifiedDate()
        {
            DateTime postDate = Sitecore.Context.Item.Statistics.Updated;
            litModifiedDate.Text = postDate.ToString("MM/dd/yyyy");
        }
    }
}