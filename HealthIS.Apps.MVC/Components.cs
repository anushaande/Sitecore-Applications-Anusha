using System;
using System.Web.Mvc;
using HealthIS.Apps.MVC.Models;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.SecurityModel;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;

namespace HealthIS.Apps.MVC.Controllers
{
    public class Components : Controller
    {
        //
        // POST: /Components/

        #region RSS Feed Component
        public ActionResult RSS2(string changedValue, string fieldName)
        {
            RSS2 rr = new RSS2();
            rr.Initialize(RenderingContext.Current.Rendering);

            rr.DisplayFeedTitle = (rr.ChDisplayFeedTitle.Checked) ? "DisplayFeedTitle" : "";
            rr.DisplayTitleFirst = (rr.ChDisplayTitleFirst.Checked) ? "DisplayTitleFirst" : "";
            rr.DisplayFeedDescription = (rr.ChDisplayFeedDescription.Checked) ? "DisplayFeedDescription" : "";

            StringBuilder itemList = new StringBuilder();

            // Update Checkbox field if requested
            if (!String.IsNullOrEmpty(changedValue) && !String.IsNullOrEmpty(fieldName))
            {
                rr.Item.Editing.BeginEdit();
                using (new SecurityDisabler())
                {
                    switch (fieldName)
                    {
                        case ("DisplayFeedTitle"):
                            rr.ChDisplayFeedTitle.Checked = (changedValue == "True" ? true : false);
                            break;
                        case ("DisplayTitleFirst"):
                            rr.ChDisplayTitleFirst.Checked = (changedValue == "True" ? true : false);
                            break;
                        case ("DisplayFeedDescription"):
                            rr.ChDisplayFeedDescription.Checked = (changedValue == "True" ? true : false);
                            break;
                    }
                }
                rr.Item.Editing.EndEdit();
            }
            else
            {
                if (String.IsNullOrEmpty(rr.FeedURL) || String.IsNullOrEmpty(rr.Rendering.DataSource))
                {
                    rr.err = "Please add Datasource or Feed URL";
                    return PartialView(rr);
                }
                else
                {
                    rr.err += "Datasource: " + rr.Rendering.DataSource;

                    try
                    {
                        XmlReader reader = XmlReader.Create(rr.FeedURL);
                        SyndicationFeed feed = SyndicationFeed.Load(reader);

                        int count = 1;

                        foreach (SyndicationItem item in feed.Items)
                        {
                            string cutImage = "";
                            string cutDescription = "";
                            string feedDescription = "";

                            Regex regexImage = new Regex(@"src\s*=\s*""(.+?)""");
                            Match matchImage = regexImage.Match(item.Summary.Text);
                            cutDescription = Regex.Replace(item.Summary.Text, "<.*?>", "");

                            if (matchImage.Success) { cutImage = matchImage.Value; }
                            if (int.Parse(rr.ItemDescriptionCharacterLimit) > 0 && cutDescription.Length > int.Parse(rr.ItemDescriptionCharacterLimit))
                            {
                                cutDescription = cutDescription.Substring(0, int.Parse(rr.ItemDescriptionCharacterLimit)) + "[...]";
                            }

                            feedDescription = (rr.ChDisplayFeedDescription.Checked ? "<div>" +
                                                    "<span class='" + rr.FeedItemPublicationDateCSSClass + "'>" +
                                                        item.PublishDate.Date.ToString() +
                                                    "</span>" +
                                                    "<span class='" + rr.FeedItemDescriptionCSSClass + "'>" +
                                                        cutDescription +
                                                    "</span>" +
                                                    "</div>" : "");

                            itemList.Append(
                                "<div class='" + rr.FeedItemCSSClass + "'>" +
                                    "<div class='rssfeed-thumnail'><img " + cutImage + " class='" + rr.FeedItemImageCSSClass + "' /></div>" +
                                    "<div class='" + rr.FeedItemTitleCSSClass + "'>" +
                                        "<a href='" + item.Links[0].Uri.OriginalString + "'>" + item.Title.Text + "</a>" +
                                    "</div>" +
                                    feedDescription +
                                "</div>"
                            );

                            if (count == int.Parse(rr.NumberOfItems)) { break; }
                            count++;
                        }

                        rr.sbItems = itemList;
                    }
                    catch (Exception ex)
                    {
                        rr.err += "External RSS feed possibly not accessible: " + ex.Message;
                    }
                }
            }

            return PartialView(rr);
        }
        #endregion

        #region Slideshow CheckBox Update
        public ActionResult SlideShow(string changedValue, string fieldName)
        {
            SlideShow ss = new SlideShow();
            ss.Initialize(RenderingContext.Current.Rendering);

            // Update Checkbox field if requested
            if (!String.IsNullOrEmpty(changedValue) && !String.IsNullOrEmpty(fieldName))
            {
                ss.Item.Editing.BeginEdit();
                using (new SecurityDisabler())
                {
                    switch (fieldName)
                    {
                        case ("PauseOnHover"):
                            ss.chPauseOnHover.Checked = (changedValue == "True" ? true : false);
                            break;
                        case ("FadeTransition"):
                            ss.chFadeTransition.Checked = (changedValue == "True" ? true : false);
                            break;
                        case ("SlideShowNoArrow"):
                            ss.chSlideShowNoArrow.Checked = (changedValue == "False" ? true : false);
                            break;
                        case ("SlideShowNoIndicator"):
                            ss.chSlideShowNoIndicator.Checked = (changedValue == "False" ? true : false);
                            break;
                    }
                }
                ss.Item.Editing.EndEdit();
            }

            return PartialView(ss);
        }
        #endregion

        #region HorizontalNavigation Add Items
        public ActionResult HorizontalNavigation(string parentPath, string templatePath)
        {
            HorizontalNavigation hn = new HorizontalNavigation();
            hn.Initialize(RenderingContext.Current.Rendering);

            if (!String.IsNullOrEmpty(parentPath) && !String.IsNullOrEmpty(templatePath))
            {
                HealthIS.Apps.MVC.Helpers.createNewChildItem(hn, parentPath, templatePath);
            }

            return PartialView(hn);
        }
        #endregion
    }
}
