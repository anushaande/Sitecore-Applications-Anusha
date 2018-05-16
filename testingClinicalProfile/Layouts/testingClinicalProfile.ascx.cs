using System;
using ClinicalProfile;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;


namespace Layouts.Testingclinicalprofile {
  
	/// <summary>
	/// Summary description for TestingclinicalprofileSublayout
	/// </summary>
  public partial class TestingclinicalprofileSublayout : System.Web.UI.UserControl 
	{
    private void Page_Load(object sender, EventArgs e) {
      // Put user code to initialize the page here
        int pid = 0;
        string fname = "";

        string conStr = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=hscdb4.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBPREP)(SERVER=DEDICATED)));User Id=authorized_request_user;Password=schmorgle;";
        if (ClinicalProfile.Profile.isOnProduction())
        {
            conStr = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=hscdb4.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=authorized_request_user;Password=schmorgle;";
        }

        using (Oracle.DataAccess.Client.OracleConnection cn = new OracleConnection(conStr))
        {
            cn.Open();
            OracleCommand orCmd = new OracleCommand("select person_id from hsc.health_person where active = 'Y' and npi is not null and rownum=1", cn);
            pid = Int32.Parse((string)orCmd.ExecuteScalar());

            if (pid > 0)
            {
                orCmd.Dispose();
                orCmd = new OracleCommand("select NVL(pref_fname,first_name) from hsc.health_person where person_id = " + pid, cn);
                fname = (string)orCmd.ExecuteScalar();
            }

            orCmd.Dispose();
            cn.Close();
            cn.Dispose();
        }

        if (pid < 1)
        {
            myOutput.Text = "Error: unable to get person_id from health_Person";
            return;
        }

        Profile clinicalProf = new Profile(pid, ClinicalProfile.Profile.ProfileParts.Demographics);
        //unit testing
        if (fname != clinicalProf.firstName)
        {
            myOutput.Text = "First name does not match health_person";
        }
        else
        {
            myOutput.Text = "Profile test passed.";
        }
    }
  }
}