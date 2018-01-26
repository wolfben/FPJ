
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FPJ.AuthCore;

namespace FPJ.Web.Controllers
{
    public class HomeController : BaseController
    {


        public ActionResult Index()
        {
            return View();
        }

    }
}
