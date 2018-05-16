using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Pipelines.RenderItemTile;
using Sitecore.Shell.Web.UI;
using Sitecore.Web.UI;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using System.Text;
using System.Text.RegularExpressions;

namespace Sitecore.Shell.Applications.WebEdit.Dialogs.AddMaster
{
    class AddMasterForm : DialogForm
  {
    protected Edit ItemName;
    protected Edit DisplayName;
    protected Edit MetaTitle;
    protected Scrollbox Items;

    public string SelectedItem
    {
      get
      {
        return StringUtil.GetString(this.ServerProperties["SelectedItem"]);
      }
      set
      {
        Assert.ArgumentNotNull((object) value, "value");
        this.ServerProperties["SelectedItem"] = (object) value;
      }
    }

    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull((object) e, "e");
      base.OnLoad(e);
      if (Context.ClientPage.IsEvent)
        return;
      this.Items.InnerHtml = this.RenderItems(AddMasterForm.BuildItems());
    }

    protected void Items_Click(string id, string language, string version)
    {
      Assert.ArgumentNotNull((object) id, "id");
      Assert.ArgumentNotNull((object) language, "language");
      Assert.ArgumentNotNull((object) version, "version");
      Item obj = Context.ContentDatabase.GetItem(id, Language.Parse(language), Sitecore.Data.Version.Parse(version));
      if (obj == null)
        return;
      if (!string.IsNullOrEmpty(this.SelectedItem))
        SheerResponse.SetAttribute("I" + (object) ID.Parse(this.SelectedItem).ToShortID(), "class", "scItemThumbnail");
      this.SelectedItem = obj.ID.ToString();
      SheerResponse.SetAttribute("I" + (object) ID.Parse(this.SelectedItem).ToShortID(), "class", "scItemThumbnailSelected");
    }

    protected void OK_Click()
    {
        string parentItemID = HealthIS.Apps.Commands.New.parentItemID;

      if (string.IsNullOrEmpty(this.SelectedItem))
      {
        SheerResponse.Alert("Select an item.");
      }
      else
      {
        Item obj = Context.ContentDatabase.GetItem(this.SelectedItem);
        if (obj == null)
          SheerResponse.Alert(Translate.Text("The item could not be found.\n\nIt may have been deleted by another user."));
        else if (this.ItemName.Value.Length == 0)
          SheerResponse.Alert("You must specify a name for the new item.");
        else if (!Regex.IsMatch(this.ItemName.Value, Settings.ItemNameValidation, RegexOptions.ECMAScript))
          SheerResponse.Alert("The name contains invalid characters.");
        else if (this.ItemName.Value.Length > Settings.MaxItemNameLength)
        {
          SheerResponse.Alert("The name is too long.");
        }
        else if (HealthIS.Apps.Util.IsADuplicateItemName(parentItemID, this.ItemName.Value))
        {
            SheerResponse.Alert("Duplicate item name. Please specify another name for this new item.");
        }
        else
        {
          SheerResponse.SetDialogValue(obj.ID.ToString() + "||" + this.ItemName.Value + "||" + this.DisplayName.Value + "||" + this.MetaTitle.Value);
          SheerResponse.CloseWindow();
        }
      }
    }

    protected override void OnOK(object sender, EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull((object) args, "args");
      this.OK_Click();
    }

    private static List<Item> BuildItems()
    {
      List<Item> objList = new List<Item>();
      ItemUri queryString = ItemUri.ParseQueryString();
      Assert.IsNotNull((object) queryString, "Item Uri not found");
      Item obj = Database.GetItem(queryString);
      Assert.IsNotNull((object) obj, "Item \"{0}\" not found.", (object) queryString.ToString());
      return Sitecore.Data.Masters.Masters.GetMasters(obj);
    }

    private string RenderItems(List<Item> items)
    {
      Assert.ArgumentNotNull((object) items, "items");
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (Item obj in items)
      {
        string click = StringUtil.EscapeJavascriptString(string.Format("Items_Click(\"{0}\",\"{1}\",\"{2}\")", (object) obj.ID, (object) obj.Language, (object) obj.Version));
        RenderItemTileArgs args = new RenderItemTileArgs(obj, TileView.Thumbnails, ImageDimension.idDefault, click);
        if (flag)
        {
          args.CssClass = "scItemThumbnailSelected";
          this.SelectedItem = obj.ID.ToString();
          flag = false;
        }
        string str = ItemTileService.RenderTile(args);
        stringBuilder.Append(str);
      }
      return stringBuilder.ToString();
    }    
  }
}