using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Forms
{
    public partial class IMpact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (ViewState["pnlAuthors"] != null)
                {
                    var tbContent = ViewState["pnlAuthors"] as List<string>;
                    foreach (string tbc in tbContent)
                    {
                        string[] content = tbc.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        if (pnlAuthors.Controls.OfType<TextBox>().Where(t => t.ID == content[0]).ToList().Count == 0)
                        {
                            TextBox tb = new TextBox();
                            tb.ID = content[0];
                            tb.Text = content.Length > 1 ? content[1] : "";
                            tb.Attributes.Add("placeholder", "Author/Institution");
                            tb.CssClass = "form-control";
                            pnlAuthors.Controls.Add(tb);
                        }
                    }
                }

                if (Session["fu_File"] == null && fu_File.HasFiles)
                {
                    Session["fu_File"] = fu_File;
                }
      
                else if (Session["fu_File"] != null && (!fu_File.HasFiles))
                {
                    fu_File = (FileUpload)Session["fu_File"];
                }

                else if (fu_File.HasFiles)
                {
                    Session["fu_File"] = fu_File;
                }

                if (!ScriptManager.GetCurrent(this.Page).IsInAsyncPostBack)//is full postback
                {
                    if (Request.Files != null && Request.Files.Count > 0)//has files
                    {
                        sessionizeFile(Request.Files);
                    }
                    else if (Session["Files"] != null)
                    {
                        sessionizeFile((HttpFileCollection)Session["Files"]);
                    }
                    else
                    {
                        lblFiles.Text = "No files posted or found.";
                    }
                }
                else{
                    if (Session["Files"] != null)
                    {
                        sessionizeFile((HttpFileCollection)Session["Files"]);
                    }
                    else
                    {
                        lblFiles.Text = "No files found.";
                    }
                }
            }
            
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ResizeIFrame", "resizeIframe(window)", true);
        }

        protected void sessionizeFile(HttpFileCollection fileCol)
        {
            try
            {
                if (fileCol != null && fileCol.Count > 0)
                {
                    if (isAcceptableUploadSize(fileCol))
                    {
                        var formats = new string[] { ".doc", ".docx", ".pdf", ".jpg", ".tif" };
                        List<string> fileNames = new List<string>();
                        for (int i = 0; i < fileCol.Count; i++)
                        {
                            var f = fileCol[i];
                            if (f != null)
                            {
                                string fileName = f.FileName;
                                string format = System.IO.Path.GetExtension(fileName).ToLower().Trim();
                                if (formats.Contains(format))
                                {
                                    fileNames.Add(fileName);
                                }
                                else throw new Exception("File '" + fileName + "' is not an acceptable file format.<BR>Accepted formats: .doc, .docx, .pdf, .jpg, .tif");
                            }
                            else throw new Exception("File not found.");
                        }
                        lblFiles.Text = "<b>Files uploaded:</b></br>&nbsp;&nbsp;&#8226;&nbsp;" + string.Join("</br>&nbsp;&nbsp;&#8226;&nbsp;", fileNames);
                        Session["Files"] = fileCol;
                    }
                    else throw new Exception("Total file upload size greater than 10 MB. Please try again.");                    
                }
                else throw new Exception("File collection null.");
            }
            catch (Exception ex)
            {
                lblFiles.Text = " ERROR: " + ex.Message;
            }
        }

        protected void btnAddAuthors_Click(object sender, EventArgs e)
        {
            var tb = new TextBox();
            tb.ID = "tb_Author_Institution_" + (pnlAuthors.Controls.OfType<TextBox>().ToList().Count + 1);
            tb.Attributes.Add("placeholder", "Author/Institution");
            tb.CssClass = "form-control";
            pnlAuthors.Controls.Add(tb);

            List<string> tbContent = new List<string>();
            foreach (Control c in pnlAuthors.Controls)
            {
                if (c is TextBox)
                {
                    TextBox _tb = (TextBox)c;
                    tbContent.Add(_tb.ID + "||" + _tb.Text);
                }
            }
            ViewState["pnlAuthors"] = tbContent;

            udpAuthors.Update();
            Page.ClientScript.RegisterStartupScript(this.GetType(), "ResizeIFrame", "resizeIframe(window)", true);
        }

        protected void btnAddFiles_Click(object sender, EventArgs e)
        {
            int i = (pnlFiles.Controls.OfType<FileUpload>().ToList().Count + 1);
            
            var fu = new FileUpload();
            fu.ID = "fu_File_" + i;
            fu.CssClass = "form-control";
            pnlFiles.Controls.Add(fu);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string from = Request.Form["tbAuth_Email"].ToString().ToLower();
            // string from = Sitecore.Configuration.Settings.GetSetting("IMpactFormEmailFromAddr", "");
            string to = Sitecore.Configuration.Settings.GetSetting("IMpactFormEmailToAddr", "");
            string sub = Sitecore.Configuration.Settings.GetSetting("IMpactFormEmailSubject", "");
            try
            {
                if (Session["Files"] != null)
                {
                    HttpFileCollection col = null;
                    try
                    {
                        col = (HttpFileCollection)Session["Files"];
                    }
                    catch { }
                    if (col != null && col.Count > 0)
                    {
                        HISForm.SubmitForm(pnlFormStart, pnlFormComplete, from, to, sub, (HttpFileCollection)Session["Files"]);
                        litComplete.Text += "<br/>Files sent!";
                    }
                    else
                    {
                        HISForm.SubmitForm(pnlFormStart, pnlFormComplete, from, to, sub, null);
                        //litComplete.Text += "<br/>Form sent!";
                    }
                }
                else
                {
                    HISForm.SubmitForm(pnlFormStart, pnlFormComplete, from, to, sub, null);
                    //litComplete.Text += "<br/>Form sent!";
                }
            }
            catch (Exception ex)
            {
                string msg = "";
                msg += "ERROR: " + ex.Message + "\n" + ex.StackTrace;
                if (ex.InnerException != null) msg += "\nInner Exception: " + ex.InnerException.Message + "\n" + ex.InnerException.StackTrace;
                litError.Text = "Error sending form! Please try again later.";
                pnlFormComplete.Visible = true;
            }
        }

        protected void btnFU_Upload_Click(object sender, EventArgs e)
        {
            List<string> fuContent = new List<string>();
            foreach (Control c in pnlFiles.Controls)
            {
                if (c is FileUpload)
                {
                    FileUpload _fu = (FileUpload)c;
                    if (_fu.HasFiles)
                    {
                        Session["fu_File"] = fu_File;
                    }
                }
            }
        }

        protected bool isAcceptableUploadSize(HttpFileCollection fileCol)
        {
            if (fileCol != null && fileCol.Count > 0)
            {
                int totalFileSize = 0;
                for (int i = 0; i < fileCol.Count; i++)
                {
                    if (fileCol[i] != null) { totalFileSize += fileCol[i].ContentLength; }
                }
                if (totalFileSize > 10485760) { return false; } // total file upload size > 10 MB, show error message
                else return true;  // total file upload size =< 10 MB
            }
            else return false; // fileCol is null or fileCol.Count < 0
        }
    }
}