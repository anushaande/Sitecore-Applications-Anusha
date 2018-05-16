using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Workflows;
using Sitecore.Diagnostics;

namespace HealthIS.Apps
{
    public class Util
    {
        public const string _defaultFromAddress = "Support@health.usf.edu";
        private const string _smtpHost = "smtp.hscnet.hsc.usf.edu";
        private const int _smtpPort = 25;
        private string _dsTestItem = "{814BE2AA-B459-46E2-94C7-F11AC2A49C71}";

        public static string testFunction()
        {
            return "Boom Head Shot";
        }

        public static bool isOnProduction()
        {
            string server = System.Environment.MachineName.ToLower();
            return (server.IndexOf("webblade8") != -1 || server.IndexOf("hsccm2") != -1 || server.IndexOf("sitecoreqa") != -1 || server.IndexOf("sitecoreprod") != -1);
        }

        public static OracleConnection getDBConnection()
        {
            string strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbdev2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBPREP)(SERVER=DEDICATED)));User Id=sitecore_web;Password=Garbl3ph!em;";
            if (isOnProduction())
            {
                strConnect = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=dbprod2.hscnet.hsc.usf.edu)(PORT=1522)))(CONNECT_DATA=(SID=DBAFRZN)(SERVER=DEDICATED)));User Id=sitecore_web;Password=Garbl3ph!em;";
            }

            OracleConnection oraCN = new OracleConnection(strConnect);
            return oraCN;
        }

        public static string getParameter(HttpRequest Request, string paramName)
        {
            string ret = "";

            try
            {
                ret =  Request.QueryString[paramName].ToString();
            }
            catch (Exception) { }

            if (ret == "")
            {
                try
                {
                    ret = Request.Form[paramName].ToString();
                }
                catch (Exception) { }
            }
            return ret;
        }

        public static string getItemNameAtLevel(int index)
        {
            string ret = "Error:";
            try
            {
                List<Item> parents = new List<Item>();

                Item myItem = Sitecore.Context.Item;
                int i = 0;

                while (myItem != null && myItem.Name != "content" && i < 115)
                {
                    parents.Add(myItem);
                    myItem = myItem.Parent;
                    i++;
                }

                if (i <= 0 || i > 115)
                {
                    ret += "Couldn't find root";
                }else if(parents.Count < index){
                    ret += "Object less than index in tree";
                }else{
                    ret = parents[parents.Count - index].Name;
                }
                
                
                /*Item myItem = Sitecore.Context.Item;
                Item parent = myItem;
                int i = 0;

                while (parent.Name != "Home" && i < 115)
                {
                    myItem = parent;
                    parent = myItem.Parent;
                    i++;
                }
                if (i > 0 && i < 115)
                {
                    ret = myItem.Name;
                }*/
            }
            catch (Exception)
            {
            }
            return ret;
        }

        public static ItemDetail getItemDetailAtLevel(int index)
        {
            ItemDetail ret = new ItemDetail();
            ret.Name = "Error:";
            try
            {
                List<Item> parents = new List<Item>();

                Item myItem = Sitecore.Context.Item;
                int i = 0;

                while (myItem != null /*&& myItem.Name != "content"*/ && i < 115)
                {
                    parents.Add(myItem);
                    myItem = myItem.Parent;
                    i++;
                }
                //remove sitecore
                parents.RemoveAt(parents.Count-1);
                //remove content so we are at home
                parents.RemoveAt(parents.Count-1);

                if (i <= 0 || i > 115)
                {
                    ret.Name += "Couldn't find root";
                }
                else if (parents.Count < index)
                {
                    ret.Name += "Object less than index in tree";
                }
                else
                {
                    Item desiredItem = parents[parents.Count - index];
                    ret.Name = desiredItem.Name;
                    ret.DisplayName = desiredItem.DisplayName;
                }

            }
            catch (Exception)
            {
            }
            return ret;
        }

        public class ItemDetail
        {
            public string Name;
            public string DisplayName;
            public ItemDetail()
            {
                Name = DisplayName = "";
            }
        }

        private class smtpSettings
        {
            public string host;
            public int port;
            public smtpSettings()
            {
                readWebConfig();
            }
            public void readWebConfig(){

                host = Sitecore.Configuration.Settings.GetSetting("MailServer",_smtpHost);

                try
                {
                    port = Int16.Parse(Sitecore.Configuration.Settings.GetSetting("MailServerPort"));
                }
                catch (Exception)
                {
                    port = _smtpPort;
                }
            }
        }

        public static Exception GetInnermostException(Exception ex)
        {
            Exception innermostEx = ex.InnerException == null ? ex : ex.InnerException;
            if (innermostEx.InnerException != null) { innermostEx = GetInnermostException(innermostEx); }
            return innermostEx;
        } 

        public static bool sendEmail(string toAddress, string subject, string message, bool isHTML = false)
        {
            return sendEmail(toAddress, subject, message, _defaultFromAddress, null, null, isHTML, null);
        }
        public static bool sendEmail(string toAddress, string subject, string message, string fromAddress, bool isHTML = false)
        {
            return sendEmail(toAddress, subject, message, fromAddress, null, null, isHTML, null);
        }
        public static bool sendEmail(string toAddress, string subject, string message, string fromAddress, List<System.Net.Mail.Attachment> attachments, bool isHTML = false)
        {
            return sendEmail(toAddress, subject, message, fromAddress, null, null, isHTML, attachments);
        }
        public static bool sendEmail(string toAddress, string subject, string message, string fromAddress, string CCAddresses, string BCCAddresses, bool isHTML = false)
        {
            return sendEmail(toAddress, subject, message, fromAddress, CCAddresses, BCCAddresses, isHTML, null);
        }
        public static bool sendEmail(string toAddress, string subject, string message, string fromAddress, string CCAddresses, string BCCAddresses, bool isHTML=false, List<System.Net.Mail.Attachment> attachments = null)
        {
            try
            {
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
                if (attachments != null)
                {
                    foreach (System.Net.Mail.Attachment a in attachments)
                    {
                        msg.Attachments.Add(a);
                    }
                }
                foreach (string adr in toAddress.Split(';'))
                {
                    msg.To.Add(adr.Trim());
                }
                msg.From = new System.Net.Mail.MailAddress(fromAddress);
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = isHTML;

                //Add carbon copies and blind carbon copies if they are defined.
                if (!String.IsNullOrEmpty(CCAddresses))
                {
                    foreach (string adr in CCAddresses.Split(';'))
                    {
                        msg.CC.Add(adr.Trim());
                    }
                }
                if (!String.IsNullOrEmpty(BCCAddresses))
                {
                    foreach (string adr in BCCAddresses.Split(';'))
                    {
                        msg.Bcc.Add(adr.Trim());
                    }
                }
                
                smtpSettings sets = new smtpSettings();
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(sets.host, sets.port);

                smtp.Send(msg);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static void redirectUserRole(object sender, EventArgs e)
        {
            
            Sitecore.Security.Accounts.UserRoles roles = Sitecore.Context.User.Roles;
            System.Collections.Generic.List<Sitecore.Security.Accounts.Role> myRoles = new List<Sitecore.Security.Accounts.Role>();

            foreach(Sitecore.Security.Accounts.Role rol in roles){
                if (rol.Domain.ToString() == "HSCNET")
                {
                    myRoles.Add(rol);
                }
            }


            if (myRoles.Count == 0)
            {
                return;
            }

            OracleConnection orCN = Util.getDBConnection();
            orCN.Open();

            OracleCommand orCmd = new OracleCommand("sitecore_user.sitecore.get_url_for_role", orCN);
            orCmd.CommandType = CommandType.StoredProcedure;
            orCmd.Parameters.Add("p_role", OracleDbType.Varchar2).Direction = ParameterDirection.Input;
            orCmd.Parameters["p_role"].Value = "hscnet\role";

            orCmd.Parameters.Add("p_site", OracleDbType.Varchar2).Direction = ParameterDirection.Output;

            orCmd.Dispose();
            orCN.Close();
            orCN.Dispose();


        }

        //Generates a name for an item based on its name+suffix+number depending on if there is already an child with that name.
        public static string getNextNameUnderFolder(Item itm, string suffix, string folderName)
        {
            string ret = "";

            ret = itm.Name + suffix;
            string tname = ret;
            List<Item> rFolder = itm.Children.Where(x => x.TemplateName == "Resources Folder").ToList();
            if (rFolder.Count > 0)
            {
                foreach (Item i in rFolder[0].Children)
                {
                    if (!String.IsNullOrEmpty(folderName) && 
                        ((Sitecore.Data.ID.IsID(folderName) && FormatID(i.ID.Guid.ToString()) == FormatID(folderName))
                            || (!Sitecore.Data.ID.IsID(folderName) && i.Name.Equals(folderName))))//is folder to iterate through
                    {
                        for (int j = 1; j < 25; j++)
                        {
                            foreach (Item folderChild in i.Children)
                            {

                                if (folderChild.Name.Equals(tname))
                                {
                                    tname = null;
                                    break;
                                }
                            }
                            if (tname != null)
                            {
                                break;
                            }
                            tname = ret + "" + j;
                        }
                        break;
                    }
                    else
                    {//go deeper
                        foreach (Item iChild in i.Children)
                        {
                            if (!String.IsNullOrEmpty(folderName) &&
                            ((Sitecore.Data.ID.IsID(folderName) && FormatID(iChild.ID.Guid.ToString()) == FormatID(folderName))
                                || (!Sitecore.Data.ID.IsID(folderName) && iChild.Name.Equals(folderName))))//is folder to iterate through
                            {
                                for (int j = 1; j < 25; j++)
                                {
                                    foreach (Item folderChild in iChild.Children)
                                    {
                                        if (folderChild.Name.Equals(tname))
                                        {
                                            tname = null;
                                            break;
                                        }
                                    }
                                    if (tname != null)
                                    {
                                        break;
                                    }
                                    tname = ret + "" + j;
                                }
                                break;
                            }
                        }
                    }
                }
                ret = tname;
            }
            return ret;
        }

        //Generates a name for an item based on its name+suffix+number depending on if there is already an child with that name.
        public static string getNextName(Item itm, string suffix)
        {
            string ret = "";

            ret = itm.Name + suffix;
            string tname = ret;
            List<Item> rFolder = itm.Children.Where(x => x.TemplateName == "Resources Folder").ToList();
            if (rFolder.Count > 0)
            {
                for (int j = 1; j < 25; j++)
                {
                    for (int i = 0; i < rFolder[0].Children.Count; i++)
                    {
                        Item child = rFolder[0].Children[i];
                        if (child.Name.Equals(tname))
                        {
                            tname = null;
                            break;
                        }
                    }
                    if (tname != null)
                    {
                        break;
                    }
                    tname = ret + "" + j;
                }
                ret = tname;
            }

            return ret;
        }

        private static Item GetResourceFolder(Item myItem)
        {
            Item rFolder = null;
            List<Item> rFolders = myItem.Children.Where(x => x.TemplateName == "Resources Folder").ToList();
            if (rFolders.Count < 1)//no resource folder - create new
            {
                TemplateItem folderTemp = myItem.Database.Templates["User Defined2/Component Template/Resources Folder"];
                string rFolderName = myItem.Name + "_Resources"; // prepend myItem's name & underscore to "Resources" to give rFolder a unique name
                myItem.Editing.BeginEdit();
                rFolder = myItem.Add(rFolderName, folderTemp);
                myItem.Editing.EndEdit();
                //myItem.Editing.AcceptChanges();

                try
                {
                    rFolder.Editing.BeginEdit();
                    // Assign values to the fields of the new item
                    //rFolder.Fields["__"].Value = "NewValue222";
                    //rFolder.Locking.Unlock();
                    rFolder["__lock"] = "<r owner=\"" + Sitecore.Context.User.Name + "\" date=\"" + DateTime.Now.ToString("yyyyMMddTHHmmss") + "\" />";
                    rFolder["__lock"] = "<r />";

                    rFolder.Appearance.Sortorder = 0;  // set rFolder's sortorder value to 0

                    // End editing will write the new values back to the Sitecore
                    // database (It's like commit transaction of a database)
                    rFolder.Editing.EndEdit();
                }
                catch (System.Exception ex)
                {
                    // Log the message on any failure to sitecore log
                    Sitecore.Diagnostics.Log.Error("Could not update item " + rFolder.Paths.FullPath + ": " + ex.Message, myItem);

                    // Cancel the edit (not really needed, as Sitecore automatically aborts
                    // the transaction on exceptions, but it wont hurt your code)
                    rFolder.Editing.CancelEdit();
                }
            }    
            else
            {
                rFolder = rFolders[0];
            }
            return rFolder;
        }

        //Creates a datasource item under the current item and assigns its id to the item's datasource field.  Returns Null if any problems
        public static Item createDatasourceItem(string dataTemplatePath, string placeholderValue, string suffix)
        {
            Item myDataSourceItem = null;
            //need to create the new datasource item under the current context item
            Item myItem = Sitecore.Context.Item;

            //get new name under current item with number appended onto it.
            string newName = getNextName(myItem, suffix);
            if (newName == null)
                return null;


            //Get rendering reference for the new sublayout based on the keyed rendering reference name
            DeviceItem dev = Sitecore.Context.Device;
            RenderingReference[] refs = Sitecore.Context.Item.Visualization.GetRenderings(dev, true);
            RenderingReference mref = null;
            for (int i = 0; i < refs.Length; i++)
            {
                RenderingReference tref = refs[i];

                if (!String.IsNullOrEmpty(tref.RenderingItem.DataSource) && tref.RenderingItem.DataSource == placeholderValue)
                {
                    mref = tref;
                    break;
                }
            }
            if (mref == null)
            {
                return null;
            }

            //Get layout definition and find rendering redefinition so you can update the datasource name
            LayoutDefinition ld = LayoutDefinition.Parse(Sitecore.Data.Fields.LayoutField.GetFieldValue(Sitecore.Context.Item.Fields[Sitecore.FieldIDs.LayoutField]));
            DeviceDefinition dd = null;
            for (int i = 0; i < ld.Devices.Count; i++)
            {
                DeviceDefinition dtemp = (DeviceDefinition)ld.Devices[i];
                if (dtemp != null && dtemp.ID.ToString() == dev.ID.ToString())
                {
                    dd = dtemp;
                }
            }
            if (dd == null)
            {
                return null;
            }

            RenderingDefinition rd = null;
            for (int i = 0; i < dd.Renderings.Count; i++)
            {
                RenderingDefinition rtemp = (RenderingDefinition)dd.Renderings[i];
                //get the next sublayout rendering without a datasource
                if (mref.RenderingID.ToString() == rtemp.ItemID.ToString() && string.IsNullOrEmpty(rtemp.Datasource))
                {
                    rd = rtemp;
                    break;
                }
            }
            if (rd == null)
            {
                return null;
            }

            //set datasource, save and close
            Item resourceFolder = GetResourceFolder(myItem);
            resourceFolder.Editing.BeginEdit();
            myItem.Editing.BeginEdit();

            //check if template is branch or common template type
            if (dataTemplatePath.Contains("Branches"))
            {
                BranchItem branch = myItem.Database.GetItem(dataTemplatePath);
                myDataSourceItem = resourceFolder.Add(newName, branch);
            }
            else
            {
                TemplateItem template = myItem.Database.GetTemplate(dataTemplatePath);
                myDataSourceItem = resourceFolder.Add(newName, template);
            }

            rd.Datasource = myDataSourceItem.ID.ToString();
            myItem[Sitecore.FieldIDs.LayoutField] = ld.ToXml();
            myItem.Editing.EndEdit();
            resourceFolder.Editing.EndEdit();
            myItem.Editing.AcceptChanges();
            resourceFolder.Editing.AcceptChanges();

            return myDataSourceItem;
        }

        public static string RemoveDeprecatedDataSources(Item item, string deviceID, string dsID)
        {
            string s = "used DS IDs: ";
            Item rFolder = GetResourceFolder(item);
            DeviceItem device = item.Database.Resources.Devices[deviceID];
            Sitecore.Data.Fields.LayoutField layoutField = new Sitecore.Data.Fields.LayoutField(item.Fields[Sitecore.FieldIDs.FinalLayoutField]);
            LayoutDefinition layoutDefinition = LayoutDefinition.Parse(layoutField.Value);
            DeviceDefinition deviceDefinition = layoutDefinition.GetDevice(device.ID.ToString());
            List<string> usedDataSourceIDs = new List<string>();
            foreach (RenderingDefinition rd in deviceDefinition.Renderings)
            {
                if (!String.IsNullOrEmpty(rd.Datasource))
                {
                    usedDataSourceIDs.Add(rd.Datasource);
                    s += rd.Datasource + " || ";
                }
            }

            s += "toRemove: ";
            
            foreach (Item child in rFolder.Children.Where(x => !usedDataSourceIDs.Contains(FormatID(x.ID.ToString()))).ToList())
            {
                if (FormatID(child.ID.ToString()) != dsID)
                {
                    s += child.ID + " " + child.Name + " || ";
                    child.Editing.BeginEdit();
                    child.Delete();
                    child.Editing.EndEdit();
                    child.Editing.AcceptChanges();
                }
            }
            return s;
        }

        public static string FormatID(string og)//formats ID to be same for comparing
        {
            return og.Contains('{') ? og : "{" + og.ToUpper() + "}";
        }

        public static string AddItemToDataSource(string itemID, string dataTemplatePath, string suffix, string dbName, string dsFolderName)
        {
            Item newItem = null;
            string msg = "";
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    if (Sitecore.Data.ID.IsID(itemID))
                    {
                        Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase(dbName);
                        Item item = db.GetItem(Sitecore.Data.ID.Parse(itemID));
                        if (item != null)
                        {
                            TemplateItem template = db.GetTemplate(dataTemplatePath);
                            if (template != null) // template is common template type
                            {
                                //get new name under current item with number appended onto it.
                                string newName = getNextNameUnderFolder(item, suffix, dsFolderName);
                                msg += "newName: " + newName + " || ";
                                if (newName == null)
                                    return null;
                                msg += "ds to find: " + dsFolderName + " || ";
                                if (Sitecore.Data.ID.IsID(dsFolderName))
                                {
                                    int so = 400;
                                    Item dsFolder = Sitecore.Context.Database.GetItem(dsFolderName);
                                    if (dsFolder.Children.Count > 0)
                                    {
                                        so = dsFolder.Children.Last().Appearance.Sortorder;
                                    }
                                    msg += "sort order: " + so + " || ";
                                    newItem = dsFolder.Add(newName, template);
                                    using (new EditContext(newItem))
                                    {
                                        newItem.Appearance.Sortorder = so + 100;
                                    }
                                }
                                else
                                {
                                    //find datasource folder to add to
                                    foreach (Item i in item.Children)
                                    {
                                        if (i.TemplateName.ToLower().Equals("Resources Folder") && (i.Name.ToLower().Equals(item.Name + "_resources") || i.Name.ToLower().Equals("resources")))
                                        {
                                            msg += "rChild: ";
                                            foreach (Item rChild in i.Children)
                                            {
                                                msg += rChild.Name + " | ";
                                                if (Sitecore.Data.ID.IsID(dsFolderName) && FormatID(rChild.ID.Guid.ToString()) == FormatID(dsFolderName)
                                                    || (!Sitecore.Data.ID.IsID(dsFolderName) && rChild.Name.ToLower().Equals(dsFolderName.ToLower())))
                                                {
                                                    msg += "dsFolder found! || ";
                                                    //add new item, save and close
                                                    i.Editing.BeginEdit();
                                                    int so = 400;
                                                    if (rChild.Children.Count > 0)
                                                    {
                                                        so = rChild.Children.Last().Appearance.Sortorder;
                                                    }
                                                    msg += "sort order: " + so + " || ";
                                                    newItem = rChild.Add(newName, template);
                                                    using (new EditContext(newItem))
                                                    {
                                                        newItem.Appearance.Sortorder = so + 100;
                                                    }
                                                    i.Editing.EndEdit();
                                                    i.Editing.AcceptChanges();
                                                    break;
                                                }
                                                else
                                                {
                                                    msg += "Going deeper... || ";
                                                    //msg += "{" + rChild.Name.ToLower() + " != " + dsFolderName + "} || ";
                                                    bool found = false;
                                                    foreach (Item subChild in rChild.Children)
                                                    {
                                                        if (Sitecore.Data.ID.IsID(dsFolderName) && FormatID(subChild.ID.Guid.ToString()) == FormatID(dsFolderName)
                                                            || (!Sitecore.Data.ID.IsID(dsFolderName) && subChild.Name.ToLower().Equals(dsFolderName.ToLower())))
                                                        {
                                                            msg += "deeper dsFolder found! || ";
                                                            //add new item, save and close
                                                            i.Editing.BeginEdit();
                                                            int so = 400;
                                                            if (subChild.Children.Count > 0)
                                                            {
                                                                so = subChild.Children.Last().Appearance.Sortorder;
                                                            }
                                                            msg += "sort order: " + so + " || ";
                                                            newItem = subChild.Add(newName, template);
                                                            using (new EditContext(newItem))
                                                            {
                                                                newItem.Appearance.Sortorder = so + 100;
                                                            }
                                                            i.Editing.EndEdit();
                                                            i.Editing.AcceptChanges();
                                                            found = true;
                                                            break;
                                                        }
                                                    }
                                                    if (found) break;
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }                                
                            }
                            else if (dataTemplatePath.Contains("Branches")) // template is branch
                            {
                                BranchItem branch = item.Database.GetItem(dataTemplatePath);
                                if (branch != null)
                                {
                                    //get new name under current item with number appended onto it.
                                    string newName = getNextNameUnderFolder(item, suffix, dsFolderName);
                                    msg += "newName: " + newName + " || ";
                                    if (newName == null)
                                        return null;
                                    msg += "ds to find: " + dsFolderName + " || ";
                                    if (Sitecore.Data.ID.IsID(dsFolderName))
                                    {
                                        int so = 400;
                                        Item dsFolder = Sitecore.Context.Database.GetItem(dsFolderName);
                                        if (dsFolder.Children.Count > 0)
                                        {
                                            so = dsFolder.Children.Last().Appearance.Sortorder;
                                        }
                                        msg += "sort order: " + so + " || ";
                                        newItem = dsFolder.Add(newName, branch);
                                        using (new EditContext(newItem))
                                        {
                                            newItem.Appearance.Sortorder = so + 100;
                                        }
                                    }
                                    else
                                    {
                                        //find datasource folder to add to
                                        foreach (Item i in item.Children)
                                        {
                                            if (i.TemplateName.ToLower().Equals("Resources Folder") && (i.Name.ToLower().Equals(item.Name + "_resources") || i.Name.ToLower().Equals("resources")))
                                            {
                                                msg += "rChild: ";
                                                foreach (Item rChild in i.Children)
                                                {
                                                    msg += rChild.Name + " | ";
                                                    if (Sitecore.Data.ID.IsID(dsFolderName) && FormatID(rChild.ID.Guid.ToString()) == FormatID(dsFolderName)
                                                        || (!Sitecore.Data.ID.IsID(dsFolderName) && rChild.Name.ToLower().Equals(dsFolderName.ToLower())))
                                                    {
                                                        msg += "dsFolder found! || ";
                                                        //add new item, save and close
                                                        i.Editing.BeginEdit();
                                                        int so = 400;
                                                        if (rChild.Children.Count > 0)
                                                        {
                                                            so = rChild.Children.Last().Appearance.Sortorder;
                                                        }
                                                        msg += "sort order: " + so + " || ";
                                                        newItem = rChild.Add(newName, branch);
                                                        using (new EditContext(newItem))
                                                        {
                                                            newItem.Appearance.Sortorder = so + 100;
                                                        }
                                                        i.Editing.EndEdit();
                                                        i.Editing.AcceptChanges();
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        msg += "Going deeper... || ";
                                                        //msg += "{" + rChild.Name.ToLower() + " != " + dsFolderName + "} || ";
                                                        bool found = false;
                                                        foreach (Item subChild in rChild.Children)
                                                        {
                                                            if (Sitecore.Data.ID.IsID(dsFolderName) && FormatID(subChild.ID.Guid.ToString()) == FormatID(dsFolderName)
                                                                || (!Sitecore.Data.ID.IsID(dsFolderName) && subChild.Name.ToLower().Equals(dsFolderName.ToLower())))
                                                            {
                                                                msg += "deeper dsFolder found! || ";
                                                                //add new item, save and close
                                                                i.Editing.BeginEdit();
                                                                int so = 400;
                                                                if (subChild.Children.Count > 0)
                                                                {
                                                                    so = subChild.Children.Last().Appearance.Sortorder;
                                                                }
                                                                msg += "sort order: " + so + " || ";
                                                                newItem = subChild.Add(newName, branch);
                                                                using (new EditContext(newItem))
                                                                {
                                                                    newItem.Appearance.Sortorder = so + 100;
                                                                }
                                                                i.Editing.EndEdit();
                                                                i.Editing.AcceptChanges();
                                                                found = true;
                                                                break;
                                                            }
                                                        }
                                                        if (found) break;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    msg += "Template not found. ";
                                }
                            }
                            else
                            {
                                msg += "Template not found. ";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg += "ERROR: " + ex.Message;
                return msg;
            }
            if (newItem != null) { return newItem.ID.ToString(); }
            else return msg;
        }

        public static string UpdateItemName(string itemID, string itemName)
        {
            string msg = "";
            try
            {
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    if (Sitecore.Data.ID.IsID(itemID))
                    {
                        Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase("master");
                        if (db != null)
                        {
                            Item item = db.GetItem(Sitecore.Data.ID.Parse(itemID));
                            if(item !=null)
                            {
                                using (new EditContext(item))
                                {
                                    //char[] arr = itemName.Where(c => char.IsLetter(c)).ToArray();
                                    //itemName = new string(arr);
                                    System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 -]");
                                    itemName = rgx.Replace(itemName, "_");
                                    item.Name = itemName;
                                }
                            }
                            else
                            {
                                msg += "Can't get item || ";
                            }
                        }
                        else
                        {
                            msg += "Can't get database || ";
                        }
                    }
                    else
                    {
                        msg += "itemID is not a valid ID || ";
                    }
                }
            }
            catch (Exception ex)
            {
                msg += "ERROR: " + ex.Message;
            }
            return msg;
        }

        public static Item UpdateDatasource(string itemID, string dataTemplatePath, string suffix, string dbName, string deviceID, string renderingUID)
        {
            Item myDataSourceItem = null;
            renderingUID = FormatID(renderingUID);
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                if (Sitecore.Data.ID.IsID(itemID))
                {
                    Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase(dbName);
                    Item item = db.GetItem(Sitecore.Data.ID.Parse(itemID));
                    if (item != null)
                    {
                        //get new name under current item with number appended onto it.
                        string newName = getNextName(item, suffix);
                        if (newName == null)
                            return null;

                        //set datasource, save and close
                        Item resourceFolder = GetResourceFolder(item);
                        resourceFolder.Editing.BeginEdit();

                        //check if template is branch or common template type
                        if (dataTemplatePath.Contains("Branches"))
                        {
                            BranchItem branch = item.Database.GetItem(dataTemplatePath);
                            myDataSourceItem = resourceFolder.Add(newName, branch);
                        }
                        else
                        {
                            TemplateItem template = item.Database.GetTemplate(dataTemplatePath);
                            myDataSourceItem = resourceFolder.Add(newName, template);
                        }

                        resourceFolder.Editing.EndEdit();
                        resourceFolder.Editing.AcceptChanges();

                        // Get the layout definitions and the device
                        DeviceItem device = db.Resources.Devices[deviceID];
                        Sitecore.Data.Fields.LayoutField layoutField = new Sitecore.Data.Fields.LayoutField(item.Fields[Sitecore.FieldIDs.FinalLayoutField]);
                        LayoutDefinition layoutDefinition = LayoutDefinition.Parse(layoutField.Value);
                        DeviceDefinition deviceDefinition = layoutDefinition.GetDevice(device.ID.ToString());
                        foreach (RenderingDefinition rd in deviceDefinition.Renderings)
                        {
                            // Update the renderings datasource value accordingly 
                            if (rd.UniqueId == renderingUID)
                            {
                                rd.Datasource = myDataSourceItem.ID.ToString();

                                // Save the layout changes
                                item.Editing.BeginEdit();
                                layoutField.Value = layoutDefinition.ToXml();
                                item.Editing.EndEdit();

                                RemoveDeprecatedDataSources(item, deviceID, myDataSourceItem.ID.ToString());
                                
                                break;
                            }
                        }
                    }
                }
            }
            return myDataSourceItem;
        }

        public static string GetWorkingState(IWorkflow wf)
        {
            foreach (var s in wf.GetStates())
            {
                if (s.DisplayName == "Working")
                {
                    return s.StateID;
                }
            }
            return null;
        }

        public static string GetFinalWFState(IWorkflow wf)
        {
            string state = "";
            foreach (var s in wf.GetStates())
            {
                if (s.FinalState)
                {
                    state = s.StateID;
                    break;
                }
            }
            return state;
        }

        public static person getFacultyBasicInfo(string person_id)
        {
            person person = new person();
            int num = 0;
            using (OracleConnection dbConnection = Util.getDBConnection())
            {
                dbConnection.Open();
                OracleCommand oracleCommand = new OracleCommand("research_dir_user.profile_views.sel_person_by_id", dbConnection);
                oracleCommand.CommandType = CommandType.StoredProcedure;
                oracleCommand.Parameters.Add("p_person_id", (OracleDbType)126, person_id, ParameterDirection.Input);
                oracleCommand.Parameters.Add("r_ret", (OracleDbType)121, ParameterDirection.Output);
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                oracleCommand.ExecuteNonQuery();
                DataSet dataSet = new DataSet();
                oracleDataAdapter.Fill(dataSet);
                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    num = 1;
                    person.last_name = dataRow["last_name"].ToString().Trim();
                    person.first_name = dataRow["first_name"].ToString().Trim();
                    person.person_id = dataRow["person_id"].ToString().Trim();
                    person.hscid = dataRow["hscnet_id"].ToString().Trim();
                }
                dataSet.Dispose();
                oracleDataAdapter.Dispose();
                oracleCommand.Dispose();
                dbConnection.Close();
                dbConnection.Dispose();
            }
            if (num == 0)
                return (person)null;
            return person;
        }

        public static void AddJS(System.Web.UI.Page page, string url, string receivingControlId = "JsPlaceholder")
        {
            Util.AddJS(page, url, new System.Collections.Specialized.NameValueCollection(), receivingControlId);
        }

        public static void AddJS(System.Web.UI.Page page, string url, System.Collections.Specialized.NameValueCollection nvc, string receivingControlId = "JsPlaceholder")
        {
            if (page == null || string.IsNullOrEmpty(url) || (nvc == null || string.IsNullOrEmpty(receivingControlId)) || HttpContext.Current.Items.Contains(url))
                return;
            HttpContext.Current.Items[url] = 1;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string index in nvc.AllKeys)
                stringBuilder.AppendFormat(" {0}={1}", index, nvc[index]);
            string str = string.Format("<script src=\"{0}\" {1} type=\"text/javascript\"></script>", url, stringBuilder);
            var lit = new System.Web.UI.WebControls.Literal();
            lit.Text = str;
            page.FindControl(receivingControlId).Controls.Add(lit);
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

        public class sublayoutInfo
        {
            public RenderingDefinition rd;
            public LayoutDefinition ld;

            public sublayoutInfo()
            {
                this.rd = (RenderingDefinition)null;
                this.ld = (LayoutDefinition)null;
            }
        }

        public static void LogError(string msg, Exception ex, object o)
        {
            Sitecore.Security.Accounts.User activeUser = Sitecore.Security.Authentication.AuthenticationManager.GetActiveUser();
            if (activeUser != null)
            {
                if (activeUser.Profile.UserName.Contains("dkpainter") || activeUser.Profile.UserName.Contains("iprefontaine"))//only show alerts for us || activeUser.Profile.UserName.Contains("jihyunkim")
                {
                    Sitecore.Context.ClientPage.ClientResponse.Alert(msg);
                }
            }
            if (o == null) { o = activeUser; }
            if (ex != null && o != null)
            {
                Sitecore.Diagnostics.Log.Error(msg, ex, o);
            }
        }

        // called when creating a new child page item; checks itemName against currently existing children under parent item (parentItemID)
        public static bool IsADuplicateItemName(string parentItemID, string itemName)
        {
            if (String.IsNullOrEmpty(parentItemID) || String.IsNullOrEmpty(itemName))
                return false;  // missing input
            Item parentItem = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(parentItemID);
            if (parentItem != null)
            {
                Sitecore.Collections.ChildList allChildren = parentItem.Children;
                if (allChildren != null)
                {
                    itemName = itemName.ToLower();
                    foreach (Item child in allChildren)
                    {
                        string chName = child.Name.ToLower();
                        if (chName.Equals(itemName)) { return true; }  // itemName given already exists
                    }
                }
                else { return false; } // parentItem has no children
            }
            else { return false; } // parentItem is null

            return false;  // no duplicate found
        }

        // called when creating a new child page item; sets name, displayName and metaTitle information for new item
        public static void SetDisplayNameAndMetaTitle(Item item, string displayName, string metaTitle)
        {
            Assert.ArgumentNotNull((object)item, "item");
            if ((!String.IsNullOrEmpty(displayName)) || (!String.IsNullOrEmpty(metaTitle)))
            {
                item.Editing.BeginEdit();
                try
                {
                    // if displayName is not null && metaTitle is, then set displayName value for both fields
                    if ((!String.IsNullOrEmpty(displayName)) && String.IsNullOrEmpty(metaTitle))
                    {
                        item.Fields["__Display Name"].Value = displayName;
                        item.Fields["Meta Title"].Value = displayName;                    
                    }                    
                    else
                    {
                        item.Fields["__Display Name"].Value = displayName;
                        item.Fields["Meta Title"].Value = metaTitle; 
                    }
                    item.Editing.EndEdit();
                }
                catch (Exception ex)
                {
                    Log.Error("Could not update item " + item.Paths.FullPath + ": " + ex.Message, ex);
                    item.Editing.CancelEdit();
                }
            }
        }

        // checks itemPath to see if it is a phase 1 or phase 2 user defined page item
        public static bool isUserDefinedPageItem(string itemPath)
        {
            if (itemPath.StartsWith("/sitecore/templates/User Defined/") ||
                itemPath.StartsWith("/sitecore/templates/User Defined2/Page Template/"))
                return true;
            else return false;
        }
    }
}
