using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Threading;

namespace HealthIS.Apps
{
    public class FacultyDirectory
    {
        public static int hasFiles(string person_id)
        {
            int num = 0;
            using (OracleConnection dbConnection = HealthIS.Apps.Util.getDBConnection())
            {
                dbConnection.Open();
                OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.has_file", dbConnection);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("p_person_id", OracleDbType.Varchar2, person_id, ParameterDirection.Input);
                oracleCommand.Parameters.Add("p_ret", OracleDbType.Decimal, ParameterDirection.Output);
                oracleCommand.ExecuteNonQuery();
                num = (int)oracleCommand.Parameters["p_ret"].Value;
                oracleCommand.Dispose();
                dbConnection.Close();
                dbConnection.Dispose();
            }
            return num;
        }

        public static FacultyDirectory.profileFile getPicture(string person_id)
        {
            return FacultyDirectory.GetFile(person_id, 'P');
        }

        public static FacultyDirectory.profileFile getResume(string person_id)
        {
            return FacultyDirectory.GetFile(person_id, 'C');
        }

        private static FacultyDirectory.profileFile GetFile(string person_id, char type)
        {
            FacultyDirectory.profileFile profileFile = new FacultyDirectory.profileFile();
            string str;
            switch (type)
            {
                case 'C':
                    str = "research_dir_user.profile_views.get_profile_cv";
                    break;
                case 'P':
                    str = "research_dir_user.profile_views.get_profile_picture";
                    break;
                default:
                    profileFile.name = "Error:type not defined";
                    return profileFile;
            }
            using (OracleConnection dbConnection = Util.getDBConnection())
            {
                dbConnection.Open();
                OracleCommand oracleCommand = new OracleCommand(str, dbConnection);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("p_person_id", OracleDbType.Varchar2, person_id, ParameterDirection.Input);
                oracleCommand.Parameters.Add("r_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                DataSet dataSet = new DataSet();
                oracleCommand.ExecuteNonQuery();
                oracleDataAdapter.Fill(dataSet);
                if ((int)type == 80 && dataSet.Tables[0].Rows.Count < 1)
                {
                    dataSet.Dispose();
                    oracleDataAdapter.Dispose();
                    oracleCommand.Dispose();
                    oracleCommand = new OracleCommand("research_dir_user.profile_views.get_profile_picture", dbConnection);
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add("p_person_id", OracleDbType.Varchar2, "-1", ParameterDirection.Input);
                    oracleCommand.Parameters.Add("r_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                    dataSet = new DataSet();
                    oracleCommand.ExecuteNonQuery();
                    oracleDataAdapter.Fill(dataSet);
                }
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow dataRow = dataSet.Tables[0].Rows[0];
                    profileFile.person_id = dataRow["person_id"].ToString();
                    profileFile.fileContent = (byte[])dataRow["file_binary"];
                    profileFile.extension = dataRow["file_ext"].ToString();
                    profileFile.mimeType = (string)dataRow["file_mimetype"];
                    profileFile.name = profileFile.person_id + "." + profileFile.extension;
                }
                dataSet.Dispose();
                oracleDataAdapter.Dispose();
                oracleCommand.Dispose();
                dbConnection.Dispose();
                dbConnection.Dispose();
            }
            return profileFile;
        }

        public static List<FacultyDirectory.person> GetACResearchName(string term)
        {
            List<FacultyDirectory.person> list = new List<FacultyDirectory.person>();
            using (OracleConnection dbConnection = Util.getDBConnection())
            {
                dbConnection.Open();
                OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.sel_ac_research_person", dbConnection);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("p_term", OracleDbType.Varchar2, term, ParameterDirection.Input);
                oracleCommand.Parameters.Add("p_rowCount", OracleDbType.Int16, 10, ParameterDirection.Input);
                oracleCommand.Parameters.Add("r_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                DataSet dataSet = new DataSet();
                oracleCommand.ExecuteNonQuery();
                oracleDataAdapter.Fill(dataSet);
                foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    FacultyDirectory.person person = new FacultyDirectory.person();
                    person.first_name = dataRow["first_name"].ToString();
                    person.last_name = dataRow["last_name"].ToString();
                    person.hscid = dataRow["hscnet_id"].ToString();
                    person.person_id = dataRow["person_id"].ToString();
                    person.label = person.last_name + ", " + person.first_name + " (" + person.hscid + ")";
                    person.value = person.person_id;
                    list.Add(person);
                }
                dataSet.Dispose();
                oracleDataAdapter.Dispose();
                oracleCommand.Dispose();
                dbConnection.Close();
                dbConnection.Dispose();
                return list;
            }
        }

        public static List<FacultyDirectory.person> GetACClinicalName(string fname, string lname, string hscid)
        {
            List<FacultyDirectory.person> list = new List<FacultyDirectory.person>();
            using (OracleConnection dbConnection = Util.getDBConnection())
            {
                dbConnection.Open();
                OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.sel_ac_clinical_person", dbConnection);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("p_fname", OracleDbType.Varchar2, fname, ParameterDirection.Input);
                oracleCommand.Parameters.Add("p_lname", OracleDbType.Varchar2, lname, ParameterDirection.Input);
                oracleCommand.Parameters.Add("p_hscid", OracleDbType.Varchar2, hscid, ParameterDirection.Input);
                oracleCommand.Parameters.Add("p_rowCount", OracleDbType.Int16, 10, ParameterDirection.Input);
                oracleCommand.Parameters.Add("r_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                DataSet dataSet = new DataSet();
                ((DbCommand)oracleCommand).ExecuteNonQuery();
                ((DataAdapter)oracleDataAdapter).Fill(dataSet);
                foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    FacultyDirectory.person person = new FacultyDirectory.person();
                    person.first_name = dataRow["first_name"].ToString();
                    person.last_name = dataRow["last_name"].ToString();
                    person.hscid = dataRow["hscnet_id"].ToString();
                    person.person_id = dataRow["person_id"].ToString();
                    person.label = person.last_name + ", " + person.first_name + " (" + person.hscid + ")";
                    person.value = person.person_id;
                    list.Add(person);
                }
                dataSet.Dispose();
                ((Component)oracleDataAdapter).Dispose();
                ((Component)oracleCommand).Dispose();
                ((DbConnection)dbConnection).Close();
                ((Component)dbConnection).Dispose();
            }
            return list;
        }

        public static List<FacultyDirectory.Faculty> getFacultyForDepartment(Decimal deptID)
        {
            List<FacultyDirectory.Faculty> list = new List<FacultyDirectory.Faculty>();
            using (OracleConnection dbConnection = Util.getDBConnection())
            {
                ((DbConnection)dbConnection).Open();
                OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.sel_faculty_for_dept", dbConnection);
                ((DbCommand)oracleCommand).CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("p_sid", OracleDbType.Decimal, deptID, ParameterDirection.Input);
                oracleCommand.Parameters.Add("r_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                ((DbCommand)oracleCommand).ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                ((DataAdapter)oracleDataAdapter).Fill(dataSet);
                foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    list.Add(new FacultyDirectory.Faculty()
                    {
                        person_id = dataRow["person_id"].ToString().Trim(),
                        first_name = dataRow["first_name"].ToString().Trim(),
                        last_name = dataRow["last_name"].ToString().Trim(),
                        dept_name = dataRow["global_dept_name"].ToString().Trim()
                    });
                dataSet.Dispose();
                ((Component)oracleDataAdapter).Dispose();
                ((Component)oracleCommand).Dispose();
                ((DbConnection)dbConnection).Close();
                ((Component)dbConnection).Dispose();
            }
            return list;
        }

        [Flags]
        public enum ProfileFilesAvailable
        {
            None = 0,
            Picture = 1,
            CV = 2,
            All = CV | Picture,
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
                this.label = this.value = this.first_name = this.last_name = this.person_id = this.hscid = "";
            }
        }

        public class Faculty
        {
            public string first_name;
            public string last_name;
            public string person_id;
            public string dept_name;

            public Faculty()
            {
                this.first_name = "";
                this.last_name = "";
                this.person_id = "";
                this.dept_name = "";
            }

            public string toString()
            {
                return this.first_name + " " + this.last_name + ",  (" + this.person_id + "), " + this.dept_name;
            }
        }

        public class Profile
        {
            public string firstName;
            public string lastName;
            public string middleName;
            public string title;
            public string primaryAppointment;
            public string imageSrc;
            public string address;
            public string academicPhone;
            public string phone;
            public string fax;
            public string outsideInterest;
            public string degrees;
            public string personalStatement;
            public string position;
            public string department;
            public string error;
            public string noFormatPhone;
            public string noFormatfax;
            public string npi;
            public string locations_url;
            public string insurances_url;
            public string health_care_phil;
            public string[] specialties;
            public string[] languages;
            public string[] publications;
            public string[] appointments;
            public string[] addresses;
            public string[] clinicalInterests;
            public string[] researchInterests;
            public string[] otherPositions;
            public string[] presentPositions;
            public bool hasClinicalInterests;
            public bool hasInterests;
            public bool hasPublications;
            public bool hasLectures;
            public bool hasGrants;
            public bool hasLinks;
            public bool hasOtherPositions;
            public bool isAcceptingNewPatients;
            public FacultyDirectory.position[] myOtherPositions;
            public FacultyDirectory.position[] myPresentPositions;
            public FacultyDirectory.degree[] myDegrees;
            public FacultyDirectory.publication[] myPublications;
            public FacultyDirectory.clinicalInterest myInterests;
            public FacultyDirectory.address[] myAddresses;
            public FacultyDirectory.award[] myAwards;
            public FacultyDirectory.membership[] myMemberships;
            public FacultyDirectory.insurance[] myInsurances;
            public FacultyDirectory.specialty[] mySpecialties;
            public FacultyDirectory.insurance[] myTopInsurances;
            public string email;
            public string researchid;
            public string philosophy;
            public string research_summary;
            public string personalLink;
            public string labLink;
            public string building;
            public string room;
            public string cvSrc;
            public string academic_philosophy;
            public string[] grants;
            public string[] awards;
            public string[] lectures;
            public string[] memberships;
            public string[] signatureProgram;
            public string[] patents;
            public bool hasAwards;
            public bool hasPatents;
            public FacultyDirectory.patent[] myPatents;
            public FacultyDirectory.appointment[] myAppointments;
            public FacultyDirectory.lecture[] myLectures;
            public FacultyDirectory.grant[] myGrants;

            public Profile(int PersonID)
            {
                this.init(PersonID, FacultyDirectory.Profile.ProfileType.Clinical, FacultyDirectory.Profile.ProfileParts.All);
            }

            public Profile(int PersonID, FacultyDirectory.Profile.ProfileParts pts)
            {
                this.init(PersonID, FacultyDirectory.Profile.ProfileType.Clinical, pts);
            }

            public Profile(int PersonID, FacultyDirectory.Profile.ProfileType typ, FacultyDirectory.Profile.ProfileParts pts)
            {
                this.init(PersonID, typ, pts);
            }

            public void init(int PersonID, FacultyDirectory.Profile.ProfileType typ, FacultyDirectory.Profile.ProfileParts pts)
            {
                switch (typ)
                {
                    case FacultyDirectory.Profile.ProfileType.Research:
                        this.initResearch(PersonID, pts);
                        break;
                    case FacultyDirectory.Profile.ProfileType.Clinical:
                        this.initClinical(PersonID, pts);
                        break;
                    default:
                        this.initClinical(0, FacultyDirectory.Profile.ProfileParts.None);
                        break;
                }
            }

            public void initResearch(int personID, FacultyDirectory.Profile.ProfileParts pts)
            {
                using (OracleConnection dbConnection = Util.getDBConnection())
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
                    if (dataSet.Tables[0].Rows.Count == 0)
                    {
                        this.error = "Could not find the person ID";
                    }
                    else
                    {
                        this.myAddresses = new FacultyDirectory.address[1];
                        foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                        {
                            FacultyDirectory.address address = new FacultyDirectory.address();
                            this.firstName = dataRow["first_name"].ToString().Trim();
                            this.lastName = dataRow["last_name"].ToString().Trim();
                            this.title = dataRow["title"].ToString();
                            this.address = dataRow["addressline1"].ToString().Trim() + dataRow["addressline2"].ToString().Trim() + "<br />" + dataRow["city"].ToString() + ", " + dataRow["state"].ToString() + " " + dataRow["zipcode"].ToString();
                            address.street = this.stringOrEmptyForNull(dataRow["addressline1"].ToString().Trim());
                            address.street2 = this.stringOrEmptyForNull(dataRow["addressline2"].ToString().Trim());
                            address.city = this.stringOrEmptyForNull(dataRow["city"].ToString());
                            address.state = this.stringOrEmptyForNull(dataRow["state"].ToString());
                            address.zip = this.stringOrEmptyForNull(dataRow["zipcode"].ToString());
                            address.url = "http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=%20" + address.street + "," + address.city + "," + address.state + "&sll=37.0625,-95.677068&sspn=47.838189,68.291016&ie=UTF8&t=h&z=17";
                            this.myAddresses[0] = address;
                            this.phone = dataRow["phone"].ToString();
                            this.personalLink = dataRow["personal_link"].ToString();
                            this.email = dataRow["email"].ToString();
                            this.researchid = dataRow["researchid"].ToString();
                            this.labLink = dataRow["lab_link"].ToString();
                            this.building = dataRow["bldg"].ToString();
                            this.room = dataRow["room"].ToString();
                            this.fax = "";
                            this.imageSrc = dataRow["img_url"].ToString();
                            this.cvSrc = dataRow["cv_url"].ToString();
                            this.research_summary = dataRow["research_summary"].ToString().Trim();
                            this.academic_philosophy = dataRow["academic_phil"].ToString().Trim();
                        }
                        this.addresses = new string[0];
                        this.clinicalInterests = new string[0];
                        this.philosophy = "";
                        this.researchInterests = this.GetResearchInterests(dataSet.Tables[0].Rows);
                        this.signatureProgram = this.GetSignProgram(dataSet.Tables[1].Rows);
                        this.appointments = this.GetAppointments(dataSet.Tables[2].Rows);
                        this.publications = this.GetPublications(dataSet.Tables[3].Rows);
                        this.degrees = this.GetDegrees(dataSet.Tables[4].Rows);
                        this.lectures = this.GetLectures(dataSet.Tables[5].Rows);
                        this.otherPositions = this.GetOtherPositions(dataSet.Tables[6].Rows);
                        this.presentPositions = this.GetPresentPositions(dataSet.Tables[6].Rows);
                        this.specialties = this.GetSpecialties(dataSet.Tables[7].Rows);
                        this.memberships = this.GetMemberships(dataSet.Tables[8].Rows);
                        this.awards = this.GetAwards(dataSet.Tables[9].Rows);
                        this.grants = this.GetGrants(dataSet.Tables[10].Rows);
                        this.patents = this.GetPatents(dataSet.Tables[11].Rows);
                        this.myAppointments = this.GetMyAppointments(dataSet.Tables[2].Rows);
                        this.myPublications = this.GetMyPublications(dataSet.Tables[3].Rows);
                        this.myDegrees = this.GetMyDegrees(dataSet.Tables[4].Rows);
                        this.myLectures = this.GetMyLectures(dataSet.Tables[5].Rows);
                        this.myOtherPositions = this.GetMyOtherPositions(dataSet.Tables[6].Rows);
                        this.myPresentPositions = this.GetMyPresentPositions(dataSet.Tables[6].Rows);
                        this.myMemberships = this.GetMyMemberships(dataSet.Tables[8].Rows);
                        this.myAwards = this.GetMyAwards(dataSet.Tables[9].Rows);
                        this.myGrants = this.GetMyGrants(dataSet.Tables[10].Rows);
                        this.myPatents = this.GetMyPatents(dataSet.Tables[11].Rows);
                        this.hasOtherPositions = this.otherPositions.Length > 0;
                        this.hasInterests = this.researchInterests.Length > 0;
                        this.hasPublications = this.publications.Length > 0;
                        this.hasLectures = this.lectures.Length > 0;
                        this.hasGrants = this.grants.Length > 0;
                        this.hasAwards = this.awards.Length > 0;
                        this.hasLinks = this.personalLink.Length > 0 || this.labLink.Length > 0;
                    }
                    dataSet.Dispose();
                    ((Component)oracleDataAdapter).Dispose();
                    ((Component)oracleCommand).Dispose();
                    ((DbConnection)dbConnection).Close();
                    ((Component)dbConnection).Dispose();
                }
            }

            public void initClinical(int personID, FacultyDirectory.Profile.ProfileParts pts)
            {
                this.firstName = this.lastName = this.middleName = this.title = this.primaryAppointment = this.imageSrc = this.address = this.academicPhone = this.phone = this.fax = this.outsideInterest = this.degrees = this.personalStatement = this.position = this.department = this.error = this.noFormatPhone = this.noFormatfax = this.npi = this.locations_url = this.insurances_url = this.health_care_phil = this.philosophy = "";
                this.hasClinicalInterests = this.hasInterests = this.hasPublications = this.hasLectures = this.hasGrants = this.hasLinks = this.hasOtherPositions = this.isAcceptingNewPatients = false;
                this.specialties = this.languages = this.publications = this.appointments = this.addresses = this.clinicalInterests = this.researchInterests = this.otherPositions = this.presentPositions = new string[0];
                this.myOtherPositions = this.myPresentPositions = new FacultyDirectory.position[0];
                this.myDegrees = new FacultyDirectory.degree[0];
                this.myPublications = new FacultyDirectory.publication[0];
                this.myInterests = new FacultyDirectory.clinicalInterest();
                this.myAddresses = new FacultyDirectory.address[0];
                this.myAwards = new FacultyDirectory.award[0];
                this.myMemberships = new FacultyDirectory.membership[0];
                this.myInsurances = new FacultyDirectory.insurance[0];
                this.mySpecialties = new FacultyDirectory.specialty[0];
                this.myTopInsurances = new FacultyDirectory.insurance[0];
                if (pts == FacultyDirectory.Profile.ProfileParts.None)
                    return;
                string str = "research_dir_user.profile_views.sel_clinical_view";
                using (OracleConnection dbConnection = Util.getDBConnection())
                {
                    OracleCommand oracleCommand = new OracleCommand();
                    oracleCommand.Connection = dbConnection;
                    oracleCommand.CommandText = str;
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add("p_person_id", OracleDbType.Long, personID, ParameterDirection.Input);
                    oracleCommand.Parameters.Add("p_parts", OracleDbType.Int32, pts, ParameterDirection.Input);
                    oracleCommand.Parameters.Add("p_personal_info", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_title_depts", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_positions", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_faculty", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_publications", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_specialty", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_location", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_education", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_clinical_interest", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_award", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_membership", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_insurance", OracleDbType.RefCursor, ParameterDirection.Output);
                    oracleCommand.Parameters.Add("p_top_insurance", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                    ((DbConnection)dbConnection).Open();
                    ((DbCommand)oracleCommand).ExecuteNonQuery();
                    DataSet dataSet = new DataSet();
                    ((DataAdapter)oracleDataAdapter).Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count == 0)
                    {
                        this.error = "Could not find the person ID";
                    }
                    else
                    {
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Demographics) != FacultyDirectory.Profile.ProfileParts.None)
                        {
                            foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                            {
                                this.firstName = dataRow["first_name"].ToString().Trim();
                                this.lastName = dataRow["last_name"].ToString().Trim();
                                this.middleName = dataRow["middle_name"].ToString().Trim();
                                this.title = dataRow["title"].ToString().Trim();
                                this.personalStatement = dataRow["personal_statement"].ToString().Replace("\r\n", "<br />");
                                this.address = dataRow["pl_name"].ToString().Trim() + "<br />" + dataRow["pl_address"].ToString().Trim() + "<br />" + dataRow["pl_city"].ToString() + ", " + dataRow["pl_state"].ToString() + " " + dataRow["pl_zip"].ToString();
                                this.noFormatPhone = dataRow["phone"].ToString();
                                this.phone = string.Format("({0}) {1} - {2}", (object)this.noFormatPhone.Substring(0, 3), (object)this.noFormatPhone.Substring(3, 3), (object)this.noFormatPhone.Substring(6, 4));
                                this.noFormatfax = dataRow["fax"].ToString();
                                this.fax = string.Format("({0}) {1} - {2}", (object)this.noFormatfax.Substring(0, 3), (object)this.noFormatfax.Substring(3, 3), (object)this.noFormatfax.Substring(6, 4));
                                this.academicPhone = dataRow["academicPhone"].ToString();
                                this.npi = dataRow["npi"].ToString();
                                this.primaryAppointment = dataRow["credentials"].ToString();
                                this.department = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dataRow["global_dept_name"].ToString().ToLower());
                                this.imageSrc = dataRow["img_url"].ToString();
                                this.locations_url = dataRow["locations_url"].ToString();
                                this.insurances_url = dataRow["insurance_url"].ToString();
                                this.health_care_phil = dataRow["health_care_phil"].ToString();
                                this.isAcceptingNewPatients = dataRow["accept_patients"].ToString() == "Y";
                            }
                        }
                        if ((pts & FacultyDirectory.Profile.ProfileParts.ClinicalInterests) != FacultyDirectory.Profile.ProfileParts.None)
                            this.clinicalInterests = this.GetClinicalInterests(dataSet.Tables[0].Rows);
                        if ((pts & FacultyDirectory.Profile.ProfileParts.ResearchInterests) != FacultyDirectory.Profile.ProfileParts.None)
                            this.researchInterests = this.GetResearchInterests(dataSet.Tables[0].Rows);
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Positions) != FacultyDirectory.Profile.ProfileParts.None)
                        {
                            this.appointments = this.GetAppointments(dataSet.Tables[1].Rows);
                            this.otherPositions = this.GetOtherPositions(dataSet.Tables[2].Rows);
                            this.presentPositions = this.GetOtherPositions(dataSet.Tables[2].Rows);
                            this.hasOtherPositions = this.otherPositions.Length > 0;
                            this.myPresentPositions = this.GetMyPresentPositions(dataSet.Tables[2].Rows);
                            this.myOtherPositions = this.GetMyOtherPositions(dataSet.Tables[2].Rows);
                        }
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Publications) != FacultyDirectory.Profile.ProfileParts.None)
                        {
                            this.publications = this.GetPublications(dataSet.Tables[4].Rows);
                            this.myPublications = this.GetMyPublications(dataSet.Tables[4].Rows);
                        }
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Specialties) != FacultyDirectory.Profile.ProfileParts.None)
                        {
                            this.specialties = this.GetSpecialties(dataSet.Tables[5].Rows);
                            this.mySpecialties = this.GetMySpecialties(dataSet.Tables[5].Rows);
                        }
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Addresses) != FacultyDirectory.Profile.ProfileParts.None)
                        {
                            this.addresses = this.GetAddresses(dataSet.Tables[6].Rows);
                            this.myAddresses = this.GetMyAddresses(dataSet.Tables[6].Rows);
                        }
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Degrees) != FacultyDirectory.Profile.ProfileParts.None)
                        {
                            this.degrees = this.GetDegrees(dataSet.Tables[7].Rows);
                            this.myDegrees = this.GetMyDegrees(dataSet.Tables[7].Rows);
                        }
                        if ((pts & FacultyDirectory.Profile.ProfileParts.ClinicalInterests) != FacultyDirectory.Profile.ProfileParts.None)
                        {
                            this.myInterests = this.GetMyClinicalInterests(dataSet.Tables[8].Rows);
                            this.hasClinicalInterests = this.myInterests.conditions.Length + this.myInterests.keywords.Length + this.myInterests.procedures.Length > 0;
                        }
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Awards) != FacultyDirectory.Profile.ProfileParts.None)
                            this.myAwards = this.GetMyAwards(dataSet.Tables[9].Rows);
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Memberships) != FacultyDirectory.Profile.ProfileParts.None)
                            this.myMemberships = this.GetMyMemberships(dataSet.Tables[10].Rows);
                        if ((pts & FacultyDirectory.Profile.ProfileParts.Insurances) != FacultyDirectory.Profile.ProfileParts.None)
                            this.myInsurances = this.GetMyInsurances(dataSet.Tables[11].Rows);
                        if ((pts & FacultyDirectory.Profile.ProfileParts.TopInsurances) != FacultyDirectory.Profile.ProfileParts.None)
                            this.myTopInsurances = this.GetMyInsurances(dataSet.Tables[12].Rows);
                        this.hasInterests = this.clinicalInterests.Length > 0 || this.researchInterests.Length > 0 || this.philosophy.Length > 0;
                    }
                    ((Component)oracleDataAdapter).Dispose();
                    dataSet.Dispose();
                    ((Component)oracleCommand).Dispose();
                    ((DbConnection)dbConnection).Close();
                    ((Component)dbConnection).Dispose();
                }
            }

            private string[] GetPhilosophy(DataRowCollection Rows)
            {
                string[] strArray = Rows[0]["health_care_phil"].ToString().Trim().Replace("\r\n", "|").Split('|');
                List<string> list = new List<string>();
                foreach (string str in strArray)
                {
                    if (!string.IsNullOrEmpty(str))
                        list.Add(str);
                }
                return list.ToArray();
            }

            private string[] GetClinicalInterests(DataRowCollection Rows)
            {
                string[] strArray = Rows[0]["clinical_interests"].ToString().Trim().Replace("\r\n", "|").Split('|');
                List<string> list = new List<string>();
                foreach (string str in strArray)
                {
                    if (!string.IsNullOrEmpty(str))
                        list.Add(str);
                }
                return list.ToArray();
            }

            private string[] GetResearchInterests(DataRowCollection Rows)
            {
                string[] strArray = Rows[0]["research_summary"].ToString().Trim().Replace("\r\n", "|").Split('|');
                List<string> list = new List<string>();
                foreach (string str in strArray)
                {
                    if (!string.IsNullOrEmpty(str))
                        list.Add(str);
                }
                return list.ToArray();
            }

            private string[] GetSpecialties(DataRowCollection Rows)
            {
                int num = 0;
                string[] strArray = new string[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string str = Rows[index]["specialty_name"].ToString().Trim();
                    if (!string.IsNullOrEmpty(str))
                    {
                        strArray[index] = str;
                        ++num;
                    }
                }
                if (num > 0)
                    return strArray;
                return new string[0];
            }

            private FacultyDirectory.specialty[] GetMySpecialties(DataRowCollection Rows)
            {
                List<FacultyDirectory.specialty> list = new List<FacultyDirectory.specialty>();
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string str1 = Rows[index]["specialty_name"].ToString().Trim();
                    if (!string.IsNullOrEmpty(str1))
                    {
                        string str2 = "";
                        string str3 = "";
                        list.Add(new FacultyDirectory.specialty()
                        {
                            title = str1,
                            url = str2,
                            contact = str3
                        });
                    }
                }
                return list.ToArray();
            }

            private string[] GetAppointments(DataRowCollection Rows)
            {
                string[] strArray1 = new string[Rows.Count];
                for (int index1 = 0; index1 < Rows.Count; ++index1)
                {
                    string str1 = Rows[index1]["title"].ToString().Trim();
                    string str2 = Rows[index1]["department"].ToString().Trim();
                    strArray1[index1] = string.IsNullOrEmpty(str1) ? "" : str1 + ", ";
                    string[] strArray2;
                    int index2;
                    (strArray2 = strArray1)[(int)(index2 = (int)index1)] = strArray2[index2] + str2;// CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str2.ToLower());
                }
                return strArray1;
            }

            private string[] GetAddresses(DataRowCollection Rows)
            {
                string[] strArray1 = new string[Rows.Count];
                for (int index1 = 0; index1 < Rows.Count; ++index1)
                {
                    string str1 = Rows[index1]["location_name"].ToString().Trim();
                    string str2 = Rows[index1]["address"].ToString().Trim();
                    string str3 = Rows[index1]["extra_detail"].ToString().Trim();
                    string str4 = Rows[index1]["city"].ToString().Trim();
                    string str5 = Rows[index1]["state"].ToString().Trim();
                    string str6 = Rows[index1]["zip"].ToString().Trim();
                    string[] strArray2;
                    int index2;
                    string str7 = (strArray2 = strArray1)[(int)(index2 = index1)] + (string.IsNullOrEmpty(str1) ? "" : str1);
                    strArray2[index2] = str7;
                    string[] strArray3;
                    int index3;
                    string str8 = (strArray3 = strArray1)[(int)(index3 = index1)] + (string.IsNullOrEmpty(str2) ? "" : ", " + str2);
                    strArray3[index3] = str8;
                    string[] strArray4;
                    int index4;
                    string str9 = (strArray4 = strArray1)[(int)(index4 = index1)] + (string.IsNullOrEmpty(str3) ? "" : ", " + str3);
                    strArray4[index4] = str9;
                    string[] strArray5;
                    int index5;
                    string str10 = (strArray5 = strArray1)[(int)(index5 = index1)] + (string.IsNullOrEmpty(str4) ? "" : ", " + str4);
                    strArray5[index5] = str10;
                    string[] strArray6;
                    int index6;
                    string str11 = (strArray6 = strArray1)[(int)(index6 = index1)] + (string.IsNullOrEmpty(str5) ? "" : ", " + str5);
                    strArray6[index6] = str11;
                    string[] strArray7;
                    int index7;
                    string str12 = (strArray7 = strArray1)[(int)(index7 = index1)] + (string.IsNullOrEmpty(str6) ? "" : ", " + str6);
                    strArray7[index7] = str12;
                }
                return strArray1;
            }

            private FacultyDirectory.address[] GetMyAddresses(DataRowCollection Rows)
            {
                FacultyDirectory.address[] addressArray = new FacultyDirectory.address[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["location_name"].ToString().Trim();
                    string val2 = Rows[index]["map_url"].ToString().Trim();
                    string val3 = Rows[index]["address"].ToString().Trim();
                    string val4 = Rows[index]["extra_detail"].ToString().Trim();
                    string val5 = Rows[index]["city"].ToString().Trim();
                    string val6 = Rows[index]["state"].ToString().Trim();
                    string val7 = Rows[index]["zip"].ToString().Trim();
                    addressArray[index] = new FacultyDirectory.address()
                    {
                        label = this.stringOrEmptyForNull(val1),
                        street = this.stringOrEmptyForNull(val3),
                        street2 = this.stringOrEmptyForNull(val4),
                        city = this.stringOrEmptyForNull(val5),
                        state = this.stringOrEmptyForNull(val6),
                        zip = this.stringOrEmptyForNull(val7),
                        url = this.stringOrEmptyForNull(val2)
                    };
                }
                return addressArray;
            }

            private string[] GetOtherPositions(DataRowCollection Rows)
            {
                List<string> list = new List<string>();
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string str1 = Rows[index]["position"].ToString().Trim();
                    string str2 = Rows[index]["department"].ToString().Trim();
                    string str3 = Rows[index]["institution"].ToString().Trim();
                    string str4 = Rows[index]["year_from"].ToString().Trim();
                    string str5 = Rows[index]["year_to"].ToString().Trim();
                    if (str5 != "" && str5 != "9999")
                    {
                        string str6 = (string.IsNullOrEmpty(str1) ? "" : str1 + " ") + (string.IsNullOrEmpty(str2) ? "" : "(" + str2 + ", ") + (string.IsNullOrEmpty(str3) ? "" : str3 + " ") + (string.IsNullOrEmpty(str4) ? "" : str4 + " - ") + str5 + ")";
                        list.Add(str6);
                    }
                }
                return list.ToArray();
            }

            private FacultyDirectory.position[] GetMyOtherPositions(DataRowCollection Rows)
            {
                List<FacultyDirectory.position> list = new List<FacultyDirectory.position>();
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["position"].ToString().Trim();
                    string val2 = Rows[index]["department"].ToString().Trim();
                    string val3 = Rows[index]["institution"].ToString().Trim();
                    string val4 = Rows[index]["year_from"].ToString().Trim();
                    string val5 = Rows[index]["year_to"].ToString().Trim();
                    if (val5 != "" && val5 != "9999")
                        list.Add(new FacultyDirectory.position()
                        {
                            title = this.stringOrEmptyForNull(val1),
                            department = this.stringOrEmptyForNull(val2),
                            institution = this.stringOrEmptyForNull(val3),
                            yearFrom = this.stringOrEmptyForNull(val4),
                            yearTo = this.stringOrEmptyForNull(val5)
                        });
                }
                return list.ToArray();
            }

            private string[] GetPresentPositions(DataRowCollection Rows)
            {
                List<string> list = new List<string>();
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string str1 = Rows[index]["position"].ToString().Trim();
                    string str2 = Rows[index]["department"].ToString().Trim();
                    string str3 = Rows[index]["institution"].ToString().Trim();
                    Rows[index]["year_from"].ToString().Trim();
                    string str4 = Rows[index]["year_to"].ToString().Trim();
                    if (str4 == "" || str4 == "9999")
                    {
                        string str5 = (string.IsNullOrEmpty(str1) ? "" : str1 + ", ") + (string.IsNullOrEmpty(str2) ? "" : str2 + ", ") + (string.IsNullOrEmpty(str3) ? "" : str3);
                        list.Add(str5);
                    }
                }
                return list.ToArray();
            }

            private FacultyDirectory.position[] GetMyPresentPositions(DataRowCollection Rows)
            {
                List<FacultyDirectory.position> list = new List<FacultyDirectory.position>();
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["position"].ToString().Trim();
                    string val2 = Rows[index]["department"].ToString().Trim();
                    string val3 = Rows[index]["institution"].ToString().Trim();
                    string val4 = Rows[index]["year_from"].ToString().Trim();
                    string val5 = Rows[index]["year_to"].ToString().Trim();
                    if (val5 == "" || val5 == "9999")
                        list.Add(new FacultyDirectory.position()
                        {
                            title = this.stringOrEmptyForNull(val1),
                            department = this.stringOrEmptyForNull(val2),
                            institution = this.stringOrEmptyForNull(val3),
                            yearFrom = this.stringOrEmptyForNull(val4),
                            yearTo = this.stringOrEmptyForNull(val5)
                        });
                }
                return list.ToArray();
            }

            private string GetDegrees(DataRowCollection Rows)
            {
                string[] strArray = new string[Rows.Count];
                string str1 = "";
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string str2 = Rows[index]["degree"].ToString().Trim();
                    string str3 = Rows[index]["specialty"].ToString().Trim();
                    string str4 = Rows[index]["institution"].ToString().Trim();
                    string str5 = Rows[index]["graduate_date"].ToString().Trim();
                    str1 = str1 + "<li>" + (string.IsNullOrEmpty(str2) ? "" : "<strong>" + str2 + "</strong>") + (string.IsNullOrEmpty(str3) ? "" : ", " + str3) + (string.IsNullOrEmpty(str4) ? "" : ", " + str4) + (string.IsNullOrEmpty(str5) ? "" : ", " + str5) + "</li>";
                }
                if (str1 != "")
                    str1 = "<ul>" + str1 + "</ul>";
                return str1;
            }

            private string stringOrEmptyForNull(string val)
            {
                return string.IsNullOrEmpty(val) ? "" : val;
            }

            private FacultyDirectory.degree[] GetMyDegrees(DataRowCollection Rows)
            {
                FacultyDirectory.degree[] degreeArray = new FacultyDirectory.degree[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["degree"].ToString().Trim();
                    string val2 = Rows[index]["specialty"].ToString().Trim();
                    string val3 = Rows[index]["institution"].ToString().Trim();
                    string val4 = Rows[index]["graduate_date"].ToString().Trim();
                    degreeArray[index] = new FacultyDirectory.degree()
                    {
                        program = this.stringOrEmptyForNull(val1),
                        specialty = this.stringOrEmptyForNull(val2),
                        institution = this.stringOrEmptyForNull(val3),
                        yearEarned = this.stringOrEmptyForNull(val4)
                    };
                }
                return degreeArray;
            }

            private string[] GetPublications(DataRowCollection Rows)
            {
                string[] strArray1 = new string[Rows.Count];
                for (int index1 = 0; index1 < Rows.Count; ++index1)
                {
                    string str1 = Rows[index1]["authors"].ToString().Trim();
                    string str2 = Rows[index1]["publication_article"].ToString().Trim();
                    string str3 = Rows[index1]["journal_name"].ToString().Trim();
                    string str4 = Rows[index1]["volume_number"].ToString().Trim();
                    string str5 = Rows[index1]["issue_number"].ToString().Trim();
                    string str6 = Rows[index1]["page_number"].ToString().Trim();
                    string str7 = Rows[index1]["year"].ToString().Trim();
                    string str8 = Rows[index1]["pmid"].ToString().Trim();
                    strArray1[index1] = string.IsNullOrEmpty(str1) ? "" : str1 + " ";
                    string[] strArray2;
                    int index2;
                    string str9 = (strArray2 = strArray1)[(index2 = index1)] + (string.IsNullOrEmpty(str2) ? "" : "<strong>" + str2 + "</strong> ");
                    strArray2[index2] = str9;
                    string[] strArray3;
                    int index3;
                    string str10 = (strArray3 = strArray1)[(index3 = index1)] + (string.IsNullOrEmpty(str3) ? "" : "<i>" + str3 + "</i>. ");
                    strArray3[index3] = str10;
                    string[] strArray4;
                    int index4;
                    string str11 = (strArray4 = strArray1)[(index4 = index1)] + (string.IsNullOrEmpty(str4) ? "" : str4);
                    strArray4[index4] = str11;
                    string[] strArray5;
                    int index5;
                    string str12 = (strArray5 = strArray1)[(index5 = index1)] + (string.IsNullOrEmpty(str5) ? "" : "(" + str5 + ") ");
                    strArray5[index5] = str12;
                    string[] strArray6;
                    int index6;
                    string str13 = (strArray6 = strArray1)[(int)(index6 = index1)] + (string.IsNullOrEmpty(str6) ? "" : ": " + str6);
                    strArray6[index6] = str13;
                    string[] strArray7;
                    int index7;
                    string str14 = (strArray7 = strArray1)[(int)(index7 = index1)] + (string.IsNullOrEmpty(str7) ? "" : ", " + str7 + ". ");
                    strArray7[index7] = str14;
                    string[] strArray8;
                    int index8;
                    string str15 = (strArray8 = strArray1)[(int)(index8 = index1)];
                    string str16;
                    if (!string.IsNullOrEmpty(str8))
                        str16 = "<a href=\"http://www.ncbi.nlm.nih.gov/pubmed/" + str8 + "\" target=\"_blank\">http://www.ncbi.nlm.nih.gov/pubmed/" + str8 + "</a>";
                    else
                        str16 = "";
                    string str17 = str15 + str16;
                    strArray8[index8] = str17;
                }
                return strArray1;
            }

            private FacultyDirectory.publication[] GetMyPublications(DataRowCollection Rows)
            {
                FacultyDirectory.publication[] publicationArray = new FacultyDirectory.publication[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["authors"].ToString().Trim();
                    string val2 = Rows[index]["publication_article"].ToString().Trim();
                    string val3 = Rows[index]["journal_name"].ToString().Trim();
                    string val4 = Rows[index]["volume_number"].ToString().Trim();
                    string val5 = Rows[index]["issue_number"].ToString().Trim();
                    string val6 = Rows[index]["page_number"].ToString().Trim();
                    string val7 = Rows[index]["year"].ToString().Trim();
                    string val8 = Rows[index]["pmid"].ToString().Trim();
                    publicationArray[index] = new FacultyDirectory.publication()
                    {
                        authors = this.stringOrEmptyForNull(val1),
                        article = this.stringOrEmptyForNull(val2),
                        journal = this.stringOrEmptyForNull(val3),
                        volume = this.stringOrEmptyForNull(val4),
                        issue = this.stringOrEmptyForNull(val5),
                        page = this.stringOrEmptyForNull(val6),
                        year = this.stringOrEmptyForNull(val7),
                        pmid = this.stringOrEmptyForNull(val8)
                    };
                }
                return publicationArray;
            }

            private FacultyDirectory.clinicalInterest GetMyClinicalInterests(DataRowCollection Rows)
            {
                FacultyDirectory.clinicalInterest clinicalInterest = new FacultyDirectory.clinicalInterest();
                List<FacultyDirectory.interest> list1 = new List<FacultyDirectory.interest>();
                List<FacultyDirectory.interest> list2 = new List<FacultyDirectory.interest>();
                List<FacultyDirectory.interest> list3 = new List<FacultyDirectory.interest>();
                for (int index = 0; index < Rows.Count; ++index)
                {
                    Rows[index]["keyword_id"].ToString().Trim();
                    string val1 = Rows[index]["keyword"].ToString().Trim();
                    string val2 = Rows[index]["url"].ToString().Trim();
                    string val3 = Rows[index]["keyword_type"].ToString().Trim();
                    FacultyDirectory.interest interest = new FacultyDirectory.interest();
                    interest.type = this.stringOrEmptyForNull(val3);
                    interest.name = this.stringOrEmptyForNull(val1);
                    interest.url = this.stringOrEmptyForNull(val2);
                    switch (val3)
                    {
                        case "1":
                            list1.Add(interest);
                            break;
                        case "3":
                            list3.Add(interest);
                            break;
                        case "2":
                            list2.Add(interest);
                            break;
                    }
                }
                clinicalInterest.keywords = list3.ToArray();
                clinicalInterest.procedures = list2.ToArray();
                clinicalInterest.conditions = list1.ToArray();
                return clinicalInterest;
            }

            private FacultyDirectory.membership[] GetMyMemberships(DataRowCollection Rows)
            {
                FacultyDirectory.membership[] membershipArray = new FacultyDirectory.membership[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["membership_name"].ToString().Trim();
                    string val2 = Rows[index]["member_title"].ToString().Trim();
                    string val3 = Rows[index]["start_date"].ToString().Trim();
                    string val4 = Rows[index]["end_date"].ToString().Trim();
                    if (val4 == "" || val4 == "9999")
                        val4 = "Present";
                    membershipArray[index] = new FacultyDirectory.membership()
                    {
                        name = this.stringOrEmptyForNull(val1),
                        title = this.stringOrEmptyForNull(val2),
                        startDate = this.stringOrEmptyForNull(val3),
                        endDate = this.stringOrEmptyForNull(val4)
                    };
                }
                return membershipArray;
            }

            private FacultyDirectory.award[] GetMyAwards(DataRowCollection Rows)
            {
                FacultyDirectory.award[] awardArray = new FacultyDirectory.award[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["title"].ToString().Trim();
                    string val2 = Rows[index]["organization"].ToString().Trim();
                    string val3 = Rows[index]["year_awarded"].ToString().Trim();
                    awardArray[index] = new FacultyDirectory.award()
                    {
                        title = this.stringOrEmptyForNull(val1),
                        organization = this.stringOrEmptyForNull(val2),
                        year = this.stringOrEmptyForNull(val3)
                    };
                }
                return awardArray;
            }

            private FacultyDirectory.insurance[] GetMyInsurances(DataRowCollection Rows)
            {
                FacultyDirectory.insurance[] insuranceArray = new FacultyDirectory.insurance[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val = Rows[index]["name"].ToString().Trim();
                    insuranceArray[index] = new FacultyDirectory.insurance()
                    {
                        name = this.stringOrEmptyForNull(val)
                    };
                }
                return insuranceArray;
            }

            private string[] GetSignProgram(DataRowCollection Rows)
            {
                string[] strArray = new string[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string str = Rows[index]["program_name"].ToString().Trim();
                    strArray[index] = string.IsNullOrEmpty(str) ? "" : str;
                }
                return strArray;
            }

            private string[] GetLectures(DataRowCollection Rows)
            {
                string[] strArray1 = new string[Rows.Count];
                for (int index1 = 0; index1 < Rows.Count; ++index1)
                {
                    string str1 = Rows[index1]["lecture_title"].ToString().Trim();
                    string str2 = Rows[index1]["orgnization"].ToString().Trim();
                    string str3 = Rows[index1]["lecture_date"].ToString().Trim();
                    string str4 = Rows[index1]["state"].ToString().Trim();
                    string str5 = Rows[index1]["country"].ToString().Trim();
                    Rows[index1]["keynote"].ToString().Trim();
                    strArray1[index1] = string.IsNullOrEmpty(str1) ? "" : "\"" + str1 + "\" ";
                    string[] strArray2;
                    int index2;
                    string str6 = (strArray2 = strArray1)[(int)(index2 = index1)] + (string.IsNullOrEmpty(str2) ? "" : str2 + ". ");
                    strArray2[index2] = str6;
                    string[] strArray3;
                    int index3;
                    string str7 = (strArray3 = strArray1)[(int)(index3 = index1)] + (string.IsNullOrEmpty(str4) ? "" : str4 + ", ");
                    strArray3[index3] = str7;
                    string[] strArray4;
                    int index4;
                    string str8 = (strArray4 = strArray1)[(int)(index4 = index1)] + (string.IsNullOrEmpty(str5) ? "" : str5);
                    strArray4[index4] = str8;
                    string[] strArray5;
                    int index5;
                    string str9 = (strArray5 = strArray1)[(int)(index5 = index1)] + (string.IsNullOrEmpty(str3) ? "" : " - " + str3 + ".");
                    strArray5[index5] = str9;
                }
                return strArray1;
            }

            private FacultyDirectory.lecture[] GetMyLectures(DataRowCollection Rows)
            {
                FacultyDirectory.lecture[] lectureArray = new FacultyDirectory.lecture[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["lecture_title"].ToString().Trim();
                    string val2 = Rows[index]["orgnization"].ToString().Trim();
                    string val3 = Rows[index]["lecture_date"].ToString().Trim();
                    string val4 = Rows[index]["state"].ToString().Trim();
                    string val5 = Rows[index]["country"].ToString().Trim();
                    string val6 = Rows[index]["keynote"].ToString().Trim();
                    lectureArray[index] = new FacultyDirectory.lecture()
                    {
                        title = this.stringOrEmptyForNull(val1),
                        organization = this.stringOrEmptyForNull(val2),
                        date = this.stringOrEmptyForNull(val3),
                        state = this.stringOrEmptyForNull(val4),
                        country = this.stringOrEmptyForNull(val5),
                        keynote = this.stringOrEmptyForNull(val6)
                    };
                }
                return lectureArray;
            }

            private string[] GetMemberships(DataRowCollection Rows)
            {
                string[] strArray1 = new string[Rows.Count];
                for (int index1 = 0; index1 < Rows.Count; ++index1)
                {
                    string str1 = Rows[index1]["membership_name"].ToString().Trim();
                    string str2 = Rows[index1]["member_title"].ToString().Trim();
                    string str3 = Rows[index1]["start_date"].ToString().Trim();
                    string str4 = Rows[index1]["end_date"].ToString().Trim();
                    if (str4 == "" || str4 == "9999")
                        str4 = "Present";
                    strArray1[index1] = string.IsNullOrEmpty(str1) ? "" : str1 + " ";
                    string[] strArray2;
                    int index2;
                    string str5 = (strArray2 = strArray1)[(int)(index2 = index1)] + (string.IsNullOrEmpty(str2) ? "" : "(" + str2 + ", ");
                    strArray2[index2] = str5;
                    string[] strArray3;
                    int index3;
                    string str6 = (strArray3 = strArray1)[(int)(index3 = index1)] + (string.IsNullOrEmpty(str3) ? "" : str3 + " - ");
                    strArray3[index3] = str6;
                    string[] strArray4;
                    int index4;
                    (strArray4 = strArray1)[(int)(index4 = (int)index1)] = strArray4[index4] + str4 + ")";
                }
                return strArray1;
            }

            private string[] GetAwards(DataRowCollection Rows)
            {
                string[] strArray1 = new string[Rows.Count];
                for (int index1 = 0; index1 < Rows.Count; ++index1)
                {
                    string str1 = Rows[index1]["title"].ToString().Trim();
                    string str2 = Rows[index1]["organization"].ToString().Trim();
                    string str3 = Rows[index1]["year_awarded"].ToString().Trim();
                    strArray1[index1] = string.IsNullOrEmpty(str1) ? "" : str1 + " ";
                    string[] strArray2;
                    int index2;
                    string str4 = (strArray2 = strArray1)[(int)(index2 = index1)] + (string.IsNullOrEmpty(str2) ? "" : "(" + str2 + " - ");
                    strArray2[index2] = str4;
                    string[] strArray3;
                    int index3;
                    string str5 = (strArray3 = strArray1)[(int)(index3 = index1)] + (string.IsNullOrEmpty(str3) ? "" : str3 + ")");
                    strArray3[index3] = str5;
                }
                return strArray1;
            }

            private string[] GetGrants(DataRowCollection Rows)
            {
                string[] strArray1 = new string[Rows.Count];
                for (int index1 = 0; index1 < Rows.Count; ++index1)
                {
                    string str1 = Rows[index1]["title"].ToString().Trim();
                    string str2 = string.Format("Order Total: {0:C}", (object)Rows[index1]["amount"].ToString().Trim());
                    string str3 = Rows[index1]["start_date"].ToString().Trim();
                    string str4 = Rows[index1]["end_date"].ToString().Trim();
                    if (str4 == "" || str4 == "9999")
                        str4 = "Present";
                    strArray1[index1] = string.IsNullOrEmpty(str1) ? "" : str1 + " ";
                    string[] strArray2;
                    int index2;
                    string str5 = (strArray2 = strArray1)[(int)(index2 = index1)] + (string.IsNullOrEmpty(str2) ? "" : "(" + str2 + ", ");
                    strArray2[index2] = str5;
                    string[] strArray3;
                    int index3;
                    string str6 = (strArray3 = strArray1)[(int)(index3 = index1)] + (string.IsNullOrEmpty(str3) ? "" : str3 + " - ");
                    strArray3[index3] = str6;
                    string[] strArray4;
                    int index4;
                    (strArray4 = strArray1)[(int)(index4 = (int)index1)] = strArray4[index4] + str4 + ")";
                }
                return strArray1;
            }

            private FacultyDirectory.grant[] GetMyGrants(DataRowCollection Rows)
            {
                FacultyDirectory.grant[] grantArray = new FacultyDirectory.grant[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["title"].ToString().Trim();
                    string val2 = string.Format("Order Total: {0:C}", (object)Rows[index]["amount"].ToString().Trim());
                    string val3 = Rows[index]["start_date"].ToString().Trim();
                    string val4 = Rows[index]["end_date"].ToString().Trim();
                    if (val4 == "" || val4 == "9999")
                        val4 = "Present";
                    grantArray[index] = new FacultyDirectory.grant()
                    {
                        title = this.stringOrEmptyForNull(val1),
                        amount = this.stringOrEmptyForNull(val2),
                        startDate = this.stringOrEmptyForNull(val3),
                        endDate = this.stringOrEmptyForNull(val4)
                    };
                }
                return grantArray;
            }

            private string[] GetPatents(DataRowCollection Rows)
            {
                string[] strArray1 = new string[Rows.Count];
                for (int index1 = 0; index1 < Rows.Count; ++index1)
                {
                    string str1 = Rows[index1]["title"].ToString().Trim();
                    string str2 = Rows[index1]["ip_owner"].ToString().Trim();
                    string str3 = Rows[index1]["year_obtained"].ToString().Trim();
                    strArray1[index1] = string.IsNullOrEmpty(str1) ? "" : str1 + " ";
                    string[] strArray2;
                    int index2;
                    string str4 = (strArray2 = strArray1)[(int)(index2 = index1)] + (string.IsNullOrEmpty(str2) ? "" : "(" + str2 + " - ");
                    strArray2[index2] = str4;
                    string[] strArray3;
                    int index3;
                    string str5 = (strArray3 = strArray1)[(int)(index3 = index1)] + (string.IsNullOrEmpty(str3) ? "" : str3 + ")");
                    strArray3[index3] = str5;
                }
                return strArray1;
            }

            private FacultyDirectory.patent[] GetMyPatents(DataRowCollection Rows)
            {
                FacultyDirectory.patent[] patentArray = new FacultyDirectory.patent[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["title"].ToString().Trim();
                    string val2 = Rows[index]["ip_owner"].ToString().Trim();
                    string val3 = Rows[index]["year_obtained"].ToString().Trim();
                    patentArray[index] = new FacultyDirectory.patent()
                    {
                        title = this.stringOrEmptyForNull(val1),
                        ipOwner = this.stringOrEmptyForNull(val2),
                        year = this.stringOrEmptyForNull(val3)
                    };
                }
                return patentArray;
            }

            private FacultyDirectory.appointment[] GetMyAppointments(DataRowCollection Rows)
            {
                FacultyDirectory.appointment[] appointmentArray = new FacultyDirectory.appointment[Rows.Count];
                for (int index = 0; index < Rows.Count; ++index)
                {
                    string val1 = Rows[index]["title"].ToString().Trim();
                    string val2 = Rows[index]["department"].ToString().Trim();
                    appointmentArray[index] = new FacultyDirectory.appointment()
                    {
                        position = this.stringOrEmptyForNull(val1),
                        department = this.stringOrEmptyForNull(val2)
                    };
                }
                return appointmentArray;
            }

            public static FacultyDirectory.profileFile getProfilePicture(string person_id)
            {
                FacultyDirectory.profileFile profileFile = new FacultyDirectory.profileFile();
                using (OracleConnection dbConnection = Util.getDBConnection())
                {
                    OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.get_profile_picture", dbConnection);
                    ((DbCommand)oracleCommand).CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add("p_person_id", OracleDbType.Long, person_id, ParameterDirection.Input);
                    oracleCommand.Parameters.Add("r_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                    ((DbCommand)oracleCommand).ExecuteNonQuery();
                    DataSet dataSet = new DataSet();
                    ((DataAdapter)oracleDataAdapter).Fill(dataSet);
                    foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    {
                        profileFile.extension = dataRow["file_ext"].ToString().Trim();
                        profileFile.mimeType = dataRow["file_mimetype"].ToString().Trim();
                        profileFile.person_id = dataRow["person_id"].ToString().Trim();
                        profileFile.name = profileFile.person_id + "." + profileFile.extension;
                        profileFile.fileContent = (byte[])dataRow["file_binary"];
                    }
                    dataSet.Dispose();
                    ((Component)oracleDataAdapter).Dispose();
                    ((Component)oracleCommand).Dispose();
                    ((DbConnection)dbConnection).Close();
                    ((Component)dbConnection).Dispose();
                }
                return profileFile;
            }

            public static FacultyDirectory.profileFile getProfileCV(string person_id)
            {
                FacultyDirectory.profileFile profileFile = new FacultyDirectory.profileFile();
                using (OracleConnection dbConnection = Util.getDBConnection())
                {
                    OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.get_profile_resume", dbConnection);
                    ((DbCommand)oracleCommand).CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add("p_person_id", OracleDbType.Long, person_id, ParameterDirection.Input);
                    oracleCommand.Parameters.Add("r_cur", OracleDbType.RefCursor, ParameterDirection.Output);
                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                    ((DbCommand)oracleCommand).ExecuteNonQuery();
                    DataSet dataSet = new DataSet();
                    ((DataAdapter)oracleDataAdapter).Fill(dataSet);
                    foreach (DataRow dataRow in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                    {
                        profileFile.extension = dataRow["file_ext"].ToString().Trim();
                        profileFile.mimeType = dataRow["file_mimetype"].ToString().Trim();
                        profileFile.person_id = dataRow["person_id"].ToString().Trim();
                        profileFile.name = profileFile.person_id + "." + profileFile.extension;
                        profileFile.fileContent = (byte[])dataRow["file_binary"];
                    }
                    dataSet.Dispose();
                    ((Component)oracleDataAdapter).Dispose();
                    ((Component)oracleCommand).Dispose();
                    ((DbConnection)dbConnection).Close();
                    ((Component)dbConnection).Dispose();
                }
                return profileFile;
            }

            [Flags]
            public enum ProfileParts
            {
                All = 65535,
                t17 = 32768,
                t16 = 16384,
                ResearchInterests = 8192,
                TitleDepartment = 4096,
                ResearchSummary = 2048,
                TopInsurances = 1024,
                Specialties = 512,
                Insurances = 256,
                Memberships = 128,
                Awards = 64,
                Addresses = 32,
                ClinicalInterests = 16,
                Degrees = 8,
                Positions = 4,
                Publications = 2,
                Demographics = 1,
                None = 0,
            }

            [Flags]
            public enum ProfileType
            {
                Research = 0,
                Clinical = 1,
            }
        }

        public class publication
        {
            public string authors;
            public string article;
            public string journal;
            public string volume;
            public string issue;
            public string page;
            public string year;
            public string pmid;

            public publication()
            {
                this.authors = "";
                this.article = "";
                this.journal = "";
                this.volume = "";
                this.issue = "";
                this.page = "";
                this.year = "";
                this.pmid = "";
            }
        }

        public class degree
        {
            public string program;
            public string specialty;
            public string institution;
            public string yearEarned;

            public degree()
            {
                this.program = "";
                this.specialty = "";
                this.institution = "";
                this.yearEarned = "";
            }
        }

        public class position
        {
            public string title;
            public string department;
            public string institution;
            public string yearFrom;
            public string yearTo;

            public position()
            {
                this.title = "";
                this.department = "";
                this.institution = "";
                this.yearFrom = "";
                this.yearTo = "";
            }
        }

        public class interest
        {
            public string name;
            public string type;
            public string url;

            public interest()
            {
                this.name = "";
                this.type = "";
                this.url = "";
            }
        }

        public class clinicalInterest
        {
            public FacultyDirectory.interest[] conditions;
            public FacultyDirectory.interest[] procedures;
            public FacultyDirectory.interest[] keywords;
        }

        public class address
        {
            public string label;
            public string url;
            public string street;
            public string street2;
            public string city;
            public string state;
            public string zip;

            public address()
            {
                this.label = "";
                this.street = "";
                this.street2 = "";
                this.city = "";
                this.state = "";
                this.zip = "";
                this.url = "";
            }
        }

        public class membership
        {
            public string name;
            public string title;
            public string startDate;
            public string endDate;

            public membership()
            {
                this.name = "";
                this.title = "";
                this.startDate = "";
                this.endDate = "";
            }
        }

        public class award
        {
            public string title;
            public string organization;
            public string year;

            public award()
            {
                this.title = "";
                this.organization = "";
                this.year = "";
            }
        }

        public class insurance
        {
            public string name;

            public insurance()
            {
                this.name = "";
            }
        }

        public class specialty
        {
            public string title;
            public string url;
            public string contact;

            public specialty()
            {
                this.title = "";
                this.url = "";
                this.contact = "";
            }
        }

        public class patent
        {
            public string title;
            public string ipOwner;
            public string year;

            public patent()
            {
                this.title = this.ipOwner = this.year = "";
            }
        }

        public class lecture
        {
            public string title;
            public string organization;
            public string date;
            public string state;
            public string country;
            public string keynote;

            public lecture()
            {
                this.title = this.organization = this.date = this.state = this.country = this.keynote = "";
            }
        }

        public class grant
        {
            public string title;
            public string amount;
            public string startDate;
            public string endDate;

            public grant()
            {
                this.title = this.amount = this.startDate = this.endDate = "";
            }
        }

        public class appointment
        {
            public string position;
            public string department;

            public appointment()
            {
                this.position = this.department = "";
            }
        }

        public class profileFile
        {
            public string name;
            public string mimeType;
            public string extension;
            public string person_id;
            public byte[] fileContent;

            public profileFile()
            {
                this.name = this.mimeType = this.extension = this.person_id = "";
                this.fileContent = new byte[0];
            }
        }
    }
}
