using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.IO;
using Sitecore.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.XmlControls;

namespace Sitecore.Shell.Applications.Templates.AddFromTemplate
{
    class AddFromTemplateForm : DialogForm
    {
    
        protected XmlControl Dialog;    
        protected Edit ItemName;
        protected Edit DisplayName;
        protected Edit MetaTitle;
        protected Edit TemplateName;    
        protected TreeviewEx Templates;    
        protected DataContext TemplatesDataContext;

        /// <summary>Executes the OK action.</summary>
        protected void DoOK()
        {
          Item parentItem = HealthIS.Apps.Commands.AddFromTemplate.parentItem;
          string str = this.TemplateName.Value;

          if (string.IsNullOrEmpty(str))
            return;
          if (!str.StartsWith("/sitecore/templates", StringComparison.InvariantCultureIgnoreCase))
            str = FileUtil.MakePath("/sitecore/templates", str);
          Item obj = Context.ContentDatabase.GetItem(str);
          if (obj == null)
            SheerResponse.Alert(Translate.Text("The template \"{0}\" does not exist.\n\nSelect a template from the list.", (object) this.TemplateName.Value));
          else if (string.IsNullOrEmpty(this.ItemName.Value))
            SheerResponse.Alert("You must specify a name for the new item.");
          else if (!Regex.IsMatch(this.ItemName.Value, Settings.ItemNameValidation, RegexOptions.ECMAScript))
            SheerResponse.Alert("The name contains invalid characters.");
          else if (this.ItemName.Value.Length > Settings.MaxItemNameLength)
          {
            SheerResponse.Alert("The name is too long.");
          }
          else if (HealthIS.Apps.Util.IsADuplicateItemName(parentItem.ID.ToString(), this.ItemName.Value))
          {
              SheerResponse.Alert("Duplicate item name. Please specify another name for this new item.");
          }
          else
          {
            Registry.SetString("/Current_User/History/Add.From.Template", obj.ID.ToString());
            Context.ClientPage.ClientResponse.SetDialogValue(obj.ID.ToString() + "||" + this.ItemName.Value + "||" + this.DisplayName.Value + "||" + this.MetaTitle.Value);
            Context.ClientPage.ClientResponse.CloseWindow();
          }
        }

        /// <summary>Raises the load event.</summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs" /> instance containing the event data.
        /// </param>
        /// <contract>
        ///     <requires name="e" condition="not null" />
        /// </contract>
        protected override void OnLoad(EventArgs e)
        {
          Assert.ArgumentNotNull((object) e, "e");
          base.OnLoad(e);
          if (Context.ClientPage.IsEvent)
            return;
          string queryString = WebUtil.GetQueryString("ic");
          if (!string.IsNullOrEmpty(queryString))
            this.Dialog["Icon"] = (object) queryString;
          string str1 = WebUtil.SafeEncode(WebUtil.GetQueryString("he"));
          if (!string.IsNullOrEmpty(str1))
            this.Dialog["Header"] = (object) str1;
          string str2 = WebUtil.SafeEncode(WebUtil.GetQueryString("txt"));
          if (!string.IsNullOrEmpty(str2))
            this.Dialog["Text"] = (object) str2;
          string str3 = WebUtil.SafeEncode(WebUtil.GetQueryString("btn"));
          if (!string.IsNullOrEmpty(str3))
            this.Dialog["OKButton"] = (object) str3;
          string str4 = Registry.GetString("/Current_User/History/Add.From.Template");
          if (!string.IsNullOrEmpty(str4))
            this.TemplatesDataContext.Folder = str4;
          Item folder = this.TemplatesDataContext.GetFolder();
          if (folder == null)
            return;
          this.TemplateName.Value = this.ShortenPath(folder.Paths.Path);
          this.DisplayName.Value = Translate.Text("New") + " " + folder.DisplayName;
        }

        /// <summary>Handles a click on the OK button.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The arguments.</param>
        /// <remarks>
        /// When the user clicks OK, the dialog is closed by calling
        /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.
        /// </remarks>
        /// <contract>
        ///     <requires name="sender" condition="not null" />
        ///     <requires name="args" condition="not null" />
        /// </contract>
        protected override void OnOK(object sender, EventArgs args)
        {
          Assert.ArgumentNotNull(sender, "sender");
          Assert.ArgumentNotNull((object) args, "args");
          this.DoOK();
        }

        /// <summary>Selects the tree node.</summary>
        protected void SelectTreeNode()
        {
          Item selectionItem = this.Templates.GetSelectionItem();
          if (selectionItem == null || !(selectionItem.TemplateID == TemplateIDs.Template) && !(selectionItem.TemplateID == TemplateIDs.BranchTemplate))
            return;
          this.TemplateName.Value = this.ShortenPath(selectionItem.Paths.Path);
          this.DisplayName.Value = Translate.Text("New") + " " + selectionItem.DisplayName;
          SheerResponse.Focus("ItemName");          
        }

        /// <summary>Shortens the path.</summary>
        /// <param name="path">The path.</param>
        /// <returns>The sorten path.</returns>
        /// <contract>
        /// 	<requires name="path" condition="not null" />
        /// 	<ensures condition="not null" />
        /// </contract>
        private string ShortenPath(string path)
        {
          Assert.ArgumentNotNull((object) path, "path");
          Item root = this.TemplatesDataContext.GetRoot();
          if (root != null && root.ID != root.Database.GetRootItem().ID)
          {
            string path1 = root.Paths.Path;
            if (path.StartsWith(path1, StringComparison.InvariantCulture))
              path = StringUtil.Mid(path, path1.Length);
          }
          return path;
        }
    }
}