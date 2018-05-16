using System;
using HealthIS.Apps;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows.Simple;
using Sitecore;

namespace HealthIS.WorkflowEmailNotification
{
    public class RequestEmail
    {
        // Access workflow process to get info
        public void Process(WorkflowPipelineArgs args)
        {
            ProcessorItem processorItem = Helper.ValidateArgsProcessorItem(args);
            if (processorItem == null) return;            
            
            Item innerItem = processorItem.InnerItem;
            UpdateWorkflowState update = new UpdateWorkflowState();
            
            // Get "To Addresses" for request email
            string toAddress = Helper.GetToAddressesForRequestEmail(User.Current, args.DataItem.Paths.FullPath);
            if (String.IsNullOrEmpty(toAddress))
            {                
                toAddress = Helper.GetText(innerItem, "to", args); // no emails found, send to default toAddress specified in workflow action
            }

            string fullPath = innerItem.Paths.FullPath;
            string fromAddress = User.Current.Profile.Email;
            string subject = Helper.GetText(innerItem, "subject", args);
            string message = Helper.GetText(innerItem, "message", args);

            Sitecore.Diagnostics.Log.Info("HealthIS WP- " + fullPath + " - " + fromAddress + " - " + toAddress + " - " + subject, this);

            Error.Assert(toAddress.Length > 0, "The 'To' field is not specified in the mail action item: " + fullPath);
            Error.Assert(subject.Length > 0, "The 'Subject' field is not specified in the mail action item: " + fullPath);
            Error.Assert(message.Length > 0, "The 'Message' field is not specified in the mail action item: " + fullPath);

            // Update main page item and its resource folder
            update.ChangeWorkflowState(args.DataItem, "Awaiting Approval", true);

            // Find all controls on item
            foreach (string control in update.GetDatasourceValue(args, args.DataItem))
            {
                if (!String.IsNullOrEmpty(control))
                {
                    Item ctrl = args.DataItem.Database.GetItem(control);

                    if (ctrl != null)
                    {
                        update.ChangeWorkflowState(ctrl, "Awaiting Approval", false);
                    }
                }
            }

            try
            {
                // Post Slack Message
                PostSlackMessage.PostMessage(args.DataItem, User.Current, false, args.CommentFields["Comments"].ToString());

                if (!String.IsNullOrEmpty(toAddress.Trim()) && !String.IsNullOrEmpty(fromAddress.Trim()))
                {
                    // Send email if no issue found
                    if (Util.sendEmail(toAddress, subject, message, fromAddress, "", "", true))
                    {
                        Sitecore.Diagnostics.Log.Info("Begin info", this);
                        Sitecore.Diagnostics.Log.Info("fromAddress " + fromAddress, this);
                        Sitecore.Diagnostics.Log.Info("toAddress " + toAddress, this);
                        Sitecore.Diagnostics.Log.Info("subject " + subject, this);
                        Sitecore.Diagnostics.Log.Info("message " + message, this);
                        Sitecore.Diagnostics.Log.Info("End info", this);
                        Sitecore.Diagnostics.Log.Info("Email Successfully Sent!!", this);
                    }
                    else
                    {
                        SheerResponse.ShowError("Sending Email Failed. Please try to save and submit again", "");
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