using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FPJ.Attributes;

namespace FPJ.API.Controllers
{
    public class UserController : ApiController
    {
        [OAuthFilter]
        public string Add()
        {
            return string.Empty;
        }
    }
}
