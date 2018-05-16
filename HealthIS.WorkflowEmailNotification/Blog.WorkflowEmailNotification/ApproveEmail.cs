using System;
using HealthIS.Apps;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows.Simple;
using System.Collections.Generic;
using System.Linq;

namespace Blog.WorkflowEmailNotification
{
    public class ApproveEmail
    {
        // Get special valuables and update with correct data
        private string GetText(Item commandItem, string field, WorkflowPipelineArgs args)
        {
            string text = commandItem[field];
            if (text.Length > 0)
                return this.ReplaceVariables(text, args);
            return string.Empty;
        }

        // Replace special valuables in template
        private string ReplaceVariables(string data, WorkflowPipelineArgs args)
        {
            string publishedDate = "Not Available";
            if (args.DataItem.Fields["pubDate"] != null)
            {
                DateTime datetime = Sitecore.DateUtil.IsoDateToDateTime(args.DataItem.Fields["pubDate"].Value);
                publishedDate = Sitecore.DateUtil.FormatShortDateTime(datetime);
            }

            data = data.Replace("$userFullName$", User.Current.Profile.FullName);
            data = data.Replace("$itemPath$", args.DataItem.Paths.FullPath);
            data = data.Replace("$itemVersion$", args.DataItem.Version.ToString());
            data = data.Replace("$itemPublishedDate$", publishedDate);
            data = data.Replace("$itemUpdatedDate$", args.DataItem.Statistics.Updated.ToString());
            return data;
        }

        // Access workflow process to get info
        public void Process(WorkflowPipelineArgs args)
        {
            ProcessorItem processorItem = args.ProcessorItem;
            
            // If item is not in processor and item doesn't have page layout, retrun
            if (processorItem == null)
                return;

            Item innerItem = processorItem.InnerItem;
            
            // Check the condition before excuting
            Assert.ArgumentNotNull((object)args, "args");

            string fullPath = innerItem.Paths.FullPath;
            string fromAddress = "noreply@support.usf.edu";
            string toAddress = User.Current.Profile.Email;

            if (String.IsNullOrEmpty(toAddress))
                return;

            string subject = this.GetText(innerItem, "subject", args);
            string message = this.GetText(innerItem, "message", args);

            Error.Assert(toAddress.Length > 0, "The 'To' field is not specified in the mail action item: " + fullPath);
            Error.Assert(subject.Length > 0, "The 'Subject' field is not specified in the mail action item: " + fullPath);
            Error.Assert(message.Length > 0, "The 'Message' field is not specified in the mail action item: " + fullPath);

            try
            {
                if (!String.IsNullOrEmpty(toAddress.Trim()) && !String.IsNullOrEmpty(fromAddress.Trim()))
                {
                    // Send email if no issue found
                    if (Util.sendEmail(toAddress, subject, message, fromAddress, "", "", true))
                    {
                        Sitecore.Diagnostics.Log.Info("Begin Blog info", this);
                        Sitecore.Diagnostics.Log.Info("fromAddress " + fromAddress, this);
                        Sitecore.Diagnostics.Log.Info("toAddress " + toAddress, this);
                        Sitecore.Diagnostics.Log.Info("subject " + subject, this);
                        Sitecore.Diagnostics.Log.Info("message " + message, this);
                        Sitecore.Diagnostics.Log.Info("End info", this);
                        Sitecore.Diagnostics.Log.Info("Email Successfully Sent!!", this);
                    }
                    else
                    {
                        SheerResponse.ShowError("Sending Email Failed. Please try to approve again", "");
                        args.AbortPipeline();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Sending Email Failed - Exception Error", ex, this);
            }
        }
    }
}