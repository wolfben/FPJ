using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utility.Helper;

namespace FPJ.Attributes
{
    public class LogExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"] + string.Empty;
            var action = filterContext.RouteData.Values["action"] + string.Empty;

            LogHelper.Error("异常：controller={0}，action={1}", filterContext.Exception, controller, action);

            if (!filterContext.ExceptionHandled)
            {
                if (filterContext.HttpContext.IsCustomErrorEnabled)
                {
                    filterContext.HttpContext.Response.Clear();
                    HttpException httpEx = filterContext.Exception as HttpException;
                    if (httpEx != null)
                    {
                        filterContext.HttpContext.Response.StatusCode = httpEx.GetHttpCode();
                        switch (httpEx.GetHttpCode())
                        {
                            case 404:
                                filterContext.Result = new RedirectResult("/error/error404"); break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        filterContext.HttpContext.Response.StatusCode = 500;
                    }

                    filterContext.ExceptionHandled = true;
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
            }
        }
    }
}
