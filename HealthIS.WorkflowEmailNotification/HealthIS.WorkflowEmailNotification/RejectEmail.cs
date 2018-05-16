using System;
using HealthIS.Apps;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Web.UI.Sheer;
using Sitecore.Workflows.Simple;

namespace HealthIS.WorkflowEmailNotification
{
    public class RejectEmail
    {
        // Access workflow process to get info
        public void Process(WorkflowPipelineArgs args)
        {
            ProcessorItem processorItem = Helper.ValidateArgsProcessorItem(args);
            if (processorItem == null) return;

            Item innerItem = processorItem.InnerItem;
            UpdateWorkflowState update = new UpdateWorkflowState();

            User userWhoMadeUpdate = User.FromName(args.DataItem.Statistics.UpdatedBy, true);

            // Get "To Addresses" for rejection email
            string toAddress = "";
            string listOfAuthorsEmails = Helper.GetToAddressesForApproveRejectEmail(User.Current, args.DataItem.Paths.FullPath);
            if (!String.IsNullOrEmpty(listOfAuthorsEmails))
            {
                toAddress = userWhoMadeUpdate.Profile.Email + ";" + listOfAuthorsEmails;
            }
            // no other author emails found, only send to contributor who made original update
            else { toAddress = userWhoMadeUpdate.Profile.Email; }  

            string fullPath = innerItem.Paths.FullPath;
            string fromAddress = User.Current.Profile.Email;
            string subject = Helper.GetText(innerItem, "subject", args);
            string message = Helper.GetText(innerItem, "message", args);

            Error.Assert(subject.Length > 0, "The 'Subject' field is not specified in the mail action item: " + fullPath);
            Error.Assert(message.Length > 0, "The 'Message' field is not specified in the mail action item: " + fullPath);

            // Update main page item and its resource folder
            update.ChangeWorkflowState(args.DataItem, "Working", true);

            // Find all controls on item
            foreach (string control in update.GetDatasourceValue(args, args.DataItem))
            {
                if (!String.IsNullOrEmpty(control))
                {
                    Item ctrl = args.DataItem.Database.GetItem(control);
                    if (ctrl != null)
                    {
                        update.ChangeWorkflowState(ctrl, "Working", false);
                    }
                }
            }

            try
            {
                if (!String.IsNullOrEmpty(toAddress.Trim()) && !String.IsNullOrEmpty(fromAddress.Trim()))
                {
                    // Send email if no issue found
                    if (Util.sendEmail(toAddress, subject, message, fromAddress, "", "", true))
                    {
                        if (fromAddress == "support@health.usf.edu") { fromAddress = "User doesn't have email address"; }
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
                        SheerResponse.ShowError("Sending Email Failed. Please try to reject again", "");
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