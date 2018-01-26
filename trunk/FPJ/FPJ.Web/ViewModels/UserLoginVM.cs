using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPJ.Web.ViewModels
{
    /// <summary>
    /// 用户登录身份视图模型
    /// </summary>
    public class UserLoginVM
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        ///可自行扩展需要保存到登录身份cookie中的字段
    }
}