using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Newtonsoft.Json;
using System.Web;

namespace FPJ.AuthCore
{
    /// <summary>
    /// 自定义登录身份认证（包装FormsAuthentication)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomerFormsAuthentication
    {
        /// <summary>
        /// 用户身份登录
        /// </summary>
        /// <param name="userName">用户唯一标识</param>
        /// <param name="expiration">过期时间（单位：分钟）</param>
        /// <param name="userData">用户登录附加数据</param>
        public static void SignIn(string userName, int? expiration, object userData = null)
        {
            string userDataJson = null;
            if (userData != null)
            {
                userDataJson = JsonConvert.SerializeObject(userData);
            }

            //创建一个登录用户的票据ticket
            FormsAuthenticationTicket formsTicket = new FormsAuthenticationTicket(
                2, userName, DateTime.Now, DateTime.Now.AddDays(1), true, userDataJson);

            //获取登录用户加密的cookie数据
            var cookieValue = FormsAuthentication.Encrypt(formsTicket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
            cookie.HttpOnly = true;
            if (expiration.HasValue)
            {
                cookie.Expires = DateTime.Now.AddMinutes(expiration.Value);
            }
            cookie.Domain = FormsAuthentication.CookieDomain;
            cookie.Path = FormsAuthentication.FormsCookiePath;

            HttpContext context = HttpContext.Current;
            if (context == null)
                throw new InvalidOperationException();

            // 写登录Cookie
            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);

        }

        /// <summary>
        /// 用户身份登录注销
        /// </summary>
        public static void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
