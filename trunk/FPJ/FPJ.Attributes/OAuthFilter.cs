using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Utility.Helper;
using FPJ.Enum;
using FPJ.Context;
using FPJ.BLL.Default;
using FPJ.Model;

namespace FPJ.Attributes
{
    public class OAuthFilter : ActionFilterAttribute
    {

        protected readonly static OAuthBLL _OAuthBLL = new OAuthBLL();

        /// <summary>
        /// 是否进行签名认证
        /// </summary>
        private bool IsSign { get; set; }

        /// <summary>
        /// 请求参数集合
        /// </summary>
        private Dictionary<string, object> DicParameters;

        public OAuthFilter(bool isSign = true)
        {
            IsSign = isSign;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            EStatus status = EStatus.成功;
            string requestParametersString = RequestInputStreamToString(filterContext.HttpContext.Request.InputStream);

            if (string.IsNullOrEmpty(requestParametersString))
            {
                status = EStatus.未授权;
                EndResult(filterContext, status);
                return;
            }

            DicParameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestParametersString);

            //签名认证
            if (IsSign)
            {
                SignHelper signHelper = new SignHelper(Config.Instance.SignKey, DicParameters);
                if (signHelper.CheckSign() == false)
                {
                    status = EStatus.签名错误;
                    EndResult(filterContext, status);
                    return;
                }
            }

            //授权认证
            if (DicParameters.ContainsKey("access_token") == false)
            {
                status = EStatus.未授权;
                EndResult(filterContext, status);
                return;
            }

            var accessToken = DicParameters["access_token"].ToString();
            var oauth = _OAuthBLL.GetByAccessToken(accessToken);
            if (oauth == null || DateTime.Now > oauth.ExpireTime)
            {
                status = EStatus.未授权;
                EndResult(filterContext, status);
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        private void EndResult(ActionExecutingContext filterContext, EStatus status)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = new { code = (int)status, msg = status.ToString() };
            filterContext.Result = result;

            return;
        }

        private string RequestInputStreamToString(Stream stream)
        {
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = stream.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            stream.Flush();
            stream.Close();
            stream.Dispose();

            return builder.ToString();
        }
    }
}
