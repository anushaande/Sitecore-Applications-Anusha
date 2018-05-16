using System;
using ClinicalProfile;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Web.UI;

namespace Sitecore.HIS.Apps
{
    public partial class FacultyDirectory : System.Web.UI.Page
    {
        public FacultyDirectory()
        {
        }

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
                    getPicture(getParameter("person_id"));
                    break;
                case "getresume":
                    getResume(getParameter("person_id"));
                    break;
                case "ajaxfacultylist":
                    ajaxFacultyList(Decimal.Parse(getParameter("dept_id")));
                    break;
                case "getapptpositions":
                    getApptPositions(getParameter("person_id"));
                    break;
                case "searchhd":
                    SearchHD(getParameter("fname"), getParameter("lname"));
                    break;
                case "listfields":
                    ListFields(getParameter("person_id"));
                    break;
                default:
                    Response.Write("Error: Method not found");
                    break;
            }
        }

        private static string getConnectionString()
        {
            string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbdev2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBPREP)(SERVER=DEDICATED)));User Id=profile_views_web;Password=nordoga;";
            if (ClinicalProfile.Profile.isOnProduction())
            {
                strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=hscdb4.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=profile_views_web;Password=nordoga;";
            }
            return strConnect;
        }

        public void getApptPositions(string personID)
        {
            string json = "Error retrieving Appointment Positions";
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
                json = Newtonsoft.Json.JsonConvert.SerializeObject(dataSet, new Newtonsoft.Json.Converters.DataSetConverter());
            }
            Response.Write(json);

        }
        

        public void getPicture(string person_id)
        {
            GetFile(person_id, 'P');
        }

        public void getResume(string person_id)
        {
            GetFile(person_id, 'C');
        }

        private void GetFile(string person_id, char type)
        {
            string cmd = "";
            switch (type)
            {
                case 'P':
                    cmd = "research_dir_user.profile_views.get_profile_picture";
                    break;
                case 'C':
                    cmd = "research_dir_user.profile_views.get_profile_cv";
                    break;
                default:
                    Response.Write("Error:type not defined");
                    return;
            }

            using (Oracle.DataAccess.Client.OracleConnection orCN = new OracleConnection(getConnectionString()))
            {
                orCN.Open();
                OracleCommand orCmd = new OracleCommand(cmd, orCN);
                orCmd.CommandType = System.Data.CommandType.StoredProcedure;

                orCmd.Parameters.Add("p_person_id", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                orCmd.Parameters["p_person_id"].Value = person_id;

                orCmd.Parameters.Add("r_cur", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                OracleDataAdapter adapt = new OracleDataAdapter(orCmd);
                System.Data.DataSet orDS = new System.Data.DataSet();

                orCmd.ExecuteNonQuery();
                adapt.Fill(orDS);

                if (orDS.Tables[0].Rows.Count > 0)
                {
                    System.Data.DataRow dr = orDS.Tables[0].Rows[0];
                    byte[] barray = (byte[])dr["file_binary"];
                    Response.ContentType = (String)dr["file_mimetype"];
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + person_id + "." + dr["file_ext"].ToString() + ";");
                    Response.AddHeader("Content-Length", barray.Length.ToString());
                    Response.OutputStream.Write(barray, 0, barray.Length);
                }
                else
                {
                    Response.Write("Error, no image found for person_id '"+person_id+"'");
                }

                orDS.Dispose();
                adapt.Dispose();
                orCmd.Dispose();
                orCN.Close();
                orCN.Dispose();
            }

        }


        public System.Collections.Generic.List<person> GetACResearchName(string term)
        {
            System.Collections.Generic.List<person> ret = new System.Collections.Generic.List<person>();

            using (OracleConnection orCN = new OracleConnection(getConnectionString()))
            {
                orCN.Open();
                OracleCommand orCmd = new OracleCommand("research_dir_user.profile_views.sel_ac_research_person", orCN);
                orCmd.CommandType = System.Data.CommandType.StoredProcedure;

                orCmd.Parameters.Add("p_term", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                orCmd.Parameters["p_term"].Value = term;

                orCmd.Parameters.Add("p_rowCount", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Input;
                orCmd.Parameters["p_rowCount"].Value = 10;

                orCmd.Parameters.Add("r_cur", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                OracleDataAdapter adapt = new OracleDataAdapter(orCmd);
                System.Data.DataSet orDS = new System.Data.DataSet();

                orCmd.ExecuteNonQuery();

                adapt.Fill(orDS);

                foreach (System.Data.DataRow dr in orDS.Tables[0].Rows)
                {
                    person p = new person();
                    p.first_name = dr["first_name"].ToString();
                    p.last_name = dr["last_name"].ToString();
                    p.hscid = dr["hscnet_id"].ToString();
                    p.person_id = dr["person_id"].ToString();
                    p.label = p.last_name + ", " + p.first_name + " (" + p.hscid + ")";
                    p.value = p.person_id;
                    ret.Add(p);
                }

                orDS.Dispose();
                adapt.Dispose();
                orCmd.Dispose();
                orCN.Close();
                orCN.Dispose();

                return ret;
            }
        }
        private void ajaxACResearchName(string term){
            //I have person list, need to output it to jquery.
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(GetACResearchName(term)));
        }

        public void SearchHD(string fname, string lname)
        {
            string ret = "Error";
            string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbprod2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=hd_search_web;Password=flakSchn0z;";
            try
            {
                using (OracleConnection orCN = new OracleConnection(strConnect))
                {
                    orCN.Open(); 
                    OracleCommand orCmd = new OracleCommand("hsc.hd_search.sel_matching_entries", orCN);
                    orCmd.CommandType = System.Data.CommandType.StoredProcedure;

                    orCmd.Parameters.Add("p_first_name", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                    orCmd.Parameters["p_first_name"].Value = fname;

                    orCmd.Parameters.Add("p_last_name", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                    orCmd.Parameters["p_last_name"].Value = lname;

                    orCmd.Parameters.Add("p_unique_id", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                    orCmd.Parameters["p_unique_id"].Value = "";

                    orCmd.Parameters.Add("p_mdm_id", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                    orCmd.Parameters["p_mdm_id"].Value = "";

                    orCmd.Parameters.Add("p_persons", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    orCmd.Parameters.Add("p_roles", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    orCmd.Parameters.Add("p_limit", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Input;
                    orCmd.Parameters["p_limit"].Value = 10;

                    OracleDataAdapter adapt = new OracleDataAdapter(orCmd);
                    System.Data.DataSet orDS = new System.Data.DataSet();

                    orCmd.ExecuteNonQuery();

                    adapt.Fill(orDS);

                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(orDS, new Newtonsoft.Json.Converters.DataSetConverter());
                    orDS.Dispose();
                    adapt.Dispose();
                    orCmd.Dispose();
                    orCN.Close();
                    orCN.Dispose();
                }
            }
            catch (Exception ex)
            {
                ret = "Error: " + ex.Message + "<br/>" + ex.StackTrace;
                ret += ex.InnerException == null ? "" : "<br/>Inner Exception: " + ex.InnerException.Message;
            }
            Response.Write(ret);
        }

        public System.Collections.Generic.List<person> GetACClinicalName(string fname, string lname, string hscid)
        {
           System.Collections.Generic.List<person> ret = new System.Collections.Generic.List<person>();

            using (OracleConnection orCN = new OracleConnection(getConnectionString()))
            {
                orCN.Open();
                OracleCommand orCmd = new OracleCommand("research_dir_user.profile_views.sel_ac_clinical_person", orCN);
                orCmd.CommandType = System.Data.CommandType.StoredProcedure;

                orCmd.Parameters.Add("p_fname", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                orCmd.Parameters["p_fname"].Value = fname;

                orCmd.Parameters.Add("p_lname", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                orCmd.Parameters["p_lname"].Value = lname;

                orCmd.Parameters.Add("p_hscid", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                orCmd.Parameters["p_hscid"].Value = hscid;

                orCmd.Parameters.Add("p_rowCount", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Input;
                orCmd.Parameters["p_rowCount"].Value = 10;

                orCmd.Parameters.Add("r_cur", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;


                OracleDataAdapter adapt = new OracleDataAdapter(orCmd);
                System.Data.DataSet orDS = new System.Data.DataSet();

                orCmd.ExecuteNonQuery();

                adapt.Fill(orDS);

                foreach (System.Data.DataRow dr in orDS.Tables[0].Rows)
                {
                    person p = new person();
                    p.first_name = dr["first_name"].ToString();
                    p.last_name = dr["last_name"].ToString();
                    p.hscid = dr["hscnet_id"].ToString();
                    p.person_id = dr["person_id"].ToString();
                    p.label = p.last_name + ", " + p.first_name + " (" + p.hscid + ")";
                    p.value = p.person_id;
                    ret.Add(p);
                }

                orDS.Dispose();
                adapt.Dispose();
                orCmd.Dispose();
                orCN.Close();
                orCN.Dispose();
            }
            return ret;
        }

        private void ajaxACClinicalName(string fname, string lname, string hscid){
            //I have person list, need to output it to jquery.
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(GetACClinicalName(fname,lname,hscid)));
        }

        public void ListFields(string personID)
        {
            string ret = "Error";
            string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbprod2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=sitecore_web;Password=Garbl3ph!em;";
            try
            {
                using (OracleConnection orCN = new OracleConnection(strConnect))
                {
                    orCN.Open();
                    OracleCommand orCmd = new OracleCommand("research_dir_user.profile_views.sel_research_view", orCN);
                    orCmd.CommandType = CommandType.StoredProcedure;
                    orCmd.Parameters.Add("p_person_id", OracleDbType.Long, (object)personID, ParameterDirection.Input);
                    orCmd.Parameters.Add("r_research", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_progs", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_title_depts", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_publications", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_degrees", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_lectures", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_positions", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_specialty", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_memberships", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_awards", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_grants", OracleDbType.RefCursor, ParameterDirection.Output);
                    orCmd.Parameters.Add("r_patents", OracleDbType.RefCursor, ParameterDirection.Output);

                    OracleDataAdapter adapt = new OracleDataAdapter(orCmd);
                    System.Data.DataSet orDS = new System.Data.DataSet();

                    orCmd.ExecuteNonQuery();

                    adapt.Fill(orDS);

                    ret = Newtonsoft.Json.JsonConvert.SerializeObject(orDS, new Newtonsoft.Json.Converters.DataSetConverter());
                    orDS.Dispose();
                    adapt.Dispose();
                    orCmd.Dispose();
                    orCN.Close();
                    orCN.Dispose();
                }
            }
            catch (Exception ex)
            {
                ret = "Error: " + ex.Message + "<br/>" + ex.StackTrace;
                ret += ex.InnerException == null ? "" : "<br/>Inner Exception: " + ex.InnerException.Message;
            }
            Response.Write(ret);            
        }

        public class person
        {
            public string first_name;
            public string last_name;
            public string person_id;
            public string hscid;
            public string label;
            public string value;
            public person()
            {
                label = value = first_name = last_name = person_id = hscid = "";
            }
        }

        public System.Collections.Generic.List<Faculty> getFacultyForDepartment(decimal deptID)
        {
            // Put user code to initialize the page here

            System.Collections.Generic.List<Faculty> FacultyList = new System.Collections.Generic.List<Faculty>();

            using (OracleConnection oracleCon = new OracleConnection(getConnectionString()))
            {
                oracleCon.Open();

                OracleCommand orCmd = new OracleCommand("research_dir_user.profile_views.sel_faculty_for_dept", oracleCon);
                orCmd.CommandType = System.Data.CommandType.StoredProcedure;
                orCmd.Parameters.Add("p_sid", OracleDbType.Decimal).Direction = ParameterDirection.Input;
                orCmd.Parameters["p_sid"].Value = deptID;
                orCmd.Parameters.Add("r_cur", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                OracleDataAdapter adapt = new OracleDataAdapter(orCmd);

                orCmd.ExecuteNonQuery();

                DataSet oraDR1 = new DataSet();

                adapt.Fill(oraDR1);

                foreach (DataRow dr in oraDR1.Tables[0].Rows)
                {
                    Faculty f = new Faculty();
                    f.person_id = dr["person_id"].ToString().Trim();
                    f.first_name = dr["first_name"].ToString().Trim();
                    f.last_name = dr["last_name"].ToString().Trim();
                    f.dept_name = dr["global_dept_name"].ToString().Trim();
                    f.phone = dr["global_dept_name"].ToString().Trim();
                    f.area_code = dr["area_code"].ToString().Trim();
                    f.email = dr["email"].ToString().Trim();

                    FacultyList.Add(f);
                }

                oraDR1.Dispose();
                adapt.Dispose();
                orCmd.Dispose();

                   oracleCon.Close();
                oracleCon.Dispose();
            }

            /*MyOutput.Text += "<b> There are " + FacultyList.Count + " faculty members</b><br/>";
            foreach (Faculty f in FacultyList)
            {
                MyOutput.Text += "&nbsp; " + f.toString() + "<br/>";
            }*/

            return FacultyList;
        }

        public void ajaxFacultyList(decimal dept_id)
        {
            System.Collections.Generic.List<Faculty> ret = getFacultyForDepartment(dept_id);
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(ret));
        }

        public class Faculty
        {
            public string first_name
            {
                get;
                set;
            }
            public string last_name
            {
                get;
                set;
            }
            public string person_id
            {
                get;
                set;
            }
            public string dept_name
            {
                get;
                set;
            }
            public string email
            {
                get;
                set;
            }
            public string phone
            {
                get;
                set;
            }
            public string area_code
            {
                get;
                set;
            }
            public Faculty()
            {
                first_name = last_name = person_id = dept_name = phone = area_code = email = "";
            }
            public string toString()
            {
                return first_name + " " + last_name + ", " + " (" + person_id + ")" + ", " + dept_name;
            }
        }

        public bool hasPicture(string person_id)
        {

            return hasFile(person_id, 'P');
        }

        public bool hasCV(string person_id)
        {
            return hasFile(person_id, 'C');
        }

        private bool hasFile(string person_id, char type)
        {
            bool ret = false;
            using (Oracle.DataAccess.Client.OracleConnection orCN = new OracleConnection(getConnectionString()))
            {
                orCN.Open();
                OracleCommand orCmd = new OracleCommand("research_dir_user.profile_views.has_files", orCN);
                orCmd.CommandType = System.Data.CommandType.StoredProcedure;

                orCmd.Parameters.Add("p_person_id", OracleDbType.Varchar2).Direction = System.Data.ParameterDirection.Input;
                orCmd.Parameters["p_person_id"].Value = person_id;

                orCmd.Parameters.Add("r_ret", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;

                
                OracleDataAdapter adapt = new OracleDataAdapter(orCmd);
                DataSet orDS = new DataSet();

                orCmd.ExecuteNonQuery();
                adapt.Fill(orDS);


                orDS.Dispose();
                adapt.Dispose();
                orCmd.Dispose();
                orCN.Close();
                orCN.Dispose();
            }
            return ret;
        }

    }

}