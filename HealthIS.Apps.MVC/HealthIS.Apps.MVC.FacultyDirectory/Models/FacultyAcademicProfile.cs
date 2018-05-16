using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace HealthIS.Apps.MVC.FacultyDirectory
{
    public class FacultyAcademicProfile : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool validId;
        public int personID = -1;
        public HealthIS.Apps.FacultyDirectory.Profile person;
        public bool dsIsSet { get; set; }
        bool disposed = false;
        public string strPID { get; set; }
        public string Errors = "";

        public override void Initialize(Rendering rendering)
        {
            try
            {
                base.Initialize(rendering);
                Rendering = rendering;
                dsIsSet = !String.IsNullOrEmpty(rendering.DataSource) && Sitecore.Context.Database.GetItem(rendering.DataSource) == null;

                Sitecore.Marketing.Wildcards.WildcardTokenizedString wts = Sitecore.Marketing.Wildcards.WildcardManager.Provider.GetWildcardUrl(PageItem, Sitecore.Context.Site);
                System.Collections.Specialized.NameValueCollection tokens = wts.FindTokenValues(HttpContext.Current.Request.Path.ToLower().Replace(' ', '-'));
                strPID = tokens["%Person_ID%"];
                if (strPID != null) { validId = Int32.TryParse(strPID, out personID); }
                if (validId && !Sitecore.Context.PageMode.IsExperienceEditorEditing)
                {
                    try
                    {
                        person = new HealthIS.Apps.FacultyDirectory.Profile(personID, HealthIS.Apps.FacultyDirectory.Profile.ProfileType.Research, HealthIS.Apps.FacultyDirectory.Profile.ProfileParts.All);
                        //Page.Title = person.firstName + " " + person.lastName + " - USF Health - Tampa, FL";
                    }
                    catch (Exception ex)
                    {
                        //errorText.Text += "<br/>"+id;
                        //errorText.Text = ex.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Errors += ex.Message + "<br/>" + ex.InnerException + "<br/>" + ex.StackTrace;
            }
	    }
		
		public string FormatAppt(string str){
			return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower()).Replace(" Of "," of ").Replace(" Usf "," USF ");
		}

        public static string pageTitle()
        {
            string name = "";
            Sitecore.Marketing.Wildcards.WildcardTokenizedString wts = Sitecore.Marketing.Wildcards.WildcardManager.Provider.GetWildcardUrl(PageContext.Current.Item, Sitecore.Context.Site);
            System.Collections.Specialized.NameValueCollection tokens = wts.FindTokenValues(HttpContext.Current.Request.Path.ToLower().Replace(' ', '-'));
            string strPID = tokens["%Person_ID%"];
            int pid = -1;
            bool validId = false;
            if (strPID != null) { validId = Int32.TryParse(strPID, out pid); }
            if (validId && !Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                try
                {
                    HealthIS.Apps.FacultyDirectory.Profile p = new HealthIS.Apps.FacultyDirectory.Profile(pid, HealthIS.Apps.FacultyDirectory.Profile.ProfileType.Research, HealthIS.Apps.FacultyDirectory.Profile.ProfileParts.All);
                    name = p.firstName + " " + p.lastName;
                    if (!String.IsNullOrEmpty(p.title))
                    {
                        name += ", " + p.title;
                    }
                    var pgField = PageContext.Current.Item.Fields["Page Title"];
                    if (pgField != null)
                    {
                        name += " | " + pgField.Value;
                    }
                }
                catch { }
            }
            return name;
        }

        public static string pageMetaDesc()
        {
            string name = "";
            Sitecore.Marketing.Wildcards.WildcardTokenizedString wts = Sitecore.Marketing.Wildcards.WildcardManager.Provider.GetWildcardUrl(PageContext.Current.Item, Sitecore.Context.Site);
            System.Collections.Specialized.NameValueCollection tokens = wts.FindTokenValues(HttpContext.Current.Request.Path.ToLower().Replace(' ', '-'));
            string strPID = tokens["%Person_ID%"];
            int pid = -1;
            bool validId = false;
            if (strPID != null) { validId = Int32.TryParse(strPID, out pid); }
            if (validId && !Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                try
                {
                    HealthIS.Apps.FacultyDirectory.Profile p = new HealthIS.Apps.FacultyDirectory.Profile(pid, HealthIS.Apps.FacultyDirectory.Profile.ProfileType.Research, HealthIS.Apps.FacultyDirectory.Profile.ProfileParts.All);
                    name = p.firstName + " " + p.lastName;
                    if (!String.IsNullOrEmpty(p.title))
                    {
                        name += ", " + p.title;
                    }
                    var pgField = PageContext.Current.Item.Fields["Page Title"];
                    if (pgField != null)
                    {
                        name += " - " + pgField.Value;
                    }
                    if (!string.IsNullOrEmpty(p.degrees))
                    {
                        name += " - " + p.degrees.Replace("</li><li>", ", ");
                    }
                }
                catch { }
            }
            return StripTagsRegex(name);
        }

        public static string StripTagsRegex(string source)
        {
            return System.Text.RegularExpressions.Regex.Replace(source, "<.*?>", string.Empty);
        }

        public void ProfileNotFound()
        {
            Sitecore.Data.Items.Item notFoundPage = null;
            notFoundPage = Sitecore.Context.Database.GetItem("/sitecore/content/Home/PageNotFound");
            if (notFoundPage != null)
            {
                //HttpContext.Current.Response.RedirectPermanent(Sitecore.Links.LinkManager.GetItemUrl(notFoundPage), true);
                HttpContext.Current.Response.StatusCode = 404;
                HttpContext.Current.Response.Redirect(Sitecore.Links.LinkManager.GetItemUrl(notFoundPage), true);
            }
        }
    }
}