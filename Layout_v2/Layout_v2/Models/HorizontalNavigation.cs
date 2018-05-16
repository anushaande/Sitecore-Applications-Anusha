using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Web;

namespace Layout_v2.Models
{
    public class HorizontalNavigation : IRenderingModel
    {
        // NavBarHorizontal Template
        public string H_NavBarClass { get; set; }
        public string H_LabelH1Class { get; set; }
        public string H_LabelIClass { get; set; }
        public string H_NavBarLabel { get; set; }
        public string H_UlClass { get; set; }

        public int ListCount { get; set; }
        public List<HtmlString> LiList { get; set; }
        public HtmlString HsTag { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public HtmlString Error { get; set; }
        public Boolean isEmpty { get; set; }
        public string NewTab { get; set; }

        public void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            Rendering = rendering;
            Item = rendering.Item;
            PageItem = PageContext.Current.Item;

            // NavBarHorizontal Template
            H_NavBarClass = Item["Nav Bar Class"];
            H_LabelH1Class = Item["Label H1 Class"];
            H_LabelIClass = Item["Label i Class"];
            H_NavBarLabel = Item["Nav Bar Label"];
            H_UlClass = Item["ul Class"];
            LiList = new List<HtmlString>();
            HsTag = new HtmlString("");
            ListCount = 0;
            Error = new HtmlString("<center><br /><h2>Horizontal Navigation<br />Please set associated content or edit related item</h2><br /></center>");
            isEmpty = false;
            NewTab = "";

            if (String.IsNullOrEmpty(Rendering.DataSource) || !Item.HasChildren)
            {
                isEmpty = true;
            }
        }
    }
}