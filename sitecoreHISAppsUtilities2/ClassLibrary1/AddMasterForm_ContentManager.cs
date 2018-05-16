using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Text.RegularExpressions;

namespace Sitecore.Shell.Applications.ContentManager.Dialogs.AddMaster
{
    class AddMasterForm : DialogForm
  {
    protected Edit ItemName;
    protected Edit DisplayName;
    protected Edit MetaTitle;  


    protected override void OnLoad(EventArgs e)
    {
      Assert.ArgumentNotNull((object) e, "e");
      base.OnLoad(e);
      if (Context.ClientPage.IsEvent)
        return;      
    }

    protected void OK_Click()
    {
        string parentItemID = HealthIS.Apps.Commands.AddMaster.parentItemID;

        if (this.ItemName.Value.Length == 0)
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
          SheerResponse.SetDialogValue(this.ItemName.Value + "||" + this.DisplayName.Value + "||" + this.MetaTitle.Value);
          SheerResponse.CloseWindow();
        }      
    }

    protected override void OnOK(object sender, EventArgs args)
    {
      Assert.ArgumentNotNull(sender, "sender");
      Assert.ArgumentNotNull((object) args, "args");
      this.OK_Click();
    }
  }
}