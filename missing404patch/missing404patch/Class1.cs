using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*namespace missing404patch
{
    public class Class1
    {
    }
}*/

namespace ParTech.Library.Pipelines
{
    public class ExecuteRequest : Sitecore.Pipelines.HttpRequest.ExecuteRequest
    {
        protected override void RedirectOnItemNotFound(string url)
        {
            var context = System.Web.HttpContext.Current;

            try
            {
                // Request the NotFound page
                string domain = context.Request.Url.GetComponents(UriComponents.Scheme | UriComponents.Host, UriFormat.Unescaped);
                string content = Sitecore.Web.WebUtil.ExecuteWebPage(string.Concat(domain, url));

                // Send the NotFound page content to the client with a 404 status code
                context.Response.TrySkipIisCustomErrors = true;
                context.Response.StatusCode = 404;
                context.Response.Write(content);
            }
            catch (Exception)
            {
                // If our plan fails for any reason, fall back to the base method
                base.RedirectOnItemNotFound(url);
            }

            // Must be outside the try/catch, cause Response.End() throws an exception
            context.Response.End();
        }
    }
}
