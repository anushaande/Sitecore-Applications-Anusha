using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Forms
{
    public class HISForm
    {

        public static string getMessage(Control c)
        {
            string msg = "";
            string br = "\n\n";
            if (c is TextBox)
            {
                TextBox t = c as TextBox;

                if (t != null)
                {
                    List<string> idInfo = t.ID.Split('_').ToList<string>();
                    idInfo.RemoveAt(0);
                    string key = String.Join(" ", idInfo.ToArray());
                    msg += key + ": " + t.Text + br;
                }
            }
            else if (c is DropDownList)
            {
                DropDownList ddl = c as DropDownList;
                if (ddl != null)
                {
                    List<string> idInfo = ddl.ID.Split('_').ToList<string>();
                    idInfo.RemoveAt(0);
                    string key = String.Join(" ", idInfo.ToArray());
                    msg += key + ": " + ddl.SelectedValue + br;
                }
            }
            else if (c is CheckBoxList)
            {
                CheckBoxList chb = c as CheckBoxList;
                if (chb != null)
                {
                    List<string> idInfo = chb.ID.Split('_').ToList<string>();
                    idInfo.RemoveAt(0);
                    string key = String.Join(" ", idInfo.ToArray());
                    msg += br + key + ": " + br;

                    foreach (ListItem item in chb.Items)
                    {
                        if (item.Selected)
                        {
                            msg += " - " + item.Text + br;
                        }
                    }
                    msg += br;

                }
            }
            else if (c is RadioButtonList)
            {
                RadioButtonList rbl = c as RadioButtonList;
                if (rbl != null)
                {
                    List<string> idInfo = rbl.ID.Split('_').ToList<string>();
                    idInfo.RemoveAt(0);
                    string key = String.Join(" ", idInfo.ToArray());
                    msg += key + ": " + rbl.SelectedValue + br;
                }
            }
            else if (c.HasControls())
            {
                foreach (Control cc in c.Controls)
                {
                    msg += getMessage(cc);
                }
            }
            return msg;
        }

        public static void SubmitForm(Panel FormPanel, Panel CompletionPanel, string from, string to, string sub, HttpFileCollection fileCollection = null)
        {
            FormPanel.Visible = false;
            string br = "\n";
            string msg = "";
            foreach (Control ctrl in FormPanel.Controls)
            {
                msg += getMessage(ctrl);
            }

            if (fileCollection != null && fileCollection.Count > 0)
            {
                List<System.Net.Mail.Attachment> attachments = new List<System.Net.Mail.Attachment>();
                for (int i = 0; i < fileCollection.Count; i++)
                {
                    System.Net.Mail.Attachment a = new System.Net.Mail.Attachment(fileCollection[i].InputStream, fileCollection[i].FileName, fileCollection[i].ContentType);
                    System.Net.Mime.ContentDisposition cd = a.ContentDisposition;
                    cd.FileName = fileCollection[i].FileName;
                    attachments.Add(a);
                }
                Util.sendEmail(to, sub, msg, from, attachments, false);
            }
            else
            {
                Util.sendEmail(to, sub, msg, from, null, false);
            }
            msg += "From: " + from + br;
            msg += "To: " + to + br;
            msg += "Subject: " + sub + br;

            CompletionPanel.Visible = true;
        }

        public static void ClearTextBoxes(Control p1)
        {
            foreach (Control ctrl in p1.Controls)
            {
                if (ctrl is TextBox)
                {
                    TextBox t = ctrl as TextBox;

                    if (t != null)
                    {
                        t.Text = String.Empty;
                    }
                }
                else if (ctrl is DropDownList)
                {
                    DropDownList ddl = ctrl as DropDownList;
                    if (ddl != null)
                    {
                        ddl.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (ctrl.Controls.Count > 0)
                    {
                        ClearTextBoxes(ctrl);
                    }
                }
            }
        }
    }
}