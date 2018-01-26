using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace Utility.Helper
{
    /// <summary>
    /// 签名帮助类
    /// </summary>
    public class SignHelper
    {
        //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        private SortedDictionary<string, object> m_values;

        /// <summary>
        /// 签名秘钥
        /// </summary>
        private string m_signkey;

        public SignHelper(string signKey)
        {
            m_signkey = signKey;
        }

        public SignHelper(string signKey, Dictionary<string, object> values)
        {
            m_signkey = signKey;
            m_values = new SortedDictionary<string, object>(values);
        }

        /// <summary>
        /// 添加值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <param name="value">字段值</param>
        public void SetValue(string key, object value)
        {
            m_values[key] = value;
        }

        /// <summary>
        /// 根据字段名获取某个字段的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }

        /// <summary>
        /// 获取签名参数
        /// </summary>
        /// <returns></returns>
        public SortedDictionary<string, object> GetValues()
        {
            return m_values;
        }

        /// <summary>
        /// 判断某个字段是否已设置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsSet(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 将字典值转换为url字符串格式
        /// </summary>
        /// <returns></returns>
        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    throw new Exception("签名参数内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }

        /// <summary>
        /// 签名
        /// </summary>
        /// <returns>返回签名结果</returns>
        public string MakeSign()
        {
            //转url格式
            string str = ToUrl();
            //在string后加入API KEY
            str += "&key=" + m_signkey;
            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 签名验证
        /// </summary>
        /// <returns>是否签名认证成功</returns>
        public bool CheckSign()
        {
            //如果没有设置签名，则跳过检测
            if (!IsSet("sign"))
            {
                throw new Exception("参数中不存在签名参数sign!");
            }
            //如果设置了签名但是签名为空，则抛异常
            else if (GetValue("sign") == null || GetValue("sign").ToString() == "")
            {
                throw new Exception("参数中签名参数sign不合法!");
            }

            //获取接收到的签名
            string return_sign = GetValue("sign").ToString();

            //在本地计算新的签名
            string cal_sign = MakeSign();

            return cal_sign == return_sign;
        }
    }
}
