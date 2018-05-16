using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SiteMap
{
    public partial class SiteMap : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("web");

            Sublayout sublayout = Parent as Sublayout;
            NameValueCollection sublayoutParameters = Sitecore.Web.WebUtil.ParseUrlParameters(sublayout.Parameters);

            StringBuilder html = new StringBuilder();
            string path = sublayoutParameters["Start URL"];
            string onlyOneDepth = sublayoutParameters["Only One Depth"];
            //string path = "/sitecore/content/Home/medicine/animal_imaging_core";

            int count = path.Split('/').Length - 1;
            Sitecore.Data.Items.Item item = master.Items[path];


            if (item == null || String.IsNullOrEmpty(path) || count <= 4)
            {
                error.Text = "<div>\"" + path + "\" has not been found. " + count + " </div>";
                error.Text += "<div>The path should start with /sitecore/content/Home/department/theItemYouWantToCall/</div>";
                return;
            }

            if (item.HasChildren)
            {
                foreach (Sitecore.Data.Items.Item i in item.Children)
                {
                    Sitecore.Data.Items.Item item2 = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(i.ID));
                    if (item2 != null)
                    {
                        Sitecore.Data.Items.LayoutItem layoutItem = item2.Visualization.GetLayout(Sitecore.Context.Device);
                        if (layoutItem != null)
                        {
                            string newPath = i.Paths.Path.Replace("/sitecore/content/Home/", "http://health.usf.edu/");

                            html.Append("<tr>" +
                                "<td class=\"confluenceTd\">" + i.DisplayName + "</td>" +
                                "<td class=\"confluenceTd\"><a href=\"" + newPath + "\" class=\"external-link\" rel=\"nofollow\" target=\"_blank\">" + newPath + "</a></td>" +
                                "<td class=\"confluenceTd\"></td>" +
                                "<td class=\"confluenceTd\"></td>" +
                                "<td class=\"confluenceTd\"></td>" +
                                "<td class=\"confluenceTd\"></td>" +
                                "<td class=\"confluenceTd\"></td>" +
                                "</tr>");
                        }
                    }

                    if (!String.IsNullOrEmpty(onlyOneDepth))
                    {
                        foreach (Sitecore.Data.Items.Item i2 in i.Children)
                        {
                            Sitecore.Data.Items.Item item3 = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(i2.ID));
                            if (item3 != null)
                            {
                                Sitecore.Data.Items.LayoutItem layoutItem2 = item3.Visualization.GetLayout(Sitecore.Context.Device);
                                if (layoutItem2 != null)
                                {
                                    string newPath2 = i2.Paths.Path.Replace("/sitecore/content/Home/", "http://health.usf.edu/");

                                    html.Append("<tr>" +
                                    "<td class=\"confluenceTd\">" + i2.DisplayName + "</td>" +
                                    "<td class=\"confluenceTd\"><a href=\"" + newPath2 + "\" class=\"external-link\" rel=\"nofollow\" target=\"_blank\">" + newPath2 + "</a></td>" +
                                    "<td class=\"confluenceTd\"></td>" +
                                    "<td class=\"confluenceTd\"></td>" +
                                    "<td class=\"confluenceTd\"></td>" +
                                    "<td class=\"confluenceTd\"></td>" +
                                    "<td class=\"confluenceTd\"></td>" +
                                    "</tr>");
                                }

                                foreach (Sitecore.Data.Items.Item i3 in i2.Children)
                                {
                                    Sitecore.Data.Items.Item item4 = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(i3.ID));
                                    if (item4 != null)
                                    {
                                        Sitecore.Data.Items.LayoutItem layoutItem3 = item4.Visualization.GetLayout(Sitecore.Context.Device);
                                        if (layoutItem3 != null)
                                        {
                                            string newPath3 = i3.Paths.Path.Replace("/sitecore/content/Home/", "http://health.usf.edu/");

                                            html.Append("<tr>" +
                                                "<td class=\"confluenceTd\">" + i3.DisplayName + "</td>" +
                                                "<td class=\"confluenceTd\"><a href=\"" + newPath3 + "\" class=\"external-link\" rel=\"nofollow\" target=\"_blank\">" + newPath3 + "</a></td>" +
                                                "<td class=\"confluenceTd\"></td>" +
                                                "<td class=\"confluenceTd\"></td>" +
                                                "<td class=\"confluenceTd\"></td>" +
                                                "<td class=\"confluenceTd\"></td>" +
                                                "<td class=\"confluenceTd\"></td>" +
                                                "</tr>");
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
            lists.Text = html.ToString();
        }
    }
}