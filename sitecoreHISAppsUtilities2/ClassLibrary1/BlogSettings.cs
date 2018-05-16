using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthIS.Apps
{
    public static class BlogSettings
    {
        private static string CurrentBlogDepartment
        {
            get
            {
                string groupList = Settings.GetSetting("Blog_Group_List");
                List<string> eachGroup = groupList.Split('|').ToList();
                bool isRoleAssigned = false;
                string roleName = "HealthIS_Blog";

                foreach (string name in eachGroup)
                {
                    isRoleAssigned = User.Current.IsInRole("hscnet\\SC_Blog_" + name + "_Author");
                    roleName = name + "_Blog";
                    if (isRoleAssigned) break;
                }

                return roleName;
            }
        }

        private static Database DatabaseName
        {
            get
            {
                string databaseName = "master";
                if (Util.isOnProduction())
                {
                    databaseName = "web";
                }

                return Sitecore.Data.Database.GetDatabase(databaseName);
            }
        }

        public static string SearchIndexName = Settings.GetSetting(CurrentBlogDepartment + "_Search_Index_Name") + DatabaseName.Name;
        public static Item ArticleRootPath = DatabaseName.GetItem(Settings.GetSetting(CurrentBlogDepartment + "_Article_Root_Path"));
        public static Item TemplatePath = DatabaseName.GetItem("/sitecore/templates/" + Settings.GetSetting(CurrentBlogDepartment + "_Template_Path"));
        public static Item CategoryRepository = DatabaseName.GetItem(Settings.GetSetting(CurrentBlogDepartment + "_Category_Repository"));
        public static Item TagRepository = DatabaseName.GetItem(Settings.GetSetting(CurrentBlogDepartment + "_Tag_Repository"));
        public static string CategoryFacetSource = Settings.GetSetting(CurrentBlogDepartment + "_Category_Facet_Source");
        public static string TagFacetSource = Settings.GetSetting(CurrentBlogDepartment + "_Tag_Facet_Source");
        public static Item TagTemplatePath = DatabaseName.GetItem("/sitecore/templates/" + Settings.GetSetting(CurrentBlogDepartment + "_Tag_Template_Path"));
        public static string ImageLibrary = Settings.GetSetting(CurrentBlogDepartment + "_Image_Library");
    }
}
