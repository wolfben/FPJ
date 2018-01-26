using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Utility.Helper
{
    /// <summary>
    /// 安全帮助类
    /// </summary>
    public class SecurityHelper
    {
        /// <summary>
        /// SHA1签名
        /// </summary>
        /// <param name="content">输入内容</param>
        /// <param name="secret">签名秘钥</param>
        /// <param name="encoding">字符编码格式</param>
        /// <returns></returns>
        public static string SHA1Sign(string content, string secret, Encoding encoding)
        {
            using (HMACSHA1 sha1 = new HMACSHA1(encoding.GetBytes(secret)))
            {
                var contentBytes = encoding.GetBytes(content);
                var resultBytes = sha1.ComputeHash(contentBytes, 0, contentBytes.Length);
                return Convert.ToBase64String(resultBytes);
            }
        }

        /// <summary>
        /// MD5签名
        /// </summary>
        /// <param name="content">输入内容</param>
        /// <param name="secret">签名秘钥</param>
        /// <param name="encoding">字符编码格式</param>
        /// <returns></returns>
        public static string MD5Sign(string content, string secret, Encoding encoding)
        {
            using (HMACMD5 md5 = new HMACMD5(encoding.GetBytes(secret)))
            {
                var contentBytes = encoding.GetBytes(content);
                var resultBytes = md5.ComputeHash(contentBytes, 0, contentBytes.Length);
                return Convert.ToBase64String(resultBytes);
            }
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="content">输入内容</param>
        /// <param name="encoding">字符编码格式</param>
        /// <returns></returns>
        public static string SHA1(string content, Encoding encoding)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                var contentBytes = encoding.GetBytes(content);
                var resultBytes = sha1.ComputeHash(contentBytes, 0, contentBytes.Length);
                return Convert.ToBase64String(resultBytes);
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="content">输入内容</param>
        /// <param name="encoding">字符编码格式</param>
        /// <returns></returns>
        public static string MD5(string content, Encoding encoding)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                var contentBytes = encoding.GetBytes(content);
                var resultBytes = md5.ComputeHash(contentBytes, 0, contentBytes.Length);
                return Convert.ToBase64String(resultBytes);
            }
        }

        /// <summary>
        /// RSA非对称加密
        /// </summary>
        /// <param name="publicKey">加密秘钥</param>
        /// <param name="content">加密内容</param>
        /// <returns></returns>
        public static string RSAEncrypt(string publicKey, string content)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(publicKey);
            byte[] bytes = new UnicodeEncoding().GetBytes(content);
            return Convert.ToBase64String(rsaProvider.Encrypt(bytes, true));
        }

        /// <summary>
        /// RSA非对称解密
        /// </summary>
        /// <param name="privateKey">解密秘钥</param>
        /// <param name="content">解密内容</param>
        /// <returns></returns>
        public static string RSADecrypt(string privateKey,string content)
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(privateKey);
            byte[] bytes = Convert.FromBase64String(content);
            byte[] buffers = rsaProvider.Decrypt(bytes, true);
            return new UnicodeEncoding().GetString(buffers);
        }

        /// <summary>
        /// 获取强伪随机数
        /// </summary>
        /// <returns></returns>
        public static int RNGNum()
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[4];
                rng.GetBytes(randomBytes);
                return BitConverter.ToInt32(randomBytes, 0);
            }
        }
    }
}
