using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace HealthIS.Layouts {
    public partial class BreadcrumbsSublayout : System.Web.UI.UserControl 
    {
        public HealthIS.Apps.Util.ItemDetail ThirdLevelItem = HealthIS.Apps.Util.getItemDetailAtLevel(3);
        public HealthIS.Apps.Util.ItemDetail DeptItem = HealthIS.Apps.Util.getItemDetailAtLevel(2);
        
        public List<HealthIS.Apps.Util.ItemDetail> CrumbList = new List<HealthIS.Apps.Util.ItemDetail>();

        private void Page_Load(object sender, EventArgs e) 
        {
            BuildBreadcrumbs();
        }

        private void BuildBreadcrumbs()
        {
            if (!String.IsNullOrEmpty(ThirdLevelItem.DisplayName)) {
                CrumbList.Insert(0, ThirdLevelItem);
            }
            CrumbList.Insert(0, DeptItem);
        }
    }
}