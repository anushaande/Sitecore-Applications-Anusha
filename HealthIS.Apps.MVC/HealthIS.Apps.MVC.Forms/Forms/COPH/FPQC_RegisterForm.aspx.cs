using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Forms.COPH
{
    public partial class FPQC_RegisterForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string from = Sitecore.Configuration.Settings.GetSetting("FPQC_RegisterEmailFromAddr", "");
            string to = Sitecore.Configuration.Settings.GetSetting("FPQC_RegisterEmailToAddr", "");
            string sub = Sitecore.Configuration.Settings.GetSetting("FPQC_RegisterEmailSubject", "");
            HISForm.SubmitForm(pnlFormStart, pnlFormComplete, from, to, sub, null);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            HISForm.ClearTextBoxes(form1);
        }
    }
}