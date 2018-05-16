using System;

namespace HealthIS.Apps.MVC.Forms
{
    public partial class SIPAIID_MemberInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string from = Sitecore.Configuration.Settings.GetSetting("SIPAIID_MemberInfoEmailFromAddr", "");
            string to = Sitecore.Configuration.Settings.GetSetting("SIPAIID_MemberInfoEmailToAddr", "");
            string sub = Sitecore.Configuration.Settings.GetSetting("SIPAIID_MemberInfoEmailSubject", "");
            HISForm.SubmitForm(pnlFormStart, pnlFormComplete, from, to, sub, null);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            HISForm.ClearTextBoxes(form1);
        }
    }
}