using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web.UI.WebControls;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System.Xml.Linq;
using System.Collections;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Text;

namespace HealthIS.Apps.MVC.Models
{
    public class RSS2 : IRenderingModel
    {
        // RSS Feed Section Template
        public string FeedTitle { get; set; }
        public string FeedURL { get; set; }
        public string ItemDescriptionCharacterLimit { get; set; }
        public string NumberOfItems { get; set; }
        public string NumberOfImages { get; set; }
        public string DisplayFeedTitle { get; set; }
        public string DisplayTitleFirst { get; set; }
        public string DisplayFeedDescription { get; set; }
        public Sitecore.Data.Fields.CheckboxField ChDisplayFeedTitle { get; set; }
        public Sitecore.Data.Fields.CheckboxField ChDisplayTitleFirst { get; set; }
        public Sitecore.Data.Fields.CheckboxField ChDisplayFeedDescription { get; set; }

        // Styling Section Template
        public string FeedCSSClass { get; set; }
        public string FeedItemCSSClass { get; set; }
        public string FeedTitleCSSClass { get; set; }
        public string FeedDescriptionCSSClass { get; set; }
        public string FeedItemTitleCSSClass { get; set; }
        public string FeedItemDescriptionCSSClass { get; set; }
        public string FeedItemPublicationDateCSSClass { get; set; }
        public string FeedItemImageCSSClass { get; set; }

        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public StringBuilder sbItems { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public string err { get; set; }
        public bool isDatasourceSet { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Item = rendering.Item;
            Rendering = rendering;
            PageItem = Sitecore.Mvc.Presentation.PageContext.Current.Item;
            isDatasourceSet = Sitecore.Data.ID.IsID(Rendering.DataSource);

            if (isDatasourceSet)
            {
                FeedTitle = FieldRenderer.Render(Item, "Feed Title");
                FeedURL = FieldRenderer.Render(Item, "Feed URL");
                ItemDescriptionCharacterLimit = FieldRenderer.Render(Item, "Item Description Character Limit");
                NumberOfItems = FieldRenderer.Render(Item, "Number of Items");
                NumberOfImages = FieldRenderer.Render(Item, "Number of Images");
                ChDisplayFeedTitle = Item.Fields["Display Feed Title"];
                ChDisplayTitleFirst = Item.Fields["Display Title First"];
                ChDisplayFeedDescription = Item.Fields["Display Feed Description"];

                FeedCSSClass = FieldRenderer.Render(Item, "Feed CSS Class");
                FeedItemCSSClass = FieldRenderer.Render(Item, "Feed Item CSS Class");
                FeedTitleCSSClass = FieldRenderer.Render(Item, "Feed Title CSS Class");
                FeedDescriptionCSSClass = FieldRenderer.Render(Item, "Feed Description CSS Class");
                FeedItemTitleCSSClass = FieldRenderer.Render(Item, "Feed Item Title CSS Class");
                FeedItemDescriptionCSSClass = FieldRenderer.Render(Item, "Feed Item Description CSS Class");
                FeedItemPublicationDateCSSClass = FieldRenderer.Render(Item, "Feed Item Publication Date CSS Class");
                FeedItemImageCSSClass = FieldRenderer.Render(Item, "Feed Item Image CSS Class");
            }
        }
    }
}