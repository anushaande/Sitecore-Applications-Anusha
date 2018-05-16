using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HealthIS.Apps.MVC.Models;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;

namespace HealthIS.Apps.MVC.Controllers
{
    public class Components : Controller
    {
        public static string getPageTitle = "";
        public static string getMetaDesc = "";

        public ActionResult FacultyDirectoryCare(string itemId, string key, string value, string type, string movementTo)
        {
            FacultyDirectoryCare facultyList = new FacultyDirectoryCare();
            facultyList.rendering = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering;
            facultyList.item = facultyList.rendering.Item;
            facultyList.pageItem = PageContext.Current.Item;
            facultyList.isDatasourceNull = true;
            facultyList.allFacultyData = "";

            try
            {
                Item item = facultyList.item.Database.GetItem(facultyList.rendering.DataSource);
                facultyList.pageItem = PageContext.Current.Item;
                facultyList.isDatasourceNull = (item != null ? false : true);

                if (facultyList.isDatasourceNull)
                {
                    throw new ArgumentNullException("args.Item");
                }
                else
                {
                }
            }
            catch
            {
                facultyList.isDatasourceNull = true;
            }

            // If page is xEditor
            if (Sitecore.Context.PageMode.IsExperienceEditor)
            {
                // Create new branch
                if (!String.IsNullOrEmpty(itemId) && type == "Add New Branch" && !String.IsNullOrEmpty(key))
                {
                    Item parent = facultyList.item.Database.GetItem(Sitecore.Data.ID.Parse(itemId));
                    string templatePath = "/sitecore/templates/Branches/Faculty Directory Care/Faculty Directory DB Section";
                    if (key == "DB Section")
                    {
                        templatePath = "/sitecore/templates/Branches/Faculty Directory Care/Faculty Directory DB Section";
                    }
                    else if (key == "Input Section")
                    {
                        templatePath = "/sitecore/templates/Branches/Faculty Directory Care/Faculty Directory Input Section";
                    }
                    string itemName = parent.HasChildren ? facultyList.pageItem.Name + "_Section" + parent.Children.Count().ToString() : facultyList.pageItem.Name + "_Section";
                    BranchItem branch = facultyList.item.Database.GetItem(templatePath);
                    parent.Add(itemName, branch);
                }

                // Add new faculty into the list
                if (!String.IsNullOrEmpty(key) && type == "Add New Faculty")
                {
                    Item item = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(itemId.Trim()));
                    if (key == "User Input")
                    {
                        Item parent = facultyList.item.Database.GetItem(Sitecore.Data.ID.Parse(itemId));
                        string itemName = parent.HasChildren ? parent.Name + "_Input_View" + parent.Children.Count().ToString() : parent.Name + "_Input_View";
                        TemplateItem template = facultyList.item.Database.GetTemplate("User Defined2/Component Template/Faculty Directory Care/Faculty Directory Input View");
                        parent.Add(itemName, template);
                    }
                    else
                    {
                        NameValueListField data = item.Fields["Faculty List"];
                        System.Collections.Specialized.NameValueCollection nameValueCollection = data.NameValues;

                        using (new Sitecore.SecurityModel.SecurityDisabler())
                        {
                            item.Editing.BeginEdit();

                            if (String.IsNullOrEmpty(item.Fields["Faculty List"].Value))
                            {
                                item.Fields["Faculty List"].Value = key + "=;dump;";
                            }
                            else
                            {
                                item.Fields["Faculty List"].Value = item.Fields["Faculty List"].Value + "&" + key + "=;dump;";
                            }

                            item.Editing.EndEdit();
                            item.Editing.AcceptChanges();
                            Sitecore.Data.Managers.ItemManager.SaveItem(item);
                        }
                    }
                }

                // Remove faculty in the list
                if (!String.IsNullOrEmpty(key) && type == "Remove Faculty")
                {
                    Item item = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(itemId.Trim()));
                    NameValueListField data = item.Fields["Faculty List"];
                    System.Collections.Specialized.NameValueCollection nameValueCollection = data.NameValues;

                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        item.Editing.BeginEdit();
                        nameValueCollection.Remove(key);
                        data.NameValues = nameValueCollection;
                        item.Editing.EndEdit();
                        item.Editing.AcceptChanges();
                        Sitecore.Data.Managers.ItemManager.SaveItem(item);
                    }
                }

                // Sort Faculty List
                if (!String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(movementTo) && type == "Sort Faculty")
                {
                    Item item = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(itemId.Trim()));
                    NameValueListField data = item.Fields["Faculty List"];
                    System.Collections.Specialized.NameValueCollection nameValueCollection = data.NameValues;

                    var originalData = item.Fields["Faculty List"].Value;
                    var splitEach = originalData.Split('&');

                    int index = 0;
                    int selectedIndex = 0;
                    int targetIndex = 0;
                    foreach (string eachFaculty in splitEach)
                    {
                        if (eachFaculty.Contains(key + "="))
                        {
                            selectedIndex = index;
                            break;
                        }
                        index++;
                    }

                    if (movementTo == "Move Up")
                    {
                        targetIndex = selectedIndex - 1;
                    }
                    else if (movementTo == "Move Down")
                    {
                        targetIndex = selectedIndex + 1;
                    }

                    try
                    {
                        originalData = originalData.Replace(splitEach[selectedIndex], "ReplaceData=Data")
                                .Replace(splitEach[targetIndex], splitEach[selectedIndex])
                                .Replace("ReplaceData=Data", splitEach[targetIndex]);

                        using (new Sitecore.SecurityModel.SecurityDisabler())
                        {
                            item.Editing.BeginEdit();
                            item.Fields["Faculty List"].Value = originalData;
                            item.Editing.EndEdit();
                            item.Editing.AcceptChanges();
                            Sitecore.Data.Managers.ItemManager.SaveItem(item);
                        }
                    }
                    catch
                    {
                    }
                }

                //
                // Add multiple faculty in a section
                //
                if (!String.IsNullOrEmpty(itemId) && !String.IsNullOrEmpty(value) && type == "Add Multiple Faculty")
                {
                    value = value.Trim();
                    var ids = value.Split(',');
                    int isNumeric;

                    if (ids.Length > 0)
                    {
                        Item item = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(itemId.Trim()));
                        foreach (string id in ids)
                        {
                            if (int.TryParse(id, out isNumeric))
                            {
                                using (new Sitecore.SecurityModel.SecurityDisabler())
                                {
                                    item.Editing.BeginEdit();
                                    try
                                    {
                                        if (String.IsNullOrEmpty(item.Fields["Faculty List"].Value))
                                        {
                                            item.Fields["Faculty List"].Value = id + "=;dump;";
                                        }
                                        else
                                        {
                                            item.Fields["Faculty List"].Value = item.Fields["Faculty List"].Value + "&" + id + "=;dump;";
                                        }
                                    }
                                    finally
                                    {
                                        item.Editing.EndEdit();
                                        item.Editing.AcceptChanges();
                                        Sitecore.Data.Managers.ItemManager.SaveItem(item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return PartialView(facultyList);
        }

        public ActionResult FacultyDirectoryCareProfile()
        {
            FacultyDirectoryCareProfile facultyProfile = new FacultyDirectoryCareProfile();
            facultyProfile.rendering = Sitecore.Mvc.Presentation.RenderingContext.CurrentOrNull.Rendering;
            facultyProfile.pageItem = PageContext.Current.Item;

            try
            {
                // Set datasource if it is empty
                if (String.IsNullOrEmpty(facultyProfile.rendering.DataSource))
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        //Get current rendering item
                        Sitecore.Layouts.RenderingReference[] renderings = facultyProfile.pageItem.Visualization.GetRenderings(Sitecore.Context.Device, true).Where(r => r.RenderingID == facultyProfile.rendering.RenderingItem.ID).ToArray();

                        // Get the layout definitions and the device
                        Sitecore.Data.Fields.LayoutField layoutField = new Sitecore.Data.Fields.LayoutField(facultyProfile.pageItem.Fields[Sitecore.FieldIDs.FinalLayoutField]);
                        Sitecore.Layouts.LayoutDefinition layoutDefinition = Sitecore.Layouts.LayoutDefinition.Parse(layoutField.Value);
                        Sitecore.Layouts.DeviceDefinition deviceDefinition = layoutDefinition.GetDevice(Sitecore.Context.Device.ID.ToString());
                        foreach (Sitecore.Layouts.RenderingReference rendering in renderings)
                        {
                            // Update the renderings datasource value as current item path 
                            deviceDefinition.GetRendering(rendering.RenderingID.ToString()).Datasource = facultyProfile.pageItem.ID.ToString();
                            // Save the layout changes
                            facultyProfile.pageItem.Editing.BeginEdit();
                            layoutField.Value = layoutDefinition.ToXml();
                            facultyProfile.pageItem.Editing.EndEdit();
                        }
                    }
                }

                Sitecore.Marketing.Wildcards.WildcardTokenizedString wts = Sitecore.Marketing.Wildcards.WildcardManager.Provider.GetWildcardUrl(facultyProfile.PageItem, Sitecore.Context.Site);
                System.Collections.Specialized.NameValueCollection tokens = wts.FindTokenValues(Request.Path.ToLower().Replace(' ', '-'));
                facultyProfile.strPID = tokens["%Person_ID%"];
                if (facultyProfile.strPID != null) { facultyProfile.validId = Int32.TryParse(facultyProfile.strPID, out facultyProfile.personID); }
                if (facultyProfile.validId && !Sitecore.Context.PageMode.IsExperienceEditorEditing)
                {
                    facultyProfile.person = new HealthIS.Apps.FacultyDirectory.Profile(facultyProfile.personID, HealthIS.Apps.FacultyDirectory.Profile.ProfileType.Clinical, HealthIS.Apps.FacultyDirectory.Profile.ProfileParts.All);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.SingleWarn("Error from faculty profile page at " + Request.Url.Host + " - " + ex.Message, this);
                facultyProfile.person = null;
            }
            return PartialView(facultyProfile);
        }
    }
}