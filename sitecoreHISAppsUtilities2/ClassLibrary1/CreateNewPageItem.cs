using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Pipelines.HasPresentation;
using Sitecore.Resources;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Sites;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Sitecore.Shell.Applications.WebEdit.Commands
{
  [UsedImplicitly]
  [Serializable]
  public class CreateNewPageItem : WebEditCommand
  {
      public static string parentItemID = string.Empty;
      
      public override void Execute(CommandContext context)
    {
      Assert.ArgumentNotNull((object) context, "context");
      NameValueCollection parameters = new NameValueCollection();
      string str = context.Parameters["itemid"] ?? context.Parameters["id"];
      ItemUri queryString = ItemUri.ParseQueryString();
      if (string.IsNullOrEmpty(str) && queryString != (ItemUri) null)
        str = queryString.ItemID.ToString();
      parameters["itemid"] = str;
      string parameter = context.Parameters["language"];
      if (string.IsNullOrEmpty(parameter) && queryString != (ItemUri) null)
        parameter = queryString.Language.ToString();
      parameters["language"] = parameter;
      parameters["navigate"] = context.Parameters["navigate"];
      Context.ClientPage.Start((object) this, "Run", parameters);
    }

    public override CommandState QueryState(CommandContext context)
    {
      Assert.ArgumentNotNull((object) context, "context");
      if (WebUtil.GetQueryString("mode") == "preview" || context.Items.Length > 0 && !context.Items[0].Access.CanCreate())
        return CommandState.Disabled;
      return base.QueryState(context);
    }

    public virtual void ExecuteCommand(Item item, BranchItem branch, string name)
    {
      Assert.ArgumentNotNull((object) item, "item");
      Assert.ArgumentNotNull((object)branch, "branch");
      Assert.ArgumentNotNull((object) name, "name");
      string str = branch.InnerItem.Fields[FieldIDs.Command].Value;
      if (string.IsNullOrEmpty(str))
        return;
      Command command = CommandManager.GetCommand(str);
      if (command == null)
        return;
      CommandContext context = new CommandContext(item);
      if (Message.IsMessage(str))
      {
        Message message = Message.Parse((object) Context.ClientPage, str);
        context.Parameters.Add(message.Arguments);
      }      
      context.Parameters.Add("name", name);
      command.Execute(context);
    }

    public virtual void PolicyBasedUnlock(Item item)
    {
      Assert.ArgumentNotNull((object) item, "item");
      Database database = Factory.GetDatabase("core");
      Assert.IsNotNull((object) database, "Database \"{0}\" not found", (object) "core");
      if (database.GetItem("/sitecore/system/Settings/Security/Policies/Page Editor/Keep Lock After Save") != null)
        return;
      try
      {
        item.Locking.Unlock();
      }
      catch (Exception ex)
      {
        Log.Warn("Item {0} couldn't be automatically unlocked, which was required by the policy-based locking.", ex, (object) this);
      }
    }

    public virtual void SetDisplayNameAndMetaTitle(Item item, string displayName, string metaTitle)
    {
        Assert.ArgumentNotNull((object)item, "item");
        if ((!String.IsNullOrEmpty(displayName)) || (!String.IsNullOrEmpty(metaTitle)))
        {
            item.Editing.BeginEdit();
            try
            {
                item.Fields["__Display Name"].Value = displayName;
                item.Fields["Meta Title"].Value = metaTitle;
                item.Editing.EndEdit();
            }
            catch (Exception ex)
            {
                Log.Error("Could not update item " + item.Paths.FullPath + ": " + ex.Message, this);
                item.Editing.CancelEdit();
            }            
        }
    }

    [UsedImplicitly]
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
        Assert.IsNotNull((object) branch, typeof (BranchItem));
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
        }
        SetDisplayNameAndMetaTitle(obj, displayName, metaTitle);
      }
      else if (!itemNotNull.Access.CanCreate())
      {
        SheerResponse.Alert("You do not have permission to create an item here.");
      }
      else
      {
        UrlString urlString = ResourceUri.Parse("control:Applications.WebEdit.Dialogs.AddMaster").ToUrlString();
        itemNotNull.Uri.AddToUrlString(urlString);
        SheerResponse.ShowModalDialog(urlString.ToString(), "1200px", "700px", string.Empty, true);
        args.WaitForPostBack();
      }
    }
  }
}
