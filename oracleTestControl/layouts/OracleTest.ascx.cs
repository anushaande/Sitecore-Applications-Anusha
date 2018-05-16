using System;
//using System.Collections;
//using System.Configuration;
//using System.Data;
//using System.Web;
//using System.Web.Security;
using Oracle.DataAccess.Types;
using Oracle.DataAccess.Client;
using Sitecore.Oracle.Data.Connections;


namespace HealthIS.Apps.Debug
{
    

	public partial class OracleTest : System.Web.UI.UserControl
	{
        

		protected void Page_Load(object sender, EventArgs e)
		{
            myTestResults.Text = "Oracle Test<br/>";

            string strServer = "DBPrep";
            if (HealthIS.Apps.Util.isOnProduction())
            {
                strServer = "DBFrzn";
            }

            mySystemPath.Text = strServer;
            
            OracleConnection oraCN = HealthIS.Apps.Util.getDBConnection();
            
            OracleCommand oraCmd = new OracleCommand("select count(*) from hsc.health_person", oraCN);
            //OracleCommand oraCmd = new OracleCommand("select sysdate from dual", oraCN);

            try
            {
                oraCN.Open();
                myTestResults.Text += "the count is " + oraCmd.ExecuteScalar();
            }
            catch (OracleException oex)
            {
                myTestResults.Text += "There was an Error!!! " + oex.ErrorCode+" : " + oex.Message + "<br/>" + oex.StackTrace;
            }
            catch (Exception ex)
            {
                myTestResults.Text += "There was an Error!!! " + ex.Message + "<br/>" + ex.StackTrace;
            }

            oraCmd.Dispose();
            oraCN.Close();
            

            myTestResults.Text += "<br/>";
            /*string tmp = "";
            object Test = Microsoft.Win32.Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\ORACLE\\ODP.NET\\2.112.3.0", "DllPath", "ShitCracker");
            if (Test == null)
            {
                tmp = "Was Null";
            }
            else
            {
                tmp = "Was " + Test.ToString();
            }*/

            //RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\VisualStudio\\10.0");

            //if (regKey != null)
            //{
            //    object val = regKey.GetValue("FullScreen");
            //}


            
        }
	}
}