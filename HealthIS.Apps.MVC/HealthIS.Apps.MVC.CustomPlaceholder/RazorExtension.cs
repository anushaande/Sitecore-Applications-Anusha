using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC
{
    public static class RazorExtension
    {
        public static HtmlString DynamicPlaceholder(this Sitecore.Mvc.Helpers.SitecoreHelper helper, string dynamicKey)
        {
            var currentRenderingId = RenderingContext.Current.Rendering.UniqueId;
            //return helper.Placeholder(string.Format("{0}_dph_{1}", dynamicKey, currentRenderingId));
            return helper.Placeholder(dynamicKey);
        }
    }
}
