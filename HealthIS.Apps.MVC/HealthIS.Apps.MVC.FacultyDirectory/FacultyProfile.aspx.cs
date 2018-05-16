using Oracle.DataAccess.Client;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.FacultyDirectory
{
    public partial class FacultyProfile : System.Web.UI.Page
    {
        private HealthIS.Apps.FacultyDirectory.Profile Profile { get; set; }
        private string Title = "";

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
                    ret = Request.Form[paramName].ToString();
                }
                catch (Exception) { }
            }
            return ret;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            string pid = getParameter("personID");
            string scItemID = getParameter("scItemID");
            if (!string.IsNullOrEmpty(scItemID))
            {
                Item i = Sitecore.Context.Database.GetItem(scItemID);
                if (i != null)
                {
                    pid = Helpers.getStrField(i, "Person ID");
                    Title = Helpers.getStrField(i, "Job Title");
                }
            }
            if (!string.IsNullOrEmpty(pid))
            {
                Profile = new Apps.FacultyDirectory.Profile(int.Parse(pid));
                if (string.IsNullOrEmpty(Title))
                {
                    if (!string.IsNullOrEmpty(Profile.title))
                    {
                        Title = Profile.title + " " + Profile.department;
                    }
                    else
                    {
                        Title = GetTitle(pid);
                    }
                }
            }
            if (Profile != null)
            {
                Response.Write("<h1>" + Profile.firstName + " " + Profile.lastName + "</h1>");
                Response.Write("<h2>" + Title + "</h2>");
                Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Profile));
            }
            else
            {
                Response.Write("NOPE. NOTHIN HERE TO SEE.");
            }

        }

        public string GetTitle(string personID)
        {
            using (OracleConnection conn = HealthIS.Apps.Util.getDBConnection())
            {
                string strComm = "research_dir_user.RESEARCHDIR.sel_appt_positions";
                OracleCommand comm = new OracleCommand(strComm, conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.Add("p_person_id", OracleDbType.Long, personID, ParameterDirection.Input);
                comm.Parameters.Add("p_appt_positions", OracleDbType.RefCursor, ParameterDirection.Output);
                conn.Open();
                comm.ExecuteNonQuery();
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(comm);
                DataSet dataSet = new DataSet();
                oracleDataAdapter.Fill(dataSet);

                string title = "";
                try { title = dataSet.Tables[0].Rows[0].Field<string>("Title") + " " + dataSet.Tables[0].Rows[0].Field<string>("GLOBAL_DEPT_NAME"); }
                catch { }
                return title;
            }
        }
    }
}