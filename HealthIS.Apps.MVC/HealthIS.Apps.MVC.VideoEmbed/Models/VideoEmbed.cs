using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Models
{
    public class VideoEmbed : IRenderingModel
    {
        public string Title { get; set; }
        public string URL { get; set; }
        public string CSS { get; set; }
        public string LightboxTitle { get; set; }
        public string ThumbnailImage { get; set; }

        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = Sitecore.Mvc.Presentation.PageContext.Current.Item;

            Title = FieldRenderer.Render(Item, "Title");
            URL = FieldRenderer.Render(Item, "URL");
            CSS = FieldRenderer.Render(Item, "CSS");
            LightboxTitle = FieldRenderer.Render(Item, "LightboxTitle");
            ThumbnailImage = FieldRenderer.Render(Item, "ThumbnailImage", "class=video-image");
        }
    }
}