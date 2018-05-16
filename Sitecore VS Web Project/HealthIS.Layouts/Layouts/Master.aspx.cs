using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace HealthIS.Layouts {

    public partial class MasterLayout : System.Web.UI.Page
    {
        public string TemplateStyles;
        public string TemplateScript;
        public string CollegeStyles;
        public string CollegeScript;
        public string DeptStyles;
        public string DeptScript;
        public string PageStyles;
        public string PageScript;
        public string MetaKeywords;
        public string MetaDescription;
        public string CustomCssStyle;

        public string deptName = "";
        public Dictionary<string, string> sw = null;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Check if sources for Template, Department, and Page resources have been defined
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Template Styles Path"]))
            {
                TemplateStyles = "<link rel=\"stylesheet\" href=\"" + Sitecore.Context.Item["Template Styles Path"] + "\">";
            }
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Template Scripts Path"]))
            {
                TemplateScript= "<script src=\"" + Sitecore.Context.Item["Template Scripts Path"] + "\"></script>";
            }
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["College Styles Path"]))
            {
                CollegeStyles = "<link rel=\"stylesheet\" href=\"" + Sitecore.Context.Item["College Styles Path"] + "\">";
            }
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["College Scripts Path"]))
            {
                CollegeScript = "<script src=\"" + Sitecore.Context.Item["College Scripts Path"] + "\"></script>";
            }
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Department Styles Path"]))
            {
                DeptStyles = "<link rel=\"stylesheet\" href=\"" + Sitecore.Context.Item["Department Styles Path"] + "\">";
            }
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Department Scripts Path"]))
            {
                DeptScript = "<script src=\"" + Sitecore.Context.Item["Department Scripts Path"] + "\"></script>";
            }
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Page Styles Path"]))
            {
                PageStyles = "<link rel=\"stylesheet\" href=\"" + Sitecore.Context.Item["Page Styles Path"] + "\">";
            }
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Page Scripts Path"]))
            {
                PageScript = "<script src=\"" + Sitecore.Context.Item["Page Scripts Path"] + "\"></script>";
            }
            // Check if meta tags have been defined on the page
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Meta Keywords"]))
            {
                MetaKeywords = "<meta name=\"keywords\" content=\"" + Sitecore.Context.Item["Meta Keywords"] + "\">";
            }
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Meta Description"]))
            {
                MetaDescription = "<meta name=\"description\" content=\"" + Sitecore.Context.Item["Meta Description"] + "\">";
            }
            // Added for Custom CSS Style of Sub-Pages, Add 01/27/2015 by Jihyun
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Custom CSS Style"]))
            {
                CustomCssStyle = "<style type='text/css'>" + Sitecore.Context.Item["Custom CSS Style"] + "</style>";
            }
        }

        private void Page_Load(object sender, EventArgs e) 
        {
            if (!String.IsNullOrEmpty(Sitecore.Context.Item["Meta Title"]))
            {
                Page.Title = Sitecore.Context.Item["Meta Title"] + " | USF Health";
            }
            else
            {
                Page.Title = Sitecore.Context.Item.DisplayName + " | USF Health";
            }

            deptName = HealthIS.Apps.Util.getItemNameAtLevel(2);

            // setup sitewit IDs
            sw = new Dictionary<string, string>();
            sw.Add("medicine",        "1306");
            sw.Add("nursing",         "1307");
            sw.Add("publichealth",    "1308");
            sw.Add("pharmacy",        "1309");
            sw.Add("paperfree",       "1310");

            Response.AddHeader("X-UA-Compatible", "IE=Edge");
        }
    }
}