using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FPJ.AuthCore;
using FPJ.Web.ViewModels;

namespace FPJ.Web.Controllers
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cp = filterContext.HttpContext.User as CustomerPrincipal<UserLoginVM>;
            if (cp != null && cp.Identity != null && cp.Identity.IsAuthenticated)
            {
                ViewBag.CurrentUser = cp.UserData;
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
