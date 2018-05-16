using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Web;

namespace Layout_v2.Models
{
    public class SlideShow : IRenderingModel
    {
        public string Title { get; set; }
        public string StyleClass { get; set; }
        public string HtmlID { get; set; }
        public string TransitionTime { get; set; }
        public string PauseOnHover { get; set; }
        public string FadeTransition { get; set; }
        public string SlideShowNoArrow { get; set; }
        public string SlideShowIndicator { get; set; }
        //public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public HtmlString Error { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Item = rendering.Item;
            
            Title = Item["Title"];
            StyleClass = Item["StyleClass"];
            HtmlID = Item["htmlID"];
            TransitionTime = Item["TransitionTime"];
            PauseOnHover = (String.IsNullOrEmpty(Item["PauseOnHover"]) ? "" : "hover");
            FadeTransition = (String.IsNullOrEmpty(Item["FadeTransition"]) ? "" : "slideshow-fade");
            SlideShowNoArrow = (String.IsNullOrEmpty(Item["SlideShowNoArrow"]) ? "" : "slideshow-no-arrows");
            SlideShowIndicator = (String.IsNullOrEmpty(Item["SlideShowIndicator"]) ? "" : "slideshow-no-indicators");
            Error = new HtmlString("<center><br /><h3>Slide Show<br />Please set associated content or edit related item</h3><br /></center>");
        }
    }
}