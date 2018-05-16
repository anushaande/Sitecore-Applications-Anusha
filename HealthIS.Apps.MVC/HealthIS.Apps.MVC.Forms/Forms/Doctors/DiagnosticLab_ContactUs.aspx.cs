using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Forms.Doctors
{
    public partial class DiagnosticLab_ContactUs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string from = Sitecore.Configuration.Settings.GetSetting("DiagnosticLab_ContactEmailFromAddr", "");
            string to = Sitecore.Configuration.Settings.GetSetting("DiagnosticLab_ContactEmailToAddr", "");
            string sub = Sitecore.Configuration.Settings.GetSetting("DiagnosticLab_ContactEmailSubject", "");
            HISForm.SubmitForm(pnlFormStart, pnlFormComplete, from, to, sub, null);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            HISForm.ClearTextBoxes(form1);
        }
    }
}