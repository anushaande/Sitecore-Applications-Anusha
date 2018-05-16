using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Pipelines;
using Sitecore.Pipelines.RenderField;
using Sitecore.Syndication;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Xml;
using Newtonsoft.Json;
using System.Net;
using Sitecore.Security.Accounts;

public static class PostSlackMessage
{
    public static void PostMessage(Item pageItem, User user, bool isPublished, string comment = null)
    {
        if (pageItem == null) {
            Sitecore.Diagnostics.Assert.IsNull(null, "Page Item can't be found!");
        }
        if (user == null) {
            Sitecore.Diagnostics.Assert.IsNull(null, "User can't be found!");
        }
        
        // Add condition to check if request is in QA2 or production
        if (System.Web.HttpContext.Current.Request.Url.Host != "cms.health.usf.edu")
        {
            return;
        }

        string pagePath = pageItem.Paths.Path.ToString().ToLower();
        pagePath = pagePath.Replace(Sitecore.Context.Data.Site.RootPath.ToLower(), "");
        pagePath = pagePath.Replace(Sitecore.Context.Data.Site.StartItem.ToLower(), "");

        string requestedByEmail = user.Profile.Email;
        string requestedByFullName = user.Profile.FullName;
        string updatedByEmail = User.FromName(pageItem.Statistics.UpdatedBy, true).Profile.Email;
        string updatedByFullName = User.FromName(pageItem.Statistics.UpdatedBy, true).Profile.FullName;
        string requestedDate = DateTime.Now.ToString("g");
        
        string pretextMsg = "Approval Request :file_cabinet:";
        string fallbackMsg = "ReferenceError - UI is not defined";
        string titleMsg = "<https://cms.health.usf.edu" + pagePath + "|" + pageItem.Fields["Page Title"].Value + "> - " + pagePath;
        string commentMsg = comment;
        string colorMsg = "#FF8000";
        
        if (isPublished)
        {
            pretextMsg = "Approved and Published :white_check_mark:";
            titleMsg = "<http://health.usf.edu" + pagePath + "|" + pageItem.Fields["Page Title"].Value + ">";
            colorMsg = "#36a64f";
        }
        var field = new[]{
                new {
                    title = (isPublished) ? "Approved by" : "Requested by",
                    value = "<mailto:" + requestedByEmail + "|" + requestedByFullName + ">",
                    @short = true
                },
                new {
                    title = "Updated by",
                    value = "<mailto:" + updatedByEmail + "|" + updatedByFullName + ">",
                    @short = true
                },
                new {
                    title = (isPublished) ? "Approved Date" : "Requested Date",
                    value = requestedDate,
                    @short = true
                },
                new {
                    title = "Page Version",
                    value = pageItem.Version.Number.ToString(),
                    @short = true
                }
        };

        var message = new
        {
            username = "Mr. Sitecore",
            channel = "G690XEDK4", // Private Channel "Sitcore Publishing"
            icon_emoji = ":sitecore:",
            attachments = new[]{ 
                new {
                    pretext = pretextMsg,
                    fallback = fallbackMsg,
			        title = titleMsg,
			        text = commentMsg,
                    color = colorMsg,
                    fields = field
                }
            }
        };

        var json = JsonConvert.SerializeObject(message);

        var webClient = new WebClient();
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        webClient.UploadString("https://hooks.slack.com/services/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", json);
    }
}