using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.IO;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using System;
using System.Web;
using System.Web.UI;

namespace HealthIS.Apps.MVC.UserActivityLog
{
    public class UserLogDetailsForm : BaseForm
    {
        protected Scrollbox LogViewer;
        protected Border TextPanel;

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull((object)e, "e");
            Assert.CanRunApplication("/sitecore/content/Applications/Tools/User Log");
            base.OnLoad(e);
            if (Context.ClientPage.IsEvent)
                return;
            string queryString = WebUtil.GetQueryString("file");
            if (string.IsNullOrEmpty(queryString))
                return;
            string path = FileUtil.MapPath(queryString);
            if (!path.StartsWith(FileUtil.MapPath(Settings.LogFolder), StringComparison.InvariantCultureIgnoreCase))
                return;
            this.TextPanel.Visible = false;
            string s = FileUtil.ReadFromFile(path);
            if (string.IsNullOrEmpty(s))
                this.LogViewer.Controls.Add((System.Web.UI.Control)new LiteralControl(Translate.Text("This file is empty or cannot be opened for reading.")));
            else
                this.LogViewer.Controls.Add((System.Web.UI.Control)new LiteralControl(HttpUtility.HtmlEncode(s).Replace("\n", "<hr/>")));
        }
    }
}
