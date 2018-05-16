using System;
using System.Collections.Specialized;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Globalization;
using Sitecore.Layouts;
using Sitecore.Links;
using Sitecore.Pipelines.HasPresentation;
using Sitecore.Resources;
using Sitecore.Shell.Applications.Dialogs.SortContent;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Sites;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;


namespace HealthIS.Apps.Commands
{
    public class UpdateItemField : Sitecore.Shell.Framework.Commands.Command
    {
        public override void Execute(Sitecore.Shell.Framework.Commands.CommandContext context)
        {
            if (context.Parameters != null && context.Parameters.Count > 0)
            {
                string id = context.Parameters["id"] ?? "";
                string fieldName = context.Parameters["fieldName"] ?? "";
                string fieldValue = context.Parameters["fieldValue"] ?? "";
                Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase("master");
                if (db != null)
                {
                    Item i = db.GetItem(id);
                    if (i != null && !String.IsNullOrEmpty(fieldName) && !String.IsNullOrEmpty(fieldValue))
                    {
                        using (new EditContext(i))
                        {
                            if (i.Fields[fieldName] != null && i.Fields[fieldName].CanWrite)
                            {
                                i.Fields[fieldName].SetValue(fieldValue, true);
                            }
                        }
                        //String refresh = String.Format("item:refreshchildren(id={0})", Sitecore.Context.Item.Parent.ID);
                        //Sitecore.Context.ClientPage.ClientResponse.Timer(refresh, 2);
                    }
                }
            }
        }
    }

    public class UpdateDataSource : Sitecore.Shell.Framework.Commands.Command
    {
        public override void Execute(Sitecore.Shell.Framework.Commands.CommandContext context)
        {
            if (context.Parameters != null && context.Parameters.Count > 0)
            {
                try
                {
                    string itemID = context.Parameters["itemID"] ?? "";
                    itemID = Util.FormatID(itemID);
                    string dsID = context.Parameters["dataSourceID"] ?? "";
                    dsID = Util.FormatID(dsID);
                    string renderingID = context.Parameters["renderingID"] ?? "";
                    renderingID = Util.FormatID(renderingID);
                    string deviceID = context.Parameters["deviceID"] ?? "";
                    deviceID = Util.FormatID(deviceID);
                    string dbName = context.Parameters["dbName"] ?? "";
                    Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase(dbName);
                    if (db != null)
                    {
                        Item i = db.GetItem(itemID);
                        try
                        {
                            string name = i.Name;
                        }
                        catch { }
                        if (i != null && Sitecore.Data.ID.IsID(renderingID) && Sitecore.Data.ID.IsID(dsID))
                        {
                            using (new EditContext(i))
                            {
                                Sitecore.Data.Fields.LayoutField layoutField = new Sitecore.Data.Fields.LayoutField(i.Fields[Sitecore.FieldIDs.LayoutField]);
                                Sitecore.Layouts.LayoutDefinition layoutDefinition = Sitecore.Layouts.LayoutDefinition.Parse(layoutField.Value);
                                Sitecore.Layouts.DeviceDefinition deviceDefinition = layoutDefinition.GetDevice(deviceID);
                                foreach (Sitecore.Layouts.RenderingDefinition rd in deviceDefinition.Renderings)
                                {
                                    if (HealthIS.Apps.Util.FormatID(rd.UniqueId) == renderingID)
                                    {
                                        rd.Datasource = HealthIS.Apps.Util.FormatID(dsID);
                                        layoutField.Value = layoutDefinition.ToXml();
                                    }
                                }
                            }
                        }
                        else
                        {
                            Exception ex = new Exception("Update Datasource Issue with id: " + itemID + " or dsID: " + dsID + " or i: " + i);
                            Util.LogError(ex.Message, ex, this);
                        }
                    }
                } 
                catch (Exception ex)
                {
                    Util.LogError(ex.Message, ex, this);
                }
            }
        }
    }

    // Recycle Bin Command
    public class OpenRecycleBin : Sitecore.Shell.Framework.Commands.Command
    {
        public override void Execute(Sitecore.Shell.Framework.Commands.CommandContext context)
        {
            Sitecore.Shell.Framework.Windows.RunShortcut(Sitecore.Data.ID.Parse("{A2CF861E-77AB-4317-A72B-2F33D942520E}"));            
        }
    }

    // Archive Specific Version Command
    public class ArchiveSpecificVersion : Sitecore.Shell.Framework.Commands.Command
    {
        public override void Execute(CommandContext context)
        {
            if (context.Items.Length != 1)
            return;
            Item obj = context.Items[0];
            NameValueCollection parameters = new NameValueCollection();
            parameters["id"] = obj.ID.ToString();
            parameters["language"] = obj.Language.ToString();
            parameters["version"] = obj.Version.ToString();
            Context.ClientPage.Start((object) this, "Run", parameters);
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (context.Items.Length != 1)
                return CommandState.Hidden;
            Item obj = context.Items[0];
            if (obj.Database.Archives.Count == 0 || !this.HasField(obj, FieldIDs.ArchiveDate))
                return CommandState.Hidden;
            if (obj.Appearance.ReadOnly || !obj.Access.CanWrite() || (Command.IsLockedByOther(obj) || !Command.CanWriteField(obj, FieldIDs.ArchiveDate)))
                return CommandState.Disabled;
            return base.QueryState(context);
        }

        protected void Run(ClientPipelineArgs args)
        {
            string index = args.Parameters["id"];
            string name = args.Parameters["language"];
            string str = args.Parameters["version"];
            Item obj = Context.ContentDatabase.Items[index, Language.Parse(name), Sitecore.Data.Version.Parse(str)];
            Error.AssertItemFound(obj);
            if (!SheerResponse.CheckModified())
                return;
            if (args.IsPostBack)
            {
                Context.ClientPage.SendMessage((object) this, "item:load(id=" + index + ",language=" + name + ",version=" + str + ")");
            }
            else
            {
                UrlString urlString = ResourceUri.Parse("control:ArchiveSpecificVersion").ToUrlString();
                obj.Uri.AddToUrlString(urlString);
                SheerResponse.ShowModalDialog(urlString.ToString(), true);
                args.WaitForPostBack();
            }
        }
    }

    // Represents New command
    public class New : Sitecore.Shell.Applications.WebEdit.Commands.WebEditCommand
    {
        public static string parentItemID = string.Empty;

        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull((object)context, "context");
            NameValueCollection parameters = new NameValueCollection();
            string str = context.Parameters["itemid"] ?? context.Parameters["id"];
            ItemUri queryString = ItemUri.ParseQueryString();
            if (string.IsNullOrEmpty(str) && queryString != (ItemUri)null)
                str = queryString.ItemID.ToString();
            parameters["itemid"] = str;
            string parameter = context.Parameters["language"];
            if (string.IsNullOrEmpty(parameter) && queryString != (ItemUri)null)
                parameter = queryString.Language.ToString();
            parameters["language"] = parameter;
            parameters["navigate"] = context.Parameters["navigate"];
            Context.ClientPage.Start((object)this, "Run", parameters);
        }

        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull((object)context, "context");
            if (WebUtil.GetQueryString("mode") == "preview" || context.Items.Length > 0 && !context.Items[0].Access.CanCreate())
                return CommandState.Disabled;
            return base.QueryState(context);
        }

        public virtual void ExecuteCommand(Item item, BranchItem branch, string name)
        {
            Assert.ArgumentNotNull((object)item, "item");
            Assert.ArgumentNotNull((object)branch, "branch");
            Assert.ArgumentNotNull((object)name, "name");
            string str = branch.InnerItem.Fields[FieldIDs.Command].Value;
            if (string.IsNullOrEmpty(str))
                return;
            Command command = CommandManager.GetCommand(str);
            if (command == null)
                return;
            CommandContext context = new CommandContext(item);
            if (Message.IsMessage(str))
            {
                Message message = Message.Parse((object)Context.ClientPage, str);
                context.Parameters.Add(message.Arguments);
            }
            context.Parameters.Add("name", name);
            command.Execute(context);
        }

        public virtual void PolicyBasedUnlock(Item item)
        {
            Assert.ArgumentNotNull((object)item, "item");
            Database database = Factory.GetDatabase("core");
            Assert.IsNotNull((object)database, "Database \"{0}\" not found", (object)"core");
            if (database.GetItem("/sitecore/system/Settings/Security/Policies/Page Editor/Keep Lock After Save") != null)
                return;
            try
            {
                item.Locking.Unlock();
            }
            catch (Exception ex)
            {
                Log.Warn("Item {0} couldn't be automatically unlocked, which was required by the policy-based locking.", ex, (object)this);
            }
        }

        protected void Run(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            parentItemID = args.Parameters["itemid"];
            Item itemNotNull = Sitecore.Client.GetItemNotNull(args.Parameters["itemid"], Language.Parse(args.Parameters["language"]));
            if (args.IsPostBack)
            {
                if (!args.HasResult)
                    return;
                string[] strArray = args.Result.Split(new[] { "||" }, StringSplitOptions.None);
                string id = strArray[0];
                string name = Uri.UnescapeDataString(strArray[1]);
                string displayName = Uri.UnescapeDataString(strArray[2]);
                string metaTitle = Uri.UnescapeDataString(strArray[3]);
                if (ShortID.IsShortID(id))
                    id = ShortID.Parse(id).ToID().ToString();
                BranchItem branch = Sitecore.Client.ContentDatabase.Branches[id, itemNotNull.Language];
                Assert.IsNotNull((object)branch, typeof(BranchItem));
                this.ExecuteCommand(itemNotNull, branch, name);
                Sitecore.Client.Site.Notifications.Disabled = true;
                Item obj = Context.Workflow.AddItem(name, branch, itemNotNull);
                Sitecore.Client.Site.Notifications.Disabled = false;
                if (obj == null)
                    return;
                this.PolicyBasedUnlock(obj);
                if (!HasPresentationPipeline.Run(obj) || !MainUtil.GetBool(args.Parameters["navigate"], true))
                {
                    WebEditCommand.Reload();
                }
                else
                {
                    UrlOptions defaultOptions = UrlOptions.DefaultOptions;
                    SiteContext site = SiteContext.GetSite(string.IsNullOrEmpty(args.Parameters["sc_pagesite"]) ? Sitecore.Web.WebEditUtil.SiteName : args.Parameters["sc_pagesite"]);
                    if (site == null)
                        return;
                    string url = string.Empty;
                    using (new SiteContextSwitcher(site))
                    {
                        using (new LanguageSwitcher(obj.Language))
                            url = LinkManager.GetItemUrl(obj, defaultOptions);
                    }
                    WebEditCommand.Reload(new UrlString(url));
                    SheerResponse.Alert("New page item created.");
                }
                HealthIS.Apps.Util.SetDisplayNameAndMetaTitle(obj, displayName, metaTitle);
            }
            else if (!itemNotNull.Access.CanCreate())
            {
                SheerResponse.Alert("You do not have permission to create an item here.");
            }
            else
            {
                UrlString urlString = ResourceUri.Parse("control:CustomAddMasterWebEdit").ToUrlString();
                itemNotNull.Uri.AddToUrlString(urlString);
                SheerResponse.ShowModalDialog(urlString.ToString(), "1200px", "700px", string.Empty, true);
                args.WaitForPostBack();
            }
        }
    }
    
    // Represents the AddMaster command
    public class AddMaster : Sitecore.Shell.Framework.Commands.Command
    {
        public static string parentItemID = string.Empty;
        
        public override void Execute(Sitecore.Shell.Framework.Commands.CommandContext context)
        {
            if (context.Items.Length != 1 || !context.Items[0].Access.CanCreate())
            return;
            Item obj = context.Items[0];
            NameValueCollection parameters = new NameValueCollection();
            parameters["Master"] = context.Parameters["master"];
            parameters["ItemID"] = obj.ID.ToString();
            parameters["Language"] = obj.Language.ToString();
            parameters["Version"] = obj.Version.ToString();
            Context.ClientPage.Start((object) this, "Add", parameters);
        }

        public override CommandState QueryState(CommandContext context)
        {
            Error.AssertObject((object) context, "context");
            if (context.Items.Length != 1)
            return CommandState.Hidden;
            if (!context.Items[0].Access.CanCreate())
            return CommandState.Disabled;
            return base.QueryState(context);
        }        

        protected void Add(ClientPipelineArgs args)
        {
            if (!SheerResponse.CheckModified())
            return;
            Item selectedTemplate = Context.ContentDatabase.GetItem(args.Parameters["Master"]);
            parentItemID = args.Parameters["itemid"];
            Item parent = Sitecore.Client.GetItemNotNull(args.Parameters["itemid"], Language.Parse(args.Parameters["language"]));
            if (selectedTemplate == null)
            SheerResponse.Alert(Translate.Text("Branch \"{0}\" not found.", (object) args.Parameters["Master"]));
            else if (selectedTemplate.TemplateID == TemplateIDs.CommandMaster)
            {
                string message = selectedTemplate["Command"];
            if (string.IsNullOrEmpty(message))
                return;
            Context.ClientPage.SendMessage((object) this, message);
            }
            else if (args.IsPostBack)
            {
                if (!args.HasResult)
                    return;
                if (parent == null)
                    SheerResponse.Alert("Parent item not found.");
                else if (!parent.Access.CanCreate())
                {
                    Context.ClientPage.ClientResponse.Alert("You do not have permission to create items here.");
                }
                else
                {
                    try
                    {
                        string[] strArray = args.Result.Split(new[] { "||" }, StringSplitOptions.None);
                        string name = strArray[0];                    
                        if (strArray.Length == 1) // new item is NOT a page item, displayName or metaTitle information not needed
                        {
                            if (selectedTemplate.TemplateID == TemplateIDs.BranchTemplate)
                            {
                                BranchItem branch = (BranchItem)selectedTemplate;
                                Context.Workflow.AddItem(name, branch, parent);
                            }
                            else
                            {
                                TemplateItem template = (TemplateItem)selectedTemplate;
                                Context.Workflow.AddItem(name, template, parent);
                            }
                        }
                        else if (strArray.Length == 3) // new item is a page item, process displayName and metaTitle inputs
                        {
                            string displayName = Uri.UnescapeDataString(strArray[1]);
                            string metaTitle = Uri.UnescapeDataString(strArray[2]);
                            Item newChildPage;

                            if (selectedTemplate.TemplateID == TemplateIDs.BranchTemplate)
                            {
                                BranchItem branch = (BranchItem)selectedTemplate;
                                newChildPage = Context.Workflow.AddItem(name, branch, parent);
                            }
                            else
                            {
                                TemplateItem template = (TemplateItem)selectedTemplate;
                                newChildPage = Context.Workflow.AddItem(name, template, parent);
                            }
                            if (newChildPage == null)
                                return;
                            HealthIS.Apps.Util.SetDisplayNameAndMetaTitle(newChildPage, displayName, metaTitle);
                        }
                    }
                    catch (WorkflowException ex)
                    {
                    Log.Error("Workflow error: could not add item from master", (Exception) ex, (object) this);
                    SheerResponse.Alert(ex.Message);
                    }
                }
            }
            else
            {
                string selectedTemplatePath = selectedTemplate.Paths.Path.ToString();
                if (HealthIS.Apps.Util.isUserDefinedPageItem(selectedTemplatePath))
                {
                    UrlString urlString = ResourceUri.Parse("control:CustomAddMasterContentManager").ToUrlString();
                    parent.Uri.AddToUrlString(urlString);
                    SheerResponse.ShowModalDialog(urlString.ToString(), "500px", "275px", string.Empty, true);
                }
                else
                {
                    SheerResponse.Input("Enter a name for the new item:", selectedTemplate.DisplayName, Settings.ItemNameValidation, "'$Input' is not a valid name.", Settings.MaxItemNameLength);
                }            
                args.WaitForPostBack();
            }
        }       
    }

    // Represents the AddFromTemplate command
    public class AddFromTemplate : Sitecore.Shell.Framework.Commands.Command
    {
        public static Item parentItem;

        public override void Execute(Sitecore.Shell.Framework.Commands.CommandContext context)
        {
            Assert.ArgumentNotNull((object)context, "context");
            if (context.Items.Length != 1 || !context.Items[0].Access.CanCreate())
                return;
            Item obj = context.Items[0];
            NameValueCollection parameters = new NameValueCollection();
            parameters["Master"] = context.Parameters["master"];
            parameters["ItemID"] = obj.ID.ToString();
            parameters["Language"] = obj.Language.ToString();
            parameters["Version"] = obj.Version.ToString();
            Context.ClientPage.Start((object)this, "Add", parameters);
            
        }

        public override CommandState QueryState(CommandContext context)
        {
            Assert.ArgumentNotNull((object)context, "context");
            if (context.Items.Length != 1)
                return CommandState.Hidden;
            if (!context.Items[0].Access.CanCreate() || !context.Items[0].Access.CanWriteLanguage())
                return CommandState.Disabled;
            return base.QueryState(context);
        }

        protected void Add(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");            
            parentItem = Sitecore.Client.GetItemNotNull(args.Parameters["itemid"], Language.Parse(args.Parameters["language"]));
            if (args.IsPostBack)
            {
                if (!args.HasResult)
                    return;
                if (parentItem == null)
                    SheerResponse.Alert("Parent item not found.");
                else if (!parentItem.Access.CanCreate())
                {
                    SheerResponse.Alert("You do not have permission to create items here.");
                }
                else
                {
                    try
                    {
                        string[] strArray = args.Result.Split(new[] { "||" }, StringSplitOptions.None);
                        string id = strArray[0];
                        string name = Uri.UnescapeDataString(strArray[1]);

                        Item selectedTemplate = Context.ContentDatabase.GetItem(id);
                        Item newChildPage;

                        if (selectedTemplate.TemplateID == TemplateIDs.BranchTemplate)
                        {
                            BranchItem branch = (BranchItem)selectedTemplate;
                            newChildPage = Context.Workflow.AddItem(name, branch, parentItem);
                        }
                        else
                        {
                            TemplateItem template = (TemplateItem)selectedTemplate;
                            newChildPage = Context.Workflow.AddItem(name, template, parentItem);
                        }
                        if (newChildPage == null)
                            return;

                        if (strArray.Length == 4) // process displayName and metaTitle inputs
                        {
                           string displayName = Uri.UnescapeDataString(strArray[2]);
                           string metaTitle = Uri.UnescapeDataString(strArray[3]);
                        
                           HealthIS.Apps.Util.SetDisplayNameAndMetaTitle(newChildPage, displayName, metaTitle); 
                        }
                    }
                    catch (WorkflowException ex)
                    {
                        Log.Error("Workflow error: could not add item from master", (Exception)ex, (object)this);
                        SheerResponse.Alert(ex.Message);
                    }
                }                
            }    
            else
            {
                Item parentTemplate = parentItem.Template;                
                string parentTemplatePath = parentTemplate.Paths.Path.ToString();
                UrlString urlString;
                if (HealthIS.Apps.Util.isUserDefinedPageItem(parentTemplatePath))
                {
                    urlString = ResourceUri.Parse("control:CustomAddFromTemplate").ToUrlString();
                }
                else
                {
                    urlString = ResourceUri.Parse("control:AddFromTemplate").ToUrlString();
                }
                parentItem.Uri.AddToUrlString(urlString);
                SheerResponse.ShowModalDialog(urlString.ToString(), "1200px", "700px", string.Empty, true);
                args.WaitForPostBack();
            }
        }
    }

    // Represents SortContent command specifically for Faculty Directory
    public class FacDirSort : Sitecore.Shell.Applications.WebEdit.Commands.WebEditCommand
    {
        public override void Execute(CommandContext context)
        {
          Assert.ArgumentNotNull((object) context, "context");
          string parameter1 = context.Parameters["itemid"];
          if (string.IsNullOrEmpty(parameter1))
            parameter1 = context.Parameters["id"];
          NameValueCollection parameters = new NameValueCollection();
          if (!string.IsNullOrEmpty(parameter1))
          {
            parameters["itemid"] = parameter1;
            Context.ClientPage.Start((object) this, "Run", parameters);
          }
          else
          {
            Item obj1 = context.Items.Length > 0 ? context.Items[0] : (Item) null;
            if (obj1 == null)
            {
              SheerResponse.Alert("Item \"{0}\" not found.");
            }
            else
            {
              string parameter2 = context.Parameters["referenceid"];
              if (string.IsNullOrEmpty(parameter2))
              {
                parameters["itemid"] = obj1.ID.ToString();
                Context.ClientPage.Start("Run", parameters);
              }
              else
              {
                Item obj2 = this.GetDatasourceItem(parameter2, obj1) ?? obj1;
                parameters["itemid"] = obj2.ID.ToString();
                Context.ClientPage.Start((object) this, "Run", parameters);
              }
            }
          }
        }

        protected virtual void Run(ClientPipelineArgs args)
        {
          Assert.ArgumentNotNull((object) args, "args");
          if (!SheerResponse.CheckModified())
            return;
          if (args.IsPostBack)
          {
            if (!args.HasResult)
              return;   
            WebEditCommand.Reload();
          }
          else
          {
            //SheerResponse.ShowModalDialog(this.GetOptions(args).ToUrlString().ToString(), true);
              SortContentOptions sco = this.GetOptions(args);              
              Assert.IsNotNull((object)Context.Site, "context site");
              UrlString urlString = new UrlString(Context.Site.XmlControlPage);
              urlString["xmlcontrol"] = "Sitecore.Shell.Applications.Dialogs.FacDirSort";
              sco.Item.Uri.AddToUrlString(urlString);
            SheerResponse.ShowModalDialog(urlString.ToString(), true);
            args.WaitForPostBack(true);
          }          
        }

        protected virtual SortContentOptions GetOptions(ClientPipelineArgs args)
        {
          Assert.ArgumentNotNull((object) args, "args");
          string parameter = args.Parameters["itemid"];
          Language language1 = Sitecore.Web.WebEditUtil.GetClientContentLanguage();
          if ((object) language1 == null)
            language1 = Context.Language;
          Language language2 = language1;
          Item obj = Client.ContentDatabase.GetItem(parameter, language2);
          Assert.IsNotNull((object) obj, "item");
          return new SortContentOptions(obj);
        }

        private Item GetDatasourceItem(string referenceId, Item item)
        {
          Assert.ArgumentNotNull((object) referenceId, "referenceId");
          Assert.ArgumentNotNull((object) item, "item");
          string xml = Sitecore.Web.WebEditUtil.ConvertJSONLayoutToXML(WebUtil.GetFormValue("scLayout"));
          Assert.IsNotNull((object) xml, "xmlLayout");
          LayoutDefinition layoutDefinition = LayoutDefinition.Parse(xml);
          ID clientDeviceId = Sitecore.Web.WebEditUtil.GetClientDeviceId();
          Assert.IsNotNull((object) clientDeviceId, "deviceId");
          RenderingDefinition renderingByUniqueId = layoutDefinition.GetDevice(clientDeviceId.ToString()).GetRenderingByUniqueId(referenceId);
          Assert.IsNotNull((object) renderingByUniqueId, "rendering");
          RenderingReference renderingReference = new RenderingReference(renderingByUniqueId, item.Language, item.Database);
          if (!string.IsNullOrEmpty(renderingReference.Settings.DataSource))
            return item.Database.GetItem(renderingReference.Settings.DataSource);
          return (Item) null;
        }
    }
}