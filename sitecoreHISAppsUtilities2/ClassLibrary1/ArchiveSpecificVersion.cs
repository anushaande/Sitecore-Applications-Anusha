using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Archiving;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.Dialogs
{
    public class ArchiveSpecificVersionForm : DialogForm
    {
    /// <summary>
    /// The versions.
    /// 
    /// </summary>
    protected Border Versions;

    /// <summary>
    /// Gets the current item.
    /// 
    /// </summary>
    protected Item CurrentItem
    {
        get
        {
            ID result;
            Assert.IsTrue(ID.TryParse(WebUtil.GetQueryString("id"), out result), "item id");
            Database database = Database.GetDatabase(WebUtil.GetQueryString("db"));
            Assert.IsNotNull((object) database, "database");
            Item obj = database.GetItem(result);
            Assert.IsNotNull((object) obj, "Item not found");
            return obj;
        }
    }

    /// <summary>
    /// Raises the load event.
    /// 
    /// </summary>
    /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.
    ///             </param>
    /// <remarks>
    /// This method notifies the server control that it should perform actions common to each HTTP
    ///               request for the page it is associated with, such as setting up a database query. At this
    ///               stage in the page lifecycle, server controls in the hierarchy are created and initialized,
    ///               view state is restored, and form controls reflect client-side data. Use the IsPostBack
    ///               property to determine whether the page is being loaded in response to a client postback,
    ///               or if it is being loaded and accessed for the first time.
    /// 
    /// </remarks>
    protected override void OnLoad(EventArgs e)
    {
        
        Assert.ArgumentNotNull((object)e, "e");
        Item obj = Client.CoreDatabase.GetItem("/sitecore/content/Applications/Content Editor/Ribbons/Chunks/Schedule/Archive");
        Assert.HasAccess(obj != null && obj.Access.CanRead(), "Access denied");
        base.OnLoad(e);
        if (Context.ClientPage.IsEvent)
            return;
        
        this.RenderVersions(this.CurrentItem);
    }

    /// <summary>
    /// The ok click handler.
    /// 
    /// </summary>
    /// <param name="sender">The sender.</param><param name="args">The args.</param>
    protected override void OnOK(object sender, EventArgs args)
    {
        Assert.ArgumentNotNull(sender, "sender");
        Assert.ArgumentNotNull((object)args, "args");

        ListString listString = new ListString(WebUtil.GetFormValue("selectedValue"));
        string allChecked = WebUtil.GetFormValue("allChecked");

        if (allChecked == "true") {
            SheerResponse.Alert("You can't select all versions. Please uncheck at least one version.");
        }
        else
        {
            if (listString != null)
            {
                ArchiveItemVersion(this.CurrentItem, listString);
            }
            base.OnOK(sender, args);
        }
    }

    /// <summary>
    /// Archive specific versions.
    /// 
    /// </summary>
    /// <param name="item">The item.
    ///             </param>
    private void ArchiveItemVersion(Item item, ListString list)
    {
        string databaseName = item.Database.Name;
        Database database = Database.GetDatabase(databaseName);
        Assert.IsNotNull((object)database, "Database \"{0}\" not found", (object)databaseName);
        Language language = Language.Parse(item.Language.Name);

        foreach (var v in list) {
            Sitecore.Data.Version version = Sitecore.Data.Version.Parse(item.Versions.GetVersionNumbers().Where(n => n.Number.ToString() == v.ToString()).FirstOrDefault());

            Item obj = database.GetItem(item.ID, language, version);
            if (obj == null)
                return;

            Sitecore.Data.Archiving.Archive archive = ArchiveManager.GetArchive("archive", database);
            Log.Audit(new object(), "Archive version: {0}", new string[1]
            {
                AuditFormatter.FormatItem(obj)
            });
            archive.ArchiveVersion(obj);
        }
    }

    /// <summary>
    /// Renders the versions.
    /// 
    /// </summary>
    /// <param name="item">The item.
    ///             </param>
    protected void RenderVersions(Item item)
    {
      Assert.ArgumentNotNull((object) item, "item");
      IEnumerable<Item> enumerable = (IEnumerable<Item>) Enumerable.ThenBy<Item, int>(Enumerable.OrderBy<Item, string>((IEnumerable<Item>) item.Versions.GetVersions(true), (Func<Item, string>) (i => i.Language.Name)), (Func<Item, int>) (i => i.Version.Number));
      StringBuilder stringBuilder1 = new StringBuilder("<table width=\"100%\" class=\"scListControl\" cellpadding=\"0\" cellspacing=\"0\">");
      stringBuilder1.Append("<tr>");
      stringBuilder1.Append("<td></td>");
      stringBuilder1.Append("<td><b>" + Translate.Text("Language") + "</b></td>");
      stringBuilder1.Append("<td><b>" + Translate.Text("Version") + "</b></td>");
      stringBuilder1.Append("<td width=\"100%\" style=\"text-align:center;\"><b>" + Translate.Text("Updated Date Time") + "</b></td>");
      stringBuilder1.Append("</tr>");
      this.Versions.Controls.Add((System.Web.UI.Control) new LiteralControl(stringBuilder1.ToString()));

      foreach (Item obj in enumerable)
      {
        StringBuilder stringBuilder2 = new StringBuilder();
        this.Versions.Controls.Add((System.Web.UI.Control)new LiteralControl(stringBuilder2.ToString()));
        Checkbox checkBoxPick = new Checkbox();
        checkBoxPick.ID = "archive_" + (object)obj.Language.Name + "_" + (object)obj.Version.Number.ToString();
        checkBoxPick.Attributes.Add("VersionNumber", obj.Version.Number.ToString());
        checkBoxPick.Class = "scComboBoxPick";
        checkBoxPick.Name = "scComboBoxPick[]";
        checkBoxPick.Attributes.Add("OnClick", "javascript:scPersistSelectedValue();");
        checkBoxPick.Width = new Unit(100.0, UnitType.Percentage);
        this.Versions.Controls.Add((System.Web.UI.Control)new LiteralControl("<tr><td style=\"text-align:center\">"));
        this.Versions.Controls.Add((System.Web.UI.Control)checkBoxPick);
        this.Versions.Controls.Add((System.Web.UI.Control)new LiteralControl("</td>"));
        stringBuilder2.AppendFormat("<td style=\"text-align:center\"><b>{0}</b><td style=\"text-align:center\"><b>{1}</b></td><td style=\"text-align:center\"><b>{2}</b></td>", (object)obj.Language.Name, (object)obj.Version.Number, DateUtil.FormatShortDateTime(DateUtil.IsoDateToDateTime(obj.Fields["__Updated"].Value)));
        this.Versions.Controls.Add((System.Web.UI.Control)new LiteralControl(stringBuilder2.ToString()));
        this.Versions.Controls.Add((System.Web.UI.Control)new LiteralControl("</tr>"));
      }
      this.Versions.Controls.Add((System.Web.UI.Control) new LiteralControl("</table>"));
    }
  }
}
