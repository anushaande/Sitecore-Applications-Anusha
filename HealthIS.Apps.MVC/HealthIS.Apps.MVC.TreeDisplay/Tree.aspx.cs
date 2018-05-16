using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.TreeDisplay
{
    public partial class Tree : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnShowTree_Click(object sender, EventArgs e)
        {
            try{
                Sitecore.Data.Items.Item home = Sitecore.Data.Database.GetDatabase("web").GetItem("/sitecore/content/Home");
                if(home != null){
                        foreach(Sitecore.Data.Items.Item i in home.Children){
                            DisplayChildren(i);
                        }
                }
                else{
                    litTree.Text += "Cannot find home node.";
                }
            }catch(Exception ex){
                pnlErr.Visible = true;
                litErr.Text += "Error: " + ex.Message + "<br/>" + ex.StackTrace;
            }
            udpTree.Update();
        }

        public void DisplayChildren(Sitecore.Data.Items.Item parent)
        {
            //string iconName = parent.Appearance.Icon.ToLower();
            Sitecore.Layouts.LayoutDefinition layout = Sitecore.Layouts.LayoutDefinition.Parse(parent[Sitecore.FieldIDs.LayoutField]);
            if (layout != null && parent[Sitecore.FieldIDs.LayoutField].Length > 0)//(iconName.Contains("document") || iconName.Contains("folder")) && !iconName.Contains("Resources"))
            {
                Sitecore.Links.UrlOptions options = new Sitecore.Links.UrlOptions();
                options.AlwaysIncludeServerUrl = true;
                string href = Sitecore.Links.LinkManager.GetItemUrl(parent, options);
                litTree.Text += "<li><b><a href=" + href + " target='_blank'>" + parent.Name + "</a></b><br/>" + href + "<br/>Status code: " + GetHttpStatus(href);
                udpTree.Update();
                if (parent.HasChildren)
                {
                    bool closeIt = false;
                    bool opened = false;
                    foreach (Sitecore.Data.Items.Item child in parent.Children)
                    {
                        if (!child.Name.Contains("Resources"))
                        {
                            if (!opened) { litTree.Text += "<ul>"; opened = true; }
                            DisplayChildren(child);
                            closeIt = true;
                        }
                    }
                    if (closeIt)
                    {
                        litTree.Text += "</ul>";
                    }
                }
                litTree.Text += "</li>";
            }
        }

        public static HttpStatusCode GetHttpStatus(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 1500;
            request.Method = "HEAD";
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    HttpWebResponse hwp = (HttpWebResponse)response;
                    return hwp.StatusCode;
                }
            }
            catch (WebException)
            {
                return HttpStatusCode.RequestTimeout;
            }
        }
    }
}