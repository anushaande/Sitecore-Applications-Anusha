using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Forms
{
    public partial class Peds_JoinUs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string from = Sitecore.Configuration.Settings.GetSetting("Peds_JoinUsEmailFromAddr", "");
            string to = Sitecore.Configuration.Settings.GetSetting("Peds_JoinUsEmailToAddr", "");
            string sub = Sitecore.Configuration.Settings.GetSetting("Peds_JoinUsEmailSubject", "");
            HISForm.SubmitForm(pnlFormStart, pnlFormComplete, from, to, sub, null);
        }
    }
}