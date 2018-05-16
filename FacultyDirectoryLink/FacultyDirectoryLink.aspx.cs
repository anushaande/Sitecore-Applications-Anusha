using System;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Newtonsoft.Json.Serialization;
using System.Data;
using HealthIS.Apps;

namespace Sitecore.HIS.Apps
{
    public partial class FacultyDirectoryLink : System.Web.UI.Page
    {
        [Flags]
        public enum ProfileFilesAvailable 
        {
            None = 0,
            Picture = 1,
            CV = 2,
            All = 3
        }

        private string getParameter(string paramName)
        {
            string ret = "";

            try
            {
                ret = Request.QueryString[paramName].ToString();
            }
            catch (Exception) { }

            if (ret == "")
            {
                try
                {
                    ret = Request.Form[paramName].ToString() ;
                }
                catch (Exception) { }
            }
            return ret;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            switch (getParameter("method").ToLower())
            {
                case "ajaxacresearchname":
                    ajaxACResearchName(getParameter("term"));
                    break;
                case "ajaxacfacultyname":
                    ajaxACClinicalName(getParameter("fname"), getParameter("lname"), getParameter("hscid"));
                    break;
                case "getpicture":
                    HealthIS.Apps.FacultyDirectory.getPicture(getParameter("person_id"), Response);
                    break;
                case "getresume":
                    HealthIS.Apps.FacultyDirectory.getResume(getParameter("person_id"), Response);
                    break;
                case "ajaxfacultylist":
                    ajaxFacultyList(Decimal.Parse(getParameter("dept_id")));
                    break;
                default:
                    Response.Write("Error: Method not found");
                    break;
            }
        }

        private void ajaxACResearchName(string term)
        {
            //I have person list, need to output it to jquery.
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(HealthIS.Apps.FacultyDirectory.GetACResearchName(term)));
        }

        
        private void ajaxACClinicalName(string fname, string lname, string hscid)
        {
            //I have person list, need to output it to jquery.
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(HealthIS.Apps.FacultyDirectory.GetACClinicalName(fname,lname,hscid)));
        }
            

        public void ajaxFacultyList(decimal dept_id)
        {
            System.Collections.Generic.List<FacultyDirectory.Faculty> ret = HealthIS.Apps.FacultyDirectory.getFacultyForDepartment(dept_id);
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ret));
        }
    }
}