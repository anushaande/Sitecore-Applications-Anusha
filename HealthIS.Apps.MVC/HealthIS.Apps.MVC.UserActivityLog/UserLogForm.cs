using Sitecore;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.IO;
using Sitecore.Shell.Framework;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using System;
using System.IO;
using System.Web;

namespace HealthIS.Apps.MVC.UserActivityLog
{
    public class UserLogForm : BaseForm
    {
        protected Frame Document;
        protected Sitecore.Web.UI.HtmlControls.Action HasFile;

        protected override void OnLoad(EventArgs e)
        {
          Assert.ArgumentNotNull((object) e, "e");
          base.OnLoad(e);
          Assert.CanRunApplication("/sitecore/content/Applications/Tools/User Log");
        }

        [HandleMessage("userlog:delete", true)]
        public void Delete(ClientPipelineArgs args)
        {
          Assert.ArgumentNotNull((object) args, "args");
          string @string = StringUtil.GetString(Context.ClientPage.ServerProperties["File"]);
          if (args.IsPostBack)
          {
            if (!(args.Result == "yes"))
              return;
            string path = FileUtil.MapPath(UserLogForm.GetFile(@string));
            if (string.IsNullOrEmpty(path))
              return;
            try
            {
              File.Delete(path);
              Log.Audit((object) this, "Delete log file {0}", new string[1]
              {
                path
              });
              this.SetFile(string.Empty);
            }
            catch (Exception ex)
            {
              SheerResponse.Alert("The file could not be deleted.\n\nError message:\n" + ex.Message);
            }
          }
          else if (Context.ClientPage.ServerProperties["File"] == null)
          {
            Context.ClientPage.ClientResponse.Alert("You must open a log file first.");
          }
          else
          {
            SheerResponse.Confirm(Translate.Text("Are you sure you want to delete \"{0}\"?", (object) @string));
            args.WaitForPostBack();
          }
        }

        [HandleMessage("userlog:download")]
        public void Download(Message message)
        {
          Assert.ArgumentNotNull((object) message, "message");
          string file1 = Context.ClientPage.ServerProperties["File"] as string;
          if (file1 == null)
          {
            Context.ClientPage.ClientResponse.Alert("You must open a log file first.");
          }
          else
          {
            if (string.IsNullOrEmpty(file1))
              return;
            string file2 = UserLogForm.GetFile(file1);
            if (string.IsNullOrEmpty(file2))
              return;
            Files.Download(TempFolder.CreateTempFile(file2));
          }
        }

        [HandleMessage("userlog:open", true)]
        public void Open(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull((object) args, "args");
            if (args.IsPostBack)
            {
                if (args.Result.Length <= 0 || !(args.Result != "undefined"))
                    return;
                string file = UserLogForm.GetFile(args.Result);
                if (string.IsNullOrEmpty(file))
                    SheerResponse.Alert("You can only open log files.");
                else
                    this.SetFile(file);
                }
            else
            {
                SheerResponse.Alert("File location: " + Settings.LogFolder);
                Files.ListFiles("Open Log File", " ", "Software/32x32/text_code_colored.png", "Open", Settings.LogFolder, "*log*.txt", false);
                args.WaitForPostBack();
            }
        }

        [HandleMessage("userlog:refresh")]
        public void Refresh(Message message)
        {
          Assert.ArgumentNotNull((object) message, "message");
          this.SetFile(StringUtil.GetString(Context.ClientPage.ServerProperties["File"]));
        }

        [HandleMessage("userlogaudit:zip")]
        public void Zip(Message message)
        {
          Assert.ArgumentNotNull((object) message, "message");
          string file1 = Context.ClientPage.ServerProperties["File"] as string;
          if (file1 == null)
          {
            Context.ClientPage.ClientResponse.Alert("You must open a log file first.");
          }
          else
          {
            if (string.IsNullOrEmpty(file1))
              return;
            string file2 = UserLogForm.GetFile(file1);
            if (string.IsNullOrEmpty(file2))
              return;
            Files.Zip(string.Empty, new string[1]
            {
              file2
            });
            Context.ClientPage.ClientResponse.Alert("The log file has been zipped.");
          }
        }

        private static string GetFile(string file)
        {
            if (string.IsNullOrEmpty(file) || !FileUtil.MapPath(file).StartsWith(FileUtil.MapPath(Settings.LogFolder), StringComparison.InvariantCultureIgnoreCase))
                return string.Empty;
            return file;
        }

        private void SetFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                Context.ClientPage.ServerProperties["File"] = (object) null;
                this.Document.SetSource("control:UserLogDetails", string.Empty);
                Context.ClientPage.ClientResponse.SetInnerHtml("Commandbar_CommandbarTitle", Translate.Text("Log Files"));
                Context.ClientPage.ClientResponse.SetInnerHtml("Commandbar_CommandbarDescription", Translate.Text("This tool displays the content of log files."));
                this.HasFile.Disabled = true;
            }
            else
            {
                Context.ClientPage.ServerProperties["File"] = (object) filename;
                this.Document.SetSource("control:UserLogDetails", "file=" + HttpUtility.UrlEncode(filename));
                FileInfo fileInfo = new FileInfo(FileUtil.MapPath(filename));
                Context.ClientPage.ClientResponse.SetInnerHtml("Commandbar_CommandbarTitle", Path.GetFileNameWithoutExtension(filename));
                Context.ClientPage.ClientResponse.SetInnerHtml("Commandbar_CommandbarDescription", Translate.Text(Translate.Text("Last access: {0}") + "<br/>", (object) DateUtil.FormatShortDateTime(DateUtil.ToServerTime(fileInfo.LastWriteTimeUtc))) + Translate.Text("Size: {0}", (object) MainUtil.FormatSize(fileInfo.Length)));
                this.HasFile.Disabled = false;
            }
        }
    }
}
