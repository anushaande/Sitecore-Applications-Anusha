using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace HealthIS.Apps.MVC.Models
{
    public class FacultyDirectoryNursingProfile : Sitecore.Mvc.Presentation.RenderingModel
    {       
        public int personID = -1;
        public bool validId = false;
        public HealthIS.Apps.FacultyDirectory.Profile person;

        public void GetNursingPersonData()
        {
            Sitecore.Marketing.Wildcards.WildcardTokenizedString wts = Sitecore.Marketing.Wildcards.WildcardManager.Provider.GetWildcardUrl(PageContext.Current.Item, Sitecore.Context.Site);
            System.Collections.Specialized.NameValueCollection tokens = wts.FindTokenValues(HttpContext.Current.Request.Path.ToLower().Replace(' ', '-'));
            string strPID = tokens["%Person_ID%"];
            if (strPID != null) { validId = Int32.TryParse(strPID, out personID); }
            if (validId && !Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                try
                {
                    person = new HealthIS.Apps.FacultyDirectory.Profile(personID, HealthIS.Apps.FacultyDirectory.Profile.ProfileType.Research, HealthIS.Apps.FacultyDirectory.Profile.ProfileParts.All);
                }
                catch { }
            }
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
                catch { }
            }
            return Tuple.Create(getPageTitle, getMetaDesc);
        }

        public int getNumberOfNavItems()
        {
            int count = 0;
            if (person.degrees != "") { count++; }
            if (person.clinicalInterests.Length > 0) { count++; }
            if (person.researchInterests.Length > 0) { count++; }
            if (person.awards.Length > 0) { count++; }
            if (person.memberships.Length > 0) { count++; }
            if (person.publications.Length > 0) { count++; }
            if (person.grants.Length > 0) { count++; }
            return count;
        }

        public static string StripTagsRegex(string source)
        {
            return System.Text.RegularExpressions.Regex.Replace(source, "<.*?>", string.Empty);
        }
    }
}