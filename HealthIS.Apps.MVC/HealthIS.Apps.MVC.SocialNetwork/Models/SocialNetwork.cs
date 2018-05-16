using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class SocialNetwork : IRenderingModel
    {
        public string facebook { get; set; }
        public string twitter { get; set; }
        public string youTube { get; set; }
        public string linkedIn { get; set; }
        public string googlePlus { get; set; }
        public string pInterest { get; set; }
        public string instagram { get; set; }
        public string flickr { get; set; }
        public string rss { get; set; }
        public string podcast { get; set; }
        public string sharePoint { get; set; }
        public string largeIconClass { get { return "social-icons"; } }
        public string smallIconClass { get { return "social-icons-small"; } }
        public Sitecore.Data.Fields.CheckboxField chSmallIcon { get; set; }
        public Sitecore.Data.Templates.TemplateField chSmallIconTemplateField { get; set; }
        public string chSmallIconTemplateId { get; set; }
        public string cssClassName { get; set; }

        public Item Item { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item PageItem { get; set; }
        public bool dsIsSet { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;
            dsIsSet = Sitecore.Data.ID.IsID(Rendering.DataSource);

            if (dsIsSet)
            {
                facebook = FieldRenderer.Render(Item, "Facebook");
                twitter = FieldRenderer.Render(Item, "Twitter");
                youTube = FieldRenderer.Render(Item, "YouTube");
                linkedIn = FieldRenderer.Render(Item, "LinkedIn");
                googlePlus = FieldRenderer.Render(Item, "GooglePlus");
                pInterest = FieldRenderer.Render(Item, "Pinterest");
                instagram = FieldRenderer.Render(Item, "Instagram");
                flickr = FieldRenderer.Render(Item, "Flickr");
                rss = FieldRenderer.Render(Item, "RSS");
                podcast = FieldRenderer.Render(Item, "Podcast");
                sharePoint = FieldRenderer.Render(Item, "SharePoint");
                chSmallIcon = Item.Fields["isSmallIcon"];
                
                chSmallIconTemplateField = Item.Fields["isSmallIcon"].GetTemplateField();
                chSmallIconTemplateId = chSmallIconTemplateField.ID.Guid.ToString().ToUpper();

                cssClassName = (chSmallIcon.Checked ? smallIconClass : largeIconClass);
            }
        }
    }
}