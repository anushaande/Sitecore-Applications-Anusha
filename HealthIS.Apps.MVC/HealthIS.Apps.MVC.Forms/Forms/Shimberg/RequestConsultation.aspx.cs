using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Forms
{
    public partial class RequestConsultation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string userEmail = Request.Form["tb_Email_Address"].ToString().ToLower();
            //string from = Sitecore.Configuration.Settings.GetSetting("ShimLib_EmailFromAddr", "");            
            string from = userEmail;
            string to = Sitecore.Configuration.Settings.GetSetting("SL_ReqConsultEmailToAddr", "") + "; " + userEmail;
            string sub = Sitecore.Configuration.Settings.GetSetting("SL_ReqConsultEmailSubject", "");
            HISForm.SubmitForm(pnlFormStart, pnlFormComplete, from, to, sub, null);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            HISForm.ClearTextBoxes(form1);
        }
    }
}