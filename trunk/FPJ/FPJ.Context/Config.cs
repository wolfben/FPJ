using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace FPJ.Context
{
    /// <summary>
    /// 配置类
    /// </summary>
    public class Config
    {
        private Config()
        {
        }

        public static readonly Config Instance = new Config();

        /// <summary>
        /// 登录地址
        /// </summary>
        public string LoginUrl
        {
            get
            {
                return "/user/login";
            }
        }

        /// <summary>
        /// Redis配置
        /// </summary>
        public string RedisConnectionStringConfig
        {
            get
            {
                return ConfigurationManager.AppSettings["RedisConfig"];
            }
        }

        /// <summary>
        /// 签名秘钥
        /// </summary>
        public string SignKey
        {
            get
            {
                return "!@#$20161212#@!";
            }
        }
    }
}
