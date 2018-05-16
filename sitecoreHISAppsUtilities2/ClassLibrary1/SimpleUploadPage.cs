using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using Sitecore.Configuration;
using Sitecore.Controls;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.Authentication;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.XamlSharp.Xaml;

namespace Sitecore.Shell.Applications.FlashUpload.Simple
{
    class SimpleUploadPage : DialogPage
    {
        /// <summary>The input upload id.</summary>
        protected HtmlInputHidden InputUploadID;
        /// <summary>The upload session id.</summary>
        protected HtmlInputHidden UploadSessionID;
        /// <summary>Keeps TicketID</summary>
        protected HtmlInputHidden UploadSessionID1;
        // max image upload size allowed
        protected HtmlInputHidden uploadImageLimit;

        /// <summary>Gets or sets the upload ID.</summary>
        /// <value>The upload ID.</value>
        protected string UploadID
        {
          get
          {
            return StringUtil.GetString(this.ViewState["uploadID"]);
          }
          set
          {
            Assert.ArgumentNotNull((object) value, "value");
            this.ViewState["uploadID"] = (object) value;
          }
        }

        /// <summary>Gets or sets the warning message.</summary>
        /// <value>The warning message.</value>
        protected string WarningMessage
        {
          get
          {
            return StringUtil.GetString(this.ViewState["WarningMessage"]);
          }
          set
          {
            Assert.ArgumentNotNull((object) value, "value");
            this.ViewState["WarningMessage"] = (object) value;
          }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Sitecore.Shell.Applications.FlashUpload.Simple.SimpleUploadPage" /> is uploading.
        /// </summary>
        /// <value><c>true</c> if uploading; otherwise, <c>false</c>.</value>
        protected bool Uploading
        {
          get
          {
            return MainUtil.GetBool(this.ViewState["Uploading"], false);
          }
          set
          {
            this.ViewState["Uploading"] = (object) value.ToString();
          }
        }

        /// <summary>Closes this dialog.</summary>
        /// <param name="uploadedItemsRaw">The uploaded items raw.</param>
        public void Close(string uploadedItemsRaw)
        {
          Assert.ArgumentNotNull((object) uploadedItemsRaw, "uploadedItemsRaw");
          ListString listString = new ListString(uploadedItemsRaw);
          if (uploadedItemsRaw == "undefined" || listString.Count == 0)
          {
            base.OK_Click();
          }
          else
          {
            ItemUri itemUri = ItemUri.Parse(listString[0]);
            Assert.IsNotNull((object) itemUri, "uri");
            if (WebUtil.GetQueryString("edit") == "1")
            {
              UrlString urlString = new UrlString("/sitecore/shell/Applications/Content Manager/default.aspx");
              urlString["fo"] = itemUri.ItemID.ToString();
              urlString["mo"] = "popup";
              urlString["wb"] = "0";
              urlString["pager"] = "0";
              urlString[Sitecore.Configuration.State.Client.UsesBrowserWindowsQueryParameterName] = WebUtil.GetQueryString(Sitecore.Configuration.State.Client.UsesBrowserWindowsQueryParameterName, "0");
              urlString.Add("sc_content", WebUtil.GetQueryString("sc_content"));
              SheerResponse.ShowModalDialog(urlString.ToString(), string.Equals(Sitecore.Context.Language.Name, "ja-jp", StringComparison.InvariantCultureIgnoreCase) ? "1115" : "955", "560");
            }
            SheerResponse.SetDialogValue(itemUri.ItemID.ToString());
            base.OK_Click();
          }
        }

        /// <summary>Handles a click on the OK button.</summary>
        /// <remarks>
        /// When the user clicks OK, the dialog is closed by calling
        /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.
        /// </remarks>
        protected override void Cancel_Click()
        {
          if (this.Uploading)
            SheerResponse.Eval("scUpload.cancel();");
          else
            base.Cancel_Click();
        }

        /// <summary>Handles a click on the OK button.</summary>
        /// <remarks>
        /// When the user clicks OK, the dialog is closed by calling
        /// the <see cref="M:Sitecore.Web.UI.Sheer.ClientResponse.CloseWindow">CloseWindow</see> method.
        /// </remarks>
        protected override void OK_Click()
        {
          if (!string.IsNullOrEmpty(this.WarningMessage))
            SheerResponse.Alert(this.WarningMessage);
          else
            SheerResponse.Eval("scUpload.start();");
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"></see> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs"></see> object that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
          Assert.ArgumentNotNull((object) e, "e");
          base.OnLoad(e);
          if (XamlControl.AjaxScriptManager.IsEvent)
            return;
          Button descendantControl = this.FindDescendantControl("OK") as Button;
          if (descendantControl != null)
          {
            this.Text = Translate.Text("Click Browse to select the file that you want to upload.");
            descendantControl.Visible = false;
          }
          this.UploadID = Guid.NewGuid().ToString();
          this.InputUploadID.Value = this.UploadID;
          if (!UIUtil.IsFirefox() && !UIUtil.IsWebkit())
            return;
          string sessionId = this.Context.Session.SessionID;
          Assert.IsNotNullOrEmpty(sessionId, "session id");
          this.UploadSessionID.Value = sessionId;
          this.UploadSessionID1.Value = TicketManager.GetCurrentTicketId();

          this.uploadImageLimit.Value = Sitecore.Configuration.Settings.GetSetting("Media.MaxImageSizeInDatabase");
        }

        /// <summary>Called when this instance has error.</summary>
        protected void OnError()
        {
          SheerResponse.Alert(Translate.Text("An error occured while uploading a file .\n\nThe reason may be that the file does not exist or the path is wrong."));
        }

        /// <summary>Called when this instance has queued.</summary>
        /// <param name="filename">The filename.</param>
        /// <param name="lengthString">The length string.</param>
        protected void OnQueued(string filename, string lengthString)
        {
          Assert.IsNotNullOrEmpty(filename, "filename");
          Assert.IsNotNullOrEmpty(lengthString, "lengthString");
          int num = int.Parse(lengthString);
          long databaseUploadSize = Settings.Upload.MaximumDatabaseUploadSize;
          if ((long) num > databaseUploadSize)
          {
            string text = Translate.Text("The file \"{0}\" is too big to be uploaded.\n\nThe maximum size of a file that can be uploaded is {1}.", (object) filename, (object) MainUtil.FormatSize(databaseUploadSize));
            this.WarningMessage = text;
            SheerResponse.Alert(text);
          }
          else
          {
            SheerResponse.SetInnerHtml("FilenameText", filename + " (" + MainUtil.FormatSize((long) num) + ")");
            this.WarningMessage = string.Empty;
            SheerResponse.Eval("scUpload.start()");
          }
        }

        /// <summary>Called when this instance has upload.</summary>
        protected void OnUpload()
        {
          this.Uploading = true;
          SheerResponse.SetInnerHtml("Message", Translate.Text("Uploading .."));
          SheerResponse.Eval("scForm.autoIncreaseModalDialogHeight();");
        }        
    }
}