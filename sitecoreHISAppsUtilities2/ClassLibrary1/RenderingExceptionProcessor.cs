using Sitecore.Mvc.Pipelines.MvcEvents.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SCSettings = Sitecore.Configuration.Settings;

namespace HealthIS.Apps
{
    class RenderingExceptionProcessor : ExceptionProcessor
    {
        public bool CustomRenderingBehaviourRequired { get; set; }

        public override void Process(ExceptionArgs args)
        {
            // if not configured to show friendly errors, 
            // do not handle any exceptions.
            if (SCSettings.CustomErrorsMode == System.Web.Configuration.CustomErrorsMode.Off
                || (SCSettings.CustomErrorsMode == System.Web.Configuration.CustomErrorsMode.RemoteOnly
                    && args.ExceptionContext.HttpContext.Request.IsLocal)) { return; }
            else { HandleError(args.ExceptionContext); }
        }

        protected virtual void HandleError(ExceptionContext exceptionContext)
        {
            try
            {
                var httpContext = exceptionContext.HttpContext;

                // Bail if we can't do anything; propogate the error for further processing. 
                if (httpContext == null)
                    return;
                if (!CustomRenderingBehaviourRequired)
                    return;
                if (exceptionContext.ExceptionHandled)
                    return;
                if (httpContext.IsCustomErrorEnabled)
                    exceptionContext.ExceptionHandled = true;

                var innerException = exceptionContext.Exception;

                string url = SCSettings.ErrorPage;
                HttpException httpException = innerException as HttpException;
                if (httpException.GetHttpCode() == 404)
                {
                    url = SCSettings.ItemNotFoundUrl;
                }

                // if configured to transfer, transfer, otherwise redirect.
                if (SCSettings.RequestErrors.UseServerSideRedirect)
                {
                    HttpContext.Current.Server.Transfer(url);
                }
                else
                {
                    Sitecore.Web.WebUtil.Redirect(url, false);
                }
            }
            catch (Exception ex)
            {
                string url = SCSettings.ErrorPage;
                url = Sitecore.Web.WebUtil.AddQueryString(url, new string[]{ "msg", ex.Message });
                Sitecore.Web.WebUtil.Redirect(url, false);
            }
        }
    }
}
