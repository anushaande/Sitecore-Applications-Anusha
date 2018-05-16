using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Security.Accounts;

namespace HealthIS.Apps.MVC.Models
{
    public class RedirectToDepartmentPage : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item item { get; set; }
        public bool isDatasourceNull { get; set; }
        public string targetLocation { get; set; }
        public bool isExternalUser
        {
            get
            {
                return User.Current.IsInRole("hscnet\\SC_Global_External");
            }
        }

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            base.Initialize(rendering);

            try
            {
                item = Item.Database.GetItem(Rendering.DataSource);
                isDatasourceNull = item == null ? true : false;

                if (!isDatasourceNull)
                {
                    NameValueListField deptList = item.Fields["Default URL"];
                    if (deptList != null)
                    {
                        System.Collections.Specialized.NameValueCollection nameValueCollection = deptList.NameValues;

                        foreach (string deptName in nameValueCollection.AllKeys)
                        {
                            if (User.Current.IsInRole("hscnet\\SC_" + deptName + "_CONTENT"))
                            {
                                targetLocation = nameValueCollection[deptName];
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                isDatasourceNull = true;
            }
        }
    }
}