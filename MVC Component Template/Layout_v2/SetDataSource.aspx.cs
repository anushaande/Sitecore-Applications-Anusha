using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC
{
    public partial class SetDataSource : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod.ToString() == "POST" && !IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.Form["method"]))
                {
                    string itemID = null, 
                        templatePath = null, 
                        suffix = null, 
                        dbName = null,
                        deviceID = null, 
                        renderingUID = null,
                        dsFolderName = null,
                        itemName = null;
                    try
                    {
                        itemID = Request.Form["itemID"];
                        templatePath = Request.Form["templatePath"];
                        suffix = Request.Form["suffix"];
                        dbName = Request.Form["dbName"];
                        deviceID = Request.Form["deviceID"];
                        renderingUID = Request.Form["renderingUID"];
                        dsFolderName = Request.Form["dsFolderName"];
                        itemName = Request.Form["itemName"];
                    }
                    catch { }

                    switch (Request.Form["method"].ToString())
                    {                            
                        case "SetDataSource":
                            
                            if (itemID != null 
                                || templatePath != null 
                                || suffix != null 
                                || dbName != null
                                || deviceID != null
                                || renderingUID != null)
                            {
                                try
                                {
                                    SetDataSrc(itemID, templatePath, suffix, dbName, deviceID, renderingUID);
                                }
                                catch (Exception ex)
                                {
                                    Response.Write("Error processing request.\n" + ex.Message);
                                }
                            }
                            else
                            {
                                Response.Write("Error processing request. Not all parameters found.");
                            }
                            break;

                        case "AddItemToDataSource":
                            if (itemID != null
                                || templatePath != null
                                || suffix != null
                                || dbName != null
                                || dsFolderName != null)
                            {
                                try
                                {
                                    AddItemToDataSrc(itemID, templatePath, suffix, dbName, dsFolderName);
                                }
                                catch (Exception ex)
                                {
                                    Response.Write("Error processing request.\n" + ex.Message);
                                }
                            }
                            else
                            {
                                Response.Write("Error processing request. Not all parameters found.");
                            }
                            break;

                        case "UpdateItemName":
                            if (itemID != null
                                || itemName != null)
                            {
                                try
                                {
                                    UpdateItemName(itemID, itemName);
                                }
                                catch (Exception ex)
                                {
                                    Response.Write("Error processing request.\n" + ex.Message);
                                }
                            }
                            else
                            {
                                Response.Write("Error processing request. Not all parameters found.");
                            }
                            break;
                        default:
                            Response.Write("No method found.");
                            break;
                    }
                }
                else
                {
                    Response.Write("No method found.");
                }
                Response.End();
            }
        }

        private void SetDataSrc(string itemID, string templatePath, string suffix, string dbName, string deviceID, string renderingUID)
        {
            string msg = "";
            try
            {
                Sitecore.Data.Items.Item ds = HealthIS.Apps.Util.UpdateDatasource(itemID, templatePath, suffix, dbName, deviceID, renderingUID);
                msg = ds.ID.ToString() + " " + ds.Name.ToString();

            }
            catch (Exception ex)
            {
                msg = "ERROR:" + ex.Message + "\nStack: " + ex.StackTrace + "\n\n";
            }
            Response.Write(msg);
        }

        private void AddItemToDataSrc(string itemID, string templatePath, string suffix, string dbName, string dsFolderName = "")
        {
            string msg = "";
            try
            {
                //Sitecore.Data.Items.Item ds = HealthIS.Apps.Util.AddItemToDataSource(itemID, templatePath, suffix, dbName, dsFolderName);
                msg = HealthIS.Apps.Util.AddItemToDataSource(itemID, templatePath, suffix, dbName, dsFolderName);
            }
            catch (Exception ex)
            {
                msg += "SDS ERROR: " + ex.Message + "\nStack: " + ex.StackTrace + "\n\nItemID: " + itemID + "\nTemplate: " + templatePath + "\nSuffix: " + suffix + "\ndbName: " + dbName + "\ndsFolderName: " + dsFolderName + "\n\n";
            }
            Response.Write(msg);
        }

        private void UpdateItemName(string itemID, string itemName)
        {
            string msg = "";
            try
            {
                msg = HealthIS.Apps.Util.UpdateItemName(itemID, itemName);
            }
            catch (Exception ex)
            {
                msg += "ERROR: " + ex.Message + "\nStack: " + ex.StackTrace + "\n\nItemID: " + itemID + "\n\n";
            }
            Response.Write(msg);
        }
    }
}