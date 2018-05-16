using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace HealthIS.Apps.MVC.Models
{
    public class FacultyDirectoryCorporateProfile : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool validId;
        public int personID = -1;
        public HealthIS.Apps.FacultyDirectory.Profile person;
        public Rendering rendering { get; set; }
        public Item pageItem { get; set; }
        public string strPID { get; set; }
        public string showError { get; set; }

        public string FormatAppt(string str)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower()).Replace(" Of ", " of ").Replace(" Usf ", " USF ");
        }

        public static Tuple<string, string> GetHeaderData()
        {
            string getPageTitle = "";
            string getMetaDesc = "";
            int pid = -1;
            bool validId = false;

            Sitecore.Marketing.Wildcards.WildcardTokenizedString wts = Sitecore.Marketing.Wildcards.WildcardManager.Provider.GetWildcardUrl(PageContext.Current.Item, Sitecore.Context.Site);
            System.Collections.Specialized.NameValueCollection tokens = wts.FindTokenValues(HttpContext.Current.Request.Path.ToLower().Replace(' ', '-'));
            string strPID = tokens["%Person_ID%"];

            if (strPID != null) { validId = Int32.TryParse(strPID, out pid); }
            if (validId && !Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                try
                {
                    HealthIS.Apps.FacultyDirectory.Profile p = new HealthIS.Apps.FacultyDirectory.Profile(pid, HealthIS.Apps.FacultyDirectory.Profile.ProfileType.Research, HealthIS.Apps.FacultyDirectory.Profile.ProfileParts.All);

                    getPageTitle = p.firstName + " " + p.lastName;
                    getMetaDesc = p.firstName + " " + p.lastName;
                    if (!String.IsNullOrEmpty(p.title))
                    {
                        getPageTitle += ", " + p.title;
                        getMetaDesc += ", " + p.title;
                    }
                    var pgField = PageContext.Current.Item.Fields["Page Title"];
                    if (!String.IsNullOrEmpty(pgField.ToString().Trim()))
                    {
                        getPageTitle += " | " + pgField.Value;
                        getMetaDesc += " - " + pgField.Value;
                    }

                    // Meta Description
                    if (!string.IsNullOrEmpty(p.degrees))
                    {
                        getMetaDesc += StripTagsRegex(" - " + p.degrees.Replace("</li><li>", ", "));
                    }
                }
                catch {
                    getPageTitle = "";
                    getMetaDesc = "";
                }
            }
            return Tuple.Create(getPageTitle, getMetaDesc);
        }

        public static string StripTagsRegex(string source)
        {
            return System.Text.RegularExpressions.Regex.Replace(source, "<.*?>", string.Empty);
        }

        //public void ProfileNotFound()
        //{
        //    Sitecore.Data.Items.Item notFoundPage = null;
        //    notFoundPage = Sitecore.Context.Database.GetItem("/sitecore/content/Home/PageNotFound");
        //    if (notFoundPage != null)
        //    {
        //        //HttpContext.Current.Response.RedirectPermanent(Sitecore.Links.LinkManager.GetItemUrl(notFoundPage), true);
        //        HttpContext.Current.Response.StatusCode = 404;
        //        HttpContext.Current.Response.Redirect(Sitecore.Links.LinkManager.GetItemUrl(notFoundPage), true);
        //    }
        //}
    }
}