using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.TreeDisplay
{
    public partial class SiteMap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string siteMap = "<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>";//"<?xml version='1.0' encoding='UTF-8'?>\n<urlset xmlns='http://www.sitemaps.org/schemas/sitemap/0.9'>";
            try{
                Sitecore.Data.Items.Item home = Sitecore.Data.Database.GetDatabase("master").GetItem("/sitecore/content/Home");
                if(home != null){
                    foreach(Sitecore.Data.Items.Item i in home.Children){
                        siteMap += DisplayChildren(i);
                    }
                }
                else{
                    siteMap += "<Error>Cannot find home node.</Error>";
                }
            }catch(Exception ex){
                siteMap += "<Error>Error:</Error>\n<Message>" + ex.Message + "</Message>\n<StackTrace>" + ex.StackTrace + "</StackTrace>";
            }
            //Response.Write(siteMap);
            siteMap += "</urlset>";
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(siteMap);
            Response.ContentType = "application/xml";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Expires = -1;
            Response.Cache.SetAllowResponseInBrowserHistory(true); //"works around an Internet&nbsp;Explorer bug"
            doc.Save(Response.Output); //doc saves itself to the textwriter, using the encoding of the text-writer (which comes from response.contentEncoding)

        }

        public string DisplayChildren(Sitecore.Data.Items.Item parent)
        {
            string content = "";
            string iconName = parent.Appearance.Icon.ToLower();
            Sitecore.Layouts.LayoutDefinition layout = Sitecore.Layouts.LayoutDefinition.Parse(parent[Sitecore.FieldIDs.LayoutField]);
            if (layout != null && parent[Sitecore.FieldIDs.LayoutField].Length > 0)
            {
                Sitecore.Links.UrlOptions options = new Sitecore.Links.UrlOptions();
                options.AlwaysIncludeServerUrl = true;
                string loc = Sitecore.Links.LinkManager.GetItemUrl(parent, options);
                string lastMod = parent.Statistics.Updated.ToLongDateString();
                content += "\n\t<url>\n\t\t<loc>" + loc + "</loc>\n\t\t<lastmod>" + lastMod + "</lastmod>\n\t</url>";                
                if (parent.HasChildren)
                {
                    string childContent = "";
                    foreach (Sitecore.Data.Items.Item child in parent.Children)
                    {
                        if (!child.Name.Contains("Resources"))
                        {
                            childContent += DisplayChildren(child);
                        }
                    }
                    if (!string.IsNullOrEmpty(childContent))
                    {
                        content += childContent;
                    }
                }
            }
            return content;
        }
    }
}