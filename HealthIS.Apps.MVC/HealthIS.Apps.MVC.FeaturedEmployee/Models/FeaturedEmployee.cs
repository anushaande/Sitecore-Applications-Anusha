using System;
using HealthIS.Apps;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;

namespace HealthIS.Apps.MVC.Models
{
    public class FeaturedEmployee : IRenderingModel
    {
        public FacultyDirectory.Profile person;
        public int personID = -1;
        public bool validId;
        public string[] profileArray = {};
        public string profilePage;
        public string profileList;
        public string errMessage = string.Empty;
        public string facDirErr = string.Empty;

        public Rendering Rendering { get; set; }
        public Item PageItem { get; set; }
        public Item dsItem { get; set; }
        public string dataSourceID { get; set; }
        public bool dsIsSet { get; set; }

        public void Initialize(Rendering rendering)
        {
            PageItem = PageContext.Current.Item;
            Rendering = rendering;
            dsIsSet = !String.IsNullOrEmpty(rendering.DataSource);
            if (dsIsSet)
            {
                try
                {
                    dsItem = Sitecore.Context.Database.GetItem(rendering.DataSource);
                    if (dsItem == null)
                    {
                        dataSourceID = "";
                        dsIsSet = false;
                    }
                    else { dataSourceID = dsItem.ID.Guid.ToString(); }
                }
                catch { dataSourceID = ""; }
            }
        }

        public void ProcessInput()
        {
            if (!String.IsNullOrEmpty(dataSourceID))
            {
                profilePage = Helpers.getStrField(dsItem, "Profile Page");
                profilePage = profilePage.Split('*')[0];
                profilePage = profilePage.Replace("/sitecore/content/Home", "").TrimEnd('/');

                profileList = Helpers.getStrField(dsItem, "PersonID List").TrimStart(' ',';').TrimEnd(' ',';');
                if (String.IsNullOrEmpty(profileList))
                {
                    errMessage = "Please use search field to add employees or enter a list of person IDs below.<br />" +
                            "IDs should be separated by a semi-colon (;) - eg. 0001; 0002; 0003";
                }
                else
                {
                    string[] invalidList = getInvalidIDs(profileList);
                    if (String.IsNullOrEmpty(invalidList[0]) && String.IsNullOrEmpty(invalidList[1]))
                    {
                        profileArray = profileList.Split(';');
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(invalidList[0]))
                        {
                            errMessage = "Invalid ID in list (" + invalidList[0] + ")";
                        }
                        if (!String.IsNullOrEmpty(invalidList[1]))
                        {
                            facDirErr = "Could not find person (" + invalidList[1] + ")";
                        }
                    }
                }
            }
        }

        public void GenerateFeaturedEmployee(string[] profiles)
        {
            validId = true;
            int featuredProfile = 0;
            try
            {
                if (!String.IsNullOrEmpty(dataSourceID) && profiles.Length > 0)
                {
                    Random randomGen = new Random();
                    featuredProfile = randomGen.Next(0, profiles.Length);

                    personID = int.Parse(profiles[featuredProfile]);
                    person = new FacultyDirectory.Profile(personID, FacultyDirectory.Profile.ProfileType.Research, FacultyDirectory.Profile.ProfileParts.All);
                }
            }
            catch (Exception ex)
            {
                validId = false;
            }
        }

        public static string[] getInvalidIDs(string list)
        {
            int personid = -1;
            string[] arr = list.Replace(";;",";").Split(';');
            string[] invalidIDs = {"",""};
            foreach (string id in arr)
            {
                try
                {
                    personid = int.Parse(id);
                    FacultyDirectory.Profile p = new FacultyDirectory.Profile(personid, FacultyDirectory.Profile.ProfileType.Research, FacultyDirectory.Profile.ProfileParts.All);
                    if (!String.IsNullOrEmpty(p.error))
                    {
                        if (!String.IsNullOrEmpty(invalidIDs[1])) { invalidIDs[1] = invalidIDs[1] + ", '" + id.TrimStart(' ') + "'"; }
                        else { invalidIDs[1] = "'" + id.TrimStart(' ') + "'"; }
                    }
                }
                catch (Exception ex)
                {
                    if (!String.IsNullOrEmpty(invalidIDs[0])) { invalidIDs[0] = invalidIDs[0] + ", '" + id.TrimStart(' ') + "'"; }
                    else { invalidIDs[0] = "'" + id.TrimStart(' ') + "'"; }
                }
            }
            return invalidIDs;
        }
    }
}