using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Linq;
using System.Web;

namespace HealthIS.Layouts.Models
{
    public class PageError
    {
        public Item currentItem = PageContext.Current.Item;        

        // Autosave
        public HtmlString executeAutosave()
        {
            if (Sitecore.Context.PageMode.IsExperienceEditor)
            {
                return new HtmlString("<script>Sitecore.PageModes.PageEditor.postRequest('webedit:save()')</script>");
            }
            return new HtmlString("");
        }
    }
}