using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Oracle.DataAccess.Client;
using System.Data.Common;
using System.Data;

namespace HealthIS.Apps.MVC.FacultyDirectory
{
    public class FacultyDirectoryList : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool dsSet = false;
        public List<FacultyDirectoryListSection> Sections = new List<FacultyDirectoryListSection>();
        public Item Root { get; set; }
        public String Errors = "";

        public HealthIS.Apps.FacultyDirectory.Profile person;
        public bool dsIsSet { get; set; }

        public override void Initialize(Rendering rendering)
        {
            try
            {
                base.Initialize(rendering);
                //Set model specific objects
                Rendering = rendering;
                dsSet = !String.IsNullOrEmpty(rendering.DataSource);
                if (dsSet)
                {
                    Root = Sitecore.Context.Database.GetItem(rendering.DataSource);
                    if (Root != null)
                    {
                        foreach (Item s in Root.Children)
                        {
                            string sectionTitle = Helpers.getStrField(s, "Section Title");
                            FacultyDirectoryListSection section = new FacultyDirectoryListSection(s);
                            foreach (Item i in s.Children)
                            {
                                section.Listings.Add(new FacultyDirectoryListItem(i));
                            }
                            Sections.Add(section);
                        }
                    }
                    else
                    {
                        Errors += "Root item not found.<br/>";
                    }
                }
            }
            catch (Exception ex)
            {
                Errors += ex.Message + "<br/>" + ex.InnerException + "<br/>" + ex.StackTrace;
            }
        }

        public string doTest(string personID, FacultyDirectoryListItem item)
        {
            string json = "Nothing!!";
            string title = "";
            using (OracleConnection dbConnection = new OracleConnection(FacultyDirectoryListItem.getConnectionString()))
            {
                string str = "research_dir_user.profile_views.sel_research_view";
                OracleCommand oracleCommand = new OracleCommand();
                oracleCommand.Connection = dbConnection;
                ((DbCommand)oracleCommand).CommandText = str;
                ((DbCommand)oracleCommand).CommandType = CommandType.StoredProcedure;
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
                ((DbConnection)dbConnection).Open();
                ((DbCommand)oracleCommand).ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                ((DataAdapter)oracleDataAdapter).Fill(dataSet);
                //json = "success!"; 
                json = Newtonsoft.Json.JsonConvert.SerializeObject(dataSet, new Newtonsoft.Json.Converters.DataSetConverter());

                foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    
                    title = dataRow["title"].ToString();
                    
                    //json += "Building = " + dataRow["bldg"].ToString();
                    //json += ", last_login = " + dataRow["last_login"].ToString();
                    //json += ", email = " + dataRow["email"].ToString();
                    //json += ", update_date = " + dataRow["update_date"].ToString();
                }

                //var table = dataSet.Tables[0];
                //if (table.Rows.Count > 0)
                //{
                //    json = dataSet.Tables[0].Rows[0][fieldName].ToString();
                //}

                //json = dataSet.Tables[0].Rows[0]["TITLE"].ToString();

                //item.SCItem.Editing.BeginEdit();
                //using (new EditContext(item.SCItem))
                //{
                //    item.SCItem.Fields["Credentials"].Value = title;
                //}
                //item.SCItem.Editing.EndEdit();
            }

            return "";
        }

        public string doTest2(string personID)
        {
            string json = "error";
            using (OracleConnection conn = Util.getDBConnection())
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
            return json;
        }

        public string listFields(string personID, string fieldName)
        {
            string json = "";
            
            //using (OracleConnection dbConnection = Util.getDBConnection())
            using (OracleConnection dbConnection = new OracleConnection(FacultyDirectoryListItem.getConnectionString()))
            {
                string str = "research_dir_user.profile_views.sel_research_view";
                OracleCommand oracleCommand = new OracleCommand();
                oracleCommand.Connection = dbConnection;
                ((DbCommand)oracleCommand).CommandText = str;
                ((DbCommand)oracleCommand).CommandType = CommandType.StoredProcedure;
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
                ((DbConnection)dbConnection).Open();
                ((DbCommand)oracleCommand).ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                ((DataAdapter)oracleDataAdapter).Fill(dataSet);
                //json = "success!"; 
                //json = Newtonsoft.Json.JsonConvert.SerializeObject(dataSet, new Newtonsoft.Json.Converters.DataSetConverter());

                var table = dataSet.Tables[0];
                if (table.Rows.Count > 0)
                {
                    json = table.ToString();
                    //json = dataSet.Tables[0].Rows[0][fieldName].ToString();
                }
            }

            return json;
        }
    }

    public class FacultyDirectoryListItem
    {
        public Item SCItem { get; set; }
        public int PersonID { get; set; }
        public string JobTitle { get; set; }
        public string Credentials { get; set; }
        public string HSCID { get; set; }
        public string JSON { get; set; }
        public HealthIS.Apps.FacultyDirectory.Profile Profile { get; set; }
        public person Person { get; set; }
        public string ProfilePage = "";

        public FacultyDirectoryListItem(Item scItem)
        {
            SCItem = scItem;
            PersonID = Helpers.getIntField(scItem, "Person ID");
            JSON = "";
            JobTitle = Helpers.getStrField(scItem, "Job Title");
            Credentials = Helpers.getStrField(scItem, "Credentials");            
            
            HSCID = Helpers.getStrField(scItem, "HSCID");
            if (PersonID != -1) {
                foreach (person p in GetACClinicalName("", "", HSCID))
                {
                    if (p.person_id == PersonID.ToString())
                    {
                        Person = p;
                        break;
                    }
                }
                try
                {
                    Profile = new HealthIS.Apps.FacultyDirectory.Profile(PersonID);
                }
                catch (Exception ex) { }
            }
        }

        public static string getConnectionString()
        {
            //string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbdev2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBPREP)(SERVER=DEDICATED)));User Id=profile_views_web;Password=nordoga;";
            //if (Util.isOnProduction())
            //{//hscdb4
            //string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbprod2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=profile_views_web;Password=nordoga;";
            string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbprod2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=sitecore_web;Password=Garbl3ph!em;";
            //}
            return strConnect;
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
                    FacultyDirectoryList l = new FacultyDirectoryList();

                    p.first_name = dr["first_name"].ToString();
                    p.last_name = dr["last_name"].ToString();
                    p.hscid = dr["hscnet_id"].ToString();
                    p.person_id = dr["person_id"].ToString();
                    //credentials = dr["title"].ToString();
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
    }

    public class person
    {
        public string first_name;
        public string last_name;
        //public string credentials;
        public string person_id;
        public string hscid;
        public string label;
        public string value;
        public person()
        {
            label = value = first_name = last_name = person_id = hscid = "";
        }
    }

    public class FacultyDirectoryListSection
    {
        public string SectionTitle { get; set; }
        public Item SCItem { get; set; }
        public List<FacultyDirectoryListItem> Listings { get; set; }
        public string ProfilePage { get; set; }
        public Sitecore.Data.Fields.CheckboxField chEnableProfileImages { get; set; }
        public bool gridView { get; set; }
        public FacultyDirectoryListSection(Item scItem)        
        {
            SCItem = scItem;
            SectionTitle = Helpers.getStrField(scItem, "Section title");
            Listings = new List<FacultyDirectoryListItem>();
            string pp = scItem.Fields["Profile Page"].Value;
            pp = pp.Split('*')[0];
            pp = pp.Replace("/sitecore/content/Home", "");
            ProfilePage = pp;
            chEnableProfileImages = Helpers.getCBField(scItem, "Enable Profile Images");
            gridView = false;
        }

    }
}