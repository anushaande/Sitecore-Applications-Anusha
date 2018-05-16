using System;
using ClinicalProfile;
using HealthIS.Apps;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Collections.Generic;
using System.Data;

namespace HealthIS.Apps
{
    public partial class FacultyDirectory 
    {
        public static void getPicture(string person_id, System.Web.HttpResponse Response)
        {
            GetFile(person_id, 'P', Response);
        }

        public static void getResume(string person_id, System.Web.HttpResponse Response)
        {
            GetFile(person_id, 'C', Response);
        }

        private static void GetFile(string person_id, char type, System.Web.HttpResponse Response)
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

            using (Oracle.DataAccess.Client.OracleConnection orCN = HealthIS.Apps.Util.getDBConnection())
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
                    Response.Write("Error, no file found for person_id '" + person_id + "'");
                }

                orDS.Dispose();
                adapt.Dispose();
                orCmd.Dispose();
                orCN.Close();
                orCN.Dispose();
            }

        }

        public static System.Collections.Generic.List<person> GetACResearchName(string term)
        {
            System.Collections.Generic.List<person> ret = new System.Collections.Generic.List<person>();

            using (OracleConnection orCN = HealthIS.Apps.Util.getDBConnection())
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

        public static System.Collections.Generic.List<person> GetACClinicalName(string fname, string lname, string hscid)
        {
            System.Collections.Generic.List<person> ret = new System.Collections.Generic.List<person>();

            using (OracleConnection orCN = HealthIS.Apps.Util.getDBConnection())
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

        public static System.Collections.Generic.List<Faculty> getFacultyForDepartment(decimal deptID)
        {
            // Put user code to initialize the page here

            System.Collections.Generic.List<Faculty> FacultyList = new System.Collections.Generic.List<Faculty>();

            using (OracleConnection oracleCon = HealthIS.Apps.Util.getDBConnection())
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

                    FacultyList.Add(f);
                }

                oraDR1.Dispose();
                adapt.Dispose();
                orCmd.Dispose();

                oracleCon.Close();
                oracleCon.Dispose();
            }
            return FacultyList;
        }

        public class Faculty
        {
            public string first_name;
            public string last_name;
            public string person_id;
            public string dept_name;
            public Faculty()
            {
                first_name = "";
                last_name = "";
                person_id = "";
                dept_name = "";
            }
            public string toString()
            {
                return first_name + " " + last_name + ", " + " (" + person_id + ")" + ", " + dept_name;
            }
        }



    }
}