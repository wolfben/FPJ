using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

namespace FPJ.AuthCore
{
    /// <summary>
    /// 当前上下文自定义用户对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomerPrincipal<T> : IPrincipal
    {
        public T UserData { get; set; }

        public FormsAuthenticationTicket Ticket { get; set; }

        private IIdentity _identity;

        public IIdentity Identity
        {
            get
            {
                return _identity;
            }
        }

        public CustomerPrincipal()
        {
            Init();
        }


        public bool IsInRole(string role)
        {
            return true;
        }

        public void Init()
        {
            if (HttpContext.Current == null)
            {
                throw new ArgumentNullException("HttpContext");
            }

            // 1. 读登录Cookie
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                return;

            try
            {
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {
                    UserData = JsonConvert.DeserializeObject<T>(ticket.UserData);
                }

                if (ticket != null && UserData != null)
                {
                    this.Ticket = ticket;
                    this.UserData = UserData;
                    this._identity = new FormsIdentity(ticket);
                    HttpContext.Current.User = this;
                }
            }
            catch
            {
                /* 有异常也不要抛出，防止攻击者试探。 */
            }
        }
    }
}
