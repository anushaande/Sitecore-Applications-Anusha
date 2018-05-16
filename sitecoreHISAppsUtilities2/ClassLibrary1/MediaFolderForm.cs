using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Resources;
using Sitecore.Resources.Media;
using Sitecore.Shell.Applications.Dialogs.Folders;
using Sitecore.Shell.Applications.FlashUpload.Advanced;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.Authentication;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;


namespace Sitecore.Shell.Applications.Media.MediaFolder
{
    /// <summary>Represents the Media Folder form.</summary>
    class MediaFolderForm : FolderBaseForm
    {
        /// <summary>The as files.</summary>
        protected Checkbox AsFiles;
        /// <summary>The cancel button.</summary>
        protected Button CancelButton;
        /// <summary>The close button.</summary>
        protected Button CloseButton;
        /// <summary>The header.</summary>
        protected Literal Header;
        /// <summary>The settings container.</summary>
        protected Border SettingsContainer;
        /// <summary>The upload button.</summary>
        protected Button UploadButton;
        /// <summary>The versioned.</summary>
        protected Checkbox Versioned;

        /// <summary>Gets or sets the upload ID.</summary>
        /// <value>The upload ID.</value>
        protected string UploadID
        {
          get
          {
            return StringUtil.GetString(this.ServerProperties["UploadID"]);
          }
          set
          {
            Assert.ArgumentNotNullOrEmpty(value, "value");
            this.ServerProperties["UploadID"] = (object) value;
          }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="T:Sitecore.Shell.Applications.Media.MediaFolder.MediaFolderForm" /> uploading the started.
        /// </summary>
        /// <value>
        ///  <c>true</c> if the <see cref="T:Sitecore.Shell.Applications.Media.MediaFolder.MediaFolderForm" /> uploading the started; otherwise, <c>false</c>.
        /// </value>
        protected bool UploadingStarted
        {
          get
          {
            return MainUtil.GetBool(this.ServerProperties["UploadingStarted"], false);
          }
          set
          {
            this.ServerProperties["UploadingStarted"] = (object) value;
          }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Shell.Applications.Media.MediaFolder.MediaFolderForm" /> class.
        /// </summary>
        public MediaFolderForm()
        {
          this.ButtonsPath = "/sitecore/content/Applications/Content Editor/Editors/Media/Media Folder/Buttons";
          this.ButtonsTitle = "Options";
          this.ItemClick = string.Empty;
          this.ItemsTitle = "Media";
        }

        /// <summary>Handles the message.</summary>
        /// <param name="message">The message.</param>
        /// <contract>
        ///   <requires name="message" condition="not null" />
        /// </contract>
        public override void HandleMessage(Message message)
        {
          Assert.ArgumentNotNull((object) message, "message");
          switch (message.Name)
          {
            case "media:refresh":
              SheerResponse.SetLocation(string.Empty);
              break;
            default:
              base.HandleMessage(message);
              break;
          }
        }

        /// <summary>Executes the cancel event.</summary>
        /// <param name="args">The arguments.</param>
        protected void DoCancel(ClientPipelineArgs args)
        {
          Assert.ArgumentNotNull((object) args, "args");
          if (args.IsPostBack)
          {
            if (!args.HasResult || !(args.Result != "no"))
              return;
            SheerResponse.Eval("scMediaFolder.activeUploader.cancelUpload()");
            this.Done(string.Empty);
          }
          else
          {
            SheerResponse.Confirm("Are you sure you want to cancel uploading the files?");
            args.WaitForPostBack();
          }
        }

        /// <summary>Executes the  event.</summary>
        /// <param name="uploadedItems">The uploaded items.</param>
        protected void Done(string uploadedItems)
        {
          Assert.ArgumentNotNull((object) uploadedItems, "uploadedItems");
          List<Item> items = new List<Item>();
          string str = uploadedItems;
          char[] chArray = new char[1]{ '|' };
          foreach (string itemUri1 in new ListString(str.TrimEnd(chArray)).Items)
          {
            ItemUri itemUri2 = ItemUri.Parse(itemUri1);
            Assert.IsNotNull((object) itemUri2, "uploaded uri");
            Database database = Factory.GetDatabase(itemUri2.DatabaseName);
            Assert.IsNotNull((object) database, "database");
            Item obj = database.GetItem(itemUri2.ToDataUri());
            if (obj != null)
              items.Add(obj);
          }
          UploadedItems.Set(items);
          SheerResponse.Eval("document.body.parentNode.removeChild(document.body);");
          SheerResponse.Eval("scForm.getParentForm().postRequest('', '', '', 'item:refreshchildren(id=" + WebUtil.GetQueryString("id") + ")');");
          SheerResponse.Eval("scForm.getParentForm().postRequest('', '', '', 'item:refresh');");
        }

        /// <summary>Called when this instance has cancel.</summary>
        protected void OnCancel()
        {
          if (this.UploadingStarted)
            Context.ClientPage.Start((object) this, "DoCancel");
          else
            SheerResponse.Eval("scMediaFolder.activeUploader.cancelUpload()");
        }

        /// <summary>Called when the complete with has errors.</summary>
        protected void OnCompleteWithErrors()
        {
          SheerResponse.SetStyle(this.CloseButton.ID, "display", string.Empty);
          this.CancelButton.Visible = false;
          this.Header.Text = "One or more files could not be uploaded.\n\nSee the Log file for more details.";
          SheerResponse.SetStyle("Header", "color", "red");
        }

        /// <summary>Called when the files has cancelled.</summary>
        /// <param name="packet">The packet.</param>
        /// <contract>
        ///   <requires name="packet" condition="not null" />
        /// </contract>
        protected void OnFilesCancelled(string packet)
        {
          Assert.ArgumentNotNullOrEmpty(packet, "packet");
          ListString listString = new ListString(packet);
          Assert.IsTrue(listString.Count > 0, "Zero cancelled files posted");
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.Append("The following files are too big to be uploaded:");
          stringBuilder.Append("\n\n");
          foreach (string str in listString.Items)
            stringBuilder.Append(str + "\n");
          string str1 = MainUtil.FormatSize(Math.Min(Settings.Media.MaxSizeInDatabase, Settings.Runtime.EffectiveMaxRequestLengthBytes));
          stringBuilder.Append(Translate.Text("The maximum size of a file that can be uploaded is {0}.", (object) str1));
          SheerResponse.Alert(stringBuilder.ToString());
        }

        /// <summary>Raises the load event.</summary>
        /// <param name="e">
        /// The <see cref="T:System.EventArgs" /> instance containing the event data.
        /// </param>
        /// <contract>
        ///   <requires name="e" condition="not null" />
        /// </contract>
        protected override void OnLoad(EventArgs e)
        {
          Assert.ArgumentNotNull((object) e, "e");
          base.OnLoad(e);
          Dictionary<string, string> settings = new Dictionary<string, string>();
          this.UploadID = Guid.NewGuid().ToString();
          settings["uploadID"] = this.UploadID;
          if (UIUtil.IsFirefox() || UIUtil.IsWebkit())
          {
            string sessionId = HttpContext.Current.Session.SessionID;
            Assert.IsNotNullOrEmpty(sessionId, "session id");
            settings["uploadSessionID"] = sessionId;
            settings["uploadSessionID1"] = TicketManager.GetCurrentTicketId();
          }
          this.Versioned.Checked = Settings.Media.UploadAsVersionableByDefault;
          this.AsFiles.Visible = !Settings.Media.DisableFileMedia;
          this.AsFiles.Checked = Settings.Media.UploadAsFiles;
          if (!Settings.Upload.UserSelectableDestination)
            this.AsFiles.Visible = false;
          settings["uploadLimit"] = !this.AsFiles.Checked ? Math.Min(Settings.Media.MaxSizeInDatabase, Settings.Runtime.EffectiveMaxRequestLengthBytes).ToString() : Settings.Runtime.EffectiveMaxRequestLengthBytes.ToString();
          settings["uploadFileLimit"] = Settings.Runtime.EffectiveMaxRequestLengthBytes.ToString();
          settings["uploadingAsFilesMessage"] = Translate.Text("At least one of the files is too large to be uploaded to the database.");

          settings["uploadImageLimit"] = Sitecore.Configuration.Settings.GetSetting("Media.MaxImageSizeInDatabase");
      
          this.RenderSettings(settings);
        }

        /// <summary>Called when the upload starts.</summary>
        protected void OnStart()
        {
          this.UploadingStarted = true;
          this.UploadButton.Visible = false;
          this.Header.Text = "Uploading ..";
          SheerResponse.Eval("scMediaFolder.activeUploader.uploadNextFile()");
        }

        /// <summary>Renders the item.</summary>
        /// <param name="output">The output.</param>
        /// <param name="item">The item to render.</param>
        /// <contract>
        ///   <requires name="output" condition="not null" />
        ///   <requires name="item" condition="not null" />
        /// </contract>
        protected override void RenderItem(HtmlTextWriter output, Item item)
        {
          Assert.ArgumentNotNull((object) output, "output");
          Assert.ArgumentNotNull((object) item, "item");
          string str1 = string.Empty;
          string str2 = string.Empty;
          string str3 = string.Empty;
          if (UploadedItems.Include(item))
            UploadedItems.RenewExpiration();
          string str4;
          int num1;
          if (item.TemplateID == TemplateIDs.MediaFolder || item.TemplateID == TemplateIDs.Folder || item.TemplateID == TemplateIDs.Node)
          {
            str4 = Themes.MapTheme("Applications/48x48/folder.png");
            num1 = 48;
            int num2 = UserOptions.View.ShowHiddenItems ? item.Children.Count : this.GetVisibleChildCount(item);
            str1 = num2.ToString() + " " + Translate.Text(num2 == 1 ? "item" : "items");
          }
          else
          {
            MediaItem mediaItem = (MediaItem) item;
            MediaUrlOptions thumbnailOptions = MediaUrlOptions.GetThumbnailOptions((MediaItem) item);
            num1 = MediaManager.HasMediaContent((Item) mediaItem) ? 72 : 48;
            thumbnailOptions.Width = num1;
            thumbnailOptions.Height = num1;
            thumbnailOptions.UseDefaultIcon = true;
            str4 = MediaManager.GetMediaUrl(mediaItem, thumbnailOptions);
            MediaMetaDataFormatter metaDataFormatter = MediaManager.Config.GetMetaDataFormatter(mediaItem.Extension);
            if (metaDataFormatter != null)
            {
              MediaMetaDataCollection metaData1 = mediaItem.GetMetaData();
              MediaMetaDataCollection metaData2 = new MediaMetaDataCollection();
              foreach (string key in metaData1.Keys)
                metaData2[key] = HttpUtility.HtmlEncode(metaData1[key]);
              if (str1 != null)
                str1 = metaDataFormatter.Format(metaData2, MediaMetaDataFormatterOutput.HtmlNoKeys);
            }
            str2 = new MediaValidatorFormatter().Format(mediaItem.ValidateMedia(), MediaValidatorFormatterOutput.HtmlPopup);
            ItemLink[] referrers = Globals.LinkDatabase.GetReferrers(item);
            if (referrers.Length > 0)
              str3 = referrers.Length.ToString() + " " + Translate.Text(referrers.Length == 1 ? "usage" : "usages");
          }
          Tag tag = new Tag("a");
          tag.Add("id", "I" + (object) item.ID.ToShortID());
          tag.Add("href", "#");
          tag.Add("onclick", "javascript:scForm.getParentForm().invoke('item:load(id=" + (object) item.ID + ")');return false");
          if (UploadedItems.Include(item))
            tag.Add("class", "highlight");
          tag.Start(output);
          ImageBuilder imageBuilder1 = new ImageBuilder();
          imageBuilder1.Src = str4;
          imageBuilder1.Class = "scMediaIcon";
          imageBuilder1.Width = num1;
          imageBuilder1.Height = num1;
          ImageBuilder imageBuilder2 = imageBuilder1;
          string str5 = string.Empty;
          if (num1 < 72)
            str5 = string.Format("padding:{0}px {0}px {0}px {0}px", (object) ((72 - num1) / 2));
          if (!string.IsNullOrEmpty(str5))
            str5 = " style=\"" + str5 + "\"";
          output.Write("<div class=\"scMediaBorder\"" + str5 + ">");
          output.Write(imageBuilder2.ToString());
          output.Write("</div>");
          output.Write("<div class=\"scMediaTitle\">" + item.DisplayName + "</div>");
          if (!string.IsNullOrEmpty(str1))
            output.Write("<div class=\"scMediaDetails\">" + str1 + "</div>");
          if (!string.IsNullOrEmpty(str2))
            output.Write("<div class=\"scMediaValidation\">" + str2 + "</div>");
          if (!string.IsNullOrEmpty(str3))
            output.Write("<div class=\"scMediaUsages\">" + str3 + "</div>");
          tag.End(output);
        }

        /// <summary>Gets the non hidden child count.</summary>
        /// <param name="item">The item to get children count.</param>
        /// <returns>The visible child count.</returns>
        private int GetVisibleChildCount(Item item)
        {
          int num = 0;
          foreach (Item child in item.Children)
          {
            if (!child.Appearance.Hidden)
              ++num;
          }
          return num;
        }

        /// <summary>Renders the settings.</summary>
        /// <param name="settings">The settings.</param>
        private void RenderSettings(Dictionary<string, string> settings)
        {
          Assert.ArgumentNotNull((object) settings, "settings");
          StringWriter stringWriter = new StringWriter();
          using (JsonWriter jsonWriter = (JsonWriter) new JsonTextWriter((TextWriter) stringWriter))
          {
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Converters.Add((JsonConverter) new XmlNodeConverter());
            jsonSerializer.Serialize(jsonWriter, (object) settings);
          }

          //this.SettingsContainer.InnerHtml = "<script type='text/javascript'>var scUploadSettings = {0};</script>".FormatWith((object)stringWriter.ToString());
          string x = stringWriter.ToString();
          this.SettingsContainer.InnerHtml = String.Format("<script type='text/javascript'>var scUploadSettings = {0};</script>",x);          
        }
    }
}