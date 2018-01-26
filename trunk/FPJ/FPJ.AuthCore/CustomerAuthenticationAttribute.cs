using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FPJ.Context;

namespace FPJ.AuthCore
{
    /// <summary>
    /// 自定义身份认证
    /// </summary>
    public class CustomerAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult(Config.Instance.LoginUrl + "?returnurl=" + returnUrl);
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
