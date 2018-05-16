using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Oracle.DataAccess.Client;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;



namespace HealthIS.Apps.MVC.Models
{
    public class FacultyDirectoryNursing : IRenderingModel
    {
        // Initialize component's rendering info
        public Item item { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item PageItem { get; set; }
        public bool isDatasourceNull { get; set; }
        public System.Collections.Generic.List<NursingPerson> StaffList;
        public string errorMessage = string.Empty;
        public Sitecore.Data.Fields.CheckboxField chX20 { get; set; }
        public Sitecore.Data.Templates.TemplateField templateField { get; set; }
        public string getTemplateID { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            item = rendering.Item;
            PageItem = PageContext.Current.Item;
            Rendering = rendering;
            isDatasourceNull = string.IsNullOrEmpty(rendering.DataSource);
            if (!isDatasourceNull)            
            {
                chX20 = item.Fields["X20 Directory Listing"];
                templateField = item.Fields["X20 Directory Listing"].GetTemplateField();
                getTemplateID = templateField.ID.Guid.ToString().ToUpper();
            }
        }
        
        public void LoadFacultyInfo()
        {
            try
            {
                string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbprod2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=sitecore_web;Password=Garbl3ph!em;";
                
                StaffList = new System.Collections.Generic.List<NursingPerson>();

                using (OracleConnection dbConnection = new OracleConnection(strConnect))
                {
                    dbConnection.Open();
                    OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.sel_nurse_personnel", dbConnection);
                    oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    oracleCommand.Parameters.Add("r_cur", OracleDbType.RefCursor, ParameterDirection.Output);

                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                    System.Data.DataSet dataSet = new System.Data.DataSet();
                    oracleCommand.ExecuteNonQuery();
                    oracleDataAdapter.Fill(dataSet);

                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        NursingPerson person = new NursingPerson();            
                        person.PERSON_ID = dr["person_id"].ToString().Trim();
                        person.FIRST_NAME = dr["first_name"].ToString().Trim();
                        person.LAST_NAME = dr["last_name"].ToString().Trim();
                        person.ROLE = dr["role"].ToString().Trim();
                        person.CREDENTIALS = dr["credentials"].ToString().Trim();
                        person.EMAIL = dr["email"].ToString().Trim();
                        person.PHONE = dr["phone"].ToString().Trim();
                        person.PRIMARY_POSITION = dr["primary_position"].ToString().Trim();
                        person.BUILDING = dr["building"].ToString().Trim();
                        person.ROOM = dr["room"].ToString().Trim();
                        StaffList.Add(person);
                    }

                    // Remove duplicate data in case of the person have both staff and faculty status
                    StaffList = StaffList.GroupBy(p => p.PERSON_ID).Select(s => s.FirstOrDefault()).ToList();

                    dataSet.Dispose();
                    oracleDataAdapter.Dispose();
                    oracleCommand.Dispose();
                    dbConnection.Close();
                    dbConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Error: " + ex.Message + "<br/>" + ex.StackTrace;
                errorMessage += ex.InnerException == null ? "" : "<br/>Inner Exception: " + ex.InnerException.Message;
                Sitecore.Diagnostics.Log.Error("Faculty Direcotry Error - 'LoadFacultyInfo' method: ", errorMessage);
            }
        }
    }

    public class NursingPerson {
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string PERSON_ID { get; set; }
        public string ROLE { get; set; }
        public string CREDENTIALS { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }      
        public string PRIMARY_POSITION { get; set; }
        public string BUILDING { get; set; }
        public string ROOM { get; set; }
    }
}