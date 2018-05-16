using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using Newtonsoft.Json;

namespace HealthIS.Apps.MVC.Models
{
    public class FacultyDirectoryDoctors
    {
        // Initialize component's rendering info
        public Item item { get; set; }
        public Sitecore.Mvc.Presentation.Rendering rendering { get; set; }
        public Item pageItem { get; set; }
        public bool isDatasourceNull { get; set; }
        public string allFacultyData { get; set; }
        public NameValueCollection getAllFacultyLists { get; set; }

        public string GetProfileLink(Item item)
        {
            string pp = item.Fields["Profile Page"].Value;
            pp = pp.Split('*')[0];
            pp = pp.Replace("/sitecore/content/Home", "");
            return pp;
        }

        public bool hasChildren
        {
            get
            {
                return rendering.Item.HasChildren;
            }
        }

        public HtmlString GetField(Item item, string fieldName, string parm = null)
        {
            return new HtmlString(FieldRenderer.Render(item, fieldName, parm));
        }

        //
        // This method is for addtional information, such as Credential and Email
        //
        public personInfo SearchHD(string fname, string lname, string personIdFromList)
        {
            string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbprod2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=hd_search_web;Password=flakSchn0z;";
            HealthIS.Apps.MVC.Models.personInfo person = new HealthIS.Apps.MVC.Models.personInfo();
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

                    foreach (System.Data.DataRow dr in orDS.Tables[0].Rows)
                    {
                        string personIdFromSearch = dr["PERSON_ID"].ToString();
                        if (personIdFromList == personIdFromSearch)
                        {
                            person.PERSON_ID = dr["PERSON_ID"].ToString();
                            person.FIRST_NAME = dr["FNAME"].ToString();
                            person.LAST_NAME = dr["LNAME"].ToString();
                            person.EMAIL = dr["EMAIL"].ToString();
                            person.HSCNET_ID = dr["HSCNET_ID"].ToString();
                        }
                    }
                    orDS.Dispose();
                    adapt.Dispose();
                    orCmd.Dispose();
                    orCN.Close();
                    orCN.Dispose();
                }
            }
            catch (Exception ex)
            {
                string error = "Error: " + ex.Message + "<br/>" + ex.StackTrace;
                error += ex.InnerException == null ? "" : "<br/>Inner Exception: " + ex.InnerException.Message;
                Sitecore.Diagnostics.Log.Error("Faculty Direcotry Error - 'SearchHD' method: ", error);
            }
            return person;
        }

        //
        // This is main faculty info, but doesn't contain whole person information, such as Credential and Email sometimes
        //
        public string FacultyInfo(string personID)
        {
            string json = personID + " couldn't find";
            try
            {
                if (personID.Contains("Temp_")) { return ""; }
                string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbprod2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=sitecore_web;Password=Garbl3ph!em;";
                using (OracleConnection dbConnection = new OracleConnection(strConnect))
                {
                    dbConnection.Open();
                    OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.sel_research_view", dbConnection);
                    oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    oracleCommand.Parameters.Add("p_person_id", OracleDbType.Long, personID, ParameterDirection.Input);
                    oracleCommand.Parameters.Add("r_research", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_progs", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_title_depts", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_publications", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_degrees", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_lectures", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_positions", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_specialty", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_memberships", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_awards", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_grants", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("r_patents", OracleDbType.RefCursor, ParameterDirection.Output);

                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                    System.Data.DataSet dataSet = new System.Data.DataSet();
                    oracleCommand.ExecuteNonQuery();
                    oracleDataAdapter.Fill(dataSet);

                    json = JsonConvert.SerializeObject(dataSet, new Newtonsoft.Json.Converters.DataSetConverter());
                    dataSet.Dispose();
                    oracleDataAdapter.Dispose();
                    oracleCommand.Dispose();
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                string error = "Error: " + ex.Message + "<br/>" + ex.StackTrace;
                error += ex.InnerException == null ? "" : "<br/>Inner Exception: " + ex.InnerException.Message;
                Sitecore.Diagnostics.Log.Error("Faculty Direcotry Error - 'FacultyInfo' method: ", error);
            }

            return json;
        }

        //
        // Deserialize JSON and load faculty information we need
        //
        public personInfo LoadFacultyInfo(NameValueCollection allFaculties, string personId)
        {
            HealthIS.Apps.MVC.Models.personInfo person = new HealthIS.Apps.MVC.Models.personInfo();
            try
            {
                string jobTitle = allFaculties[personId];
                jobTitle = jobTitle.Replace(";amp;", "&");
                jobTitle = jobTitle.Replace(";eq;", "=");
                jobTitle = jobTitle.Replace(";dump;", ""); // default value

                person.jobTitle = jobTitle;

                if (!personId.ToLower().Contains("temp_") && allFaculties.Count > 0)
                {
                    // Get data from FacultyInfo method
                    System.Data.DataSet dataSet = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataSet>(FacultyInfo(personId));
                    System.Data.DataTable dataTable = dataSet.Tables[0];

                    person.PERSON_ID = personId;
                    foreach (System.Data.DataRow row in dataTable.Rows)
                    {
                        person.FIRST_NAME = row["FIRST_NAME"].ToString();
                        person.LAST_NAME = row["LAST_NAME"].ToString();
                        person.TITLE = row["TITLE"].ToString();
                        person.EMAIL = row["EMAIL"].ToString();
                        var hscId = row["EMAIL"].ToString().Split('@');
                        person.HSCNET_ID = hscId[0].ToString();
                    }

                    // If FacultyInfo method doesn't have enough information, call SearchHD method to add more information
                    if (String.IsNullOrEmpty(person.EMAIL) && !String.IsNullOrEmpty(personId))
                    {
                        personInfo searchedData = SearchHD(person.FIRST_NAME, person.LAST_NAME, personId);
                        person.FIRST_NAME = searchedData.FIRST_NAME;
                        person.LAST_NAME = searchedData.LAST_NAME;
                        person.EMAIL = searchedData.EMAIL;
                        person.HSCNET_ID = searchedData.HSCNET_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "Error: " + ex.Message + "<br/>" + ex.StackTrace;
                error += ex.InnerException == null ? "" : "<br/>Inner Exception: " + ex.InnerException.Message;
                Sitecore.Diagnostics.Log.Error("Faculty Direcotry Error - 'LoadFacultyInfo' method: ", error);
            }
            return person;
        }
    }

    public class personInfo
    {
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string TITLE { get; set; }
        public string jobTitle { get; set; }
        public string PERSON_ID { get; set; }
        public string HSCNET_ID { get; set; }
        public string BUILDING { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
    }
}