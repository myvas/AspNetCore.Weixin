using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCore.Weixin
{
    public static class JsSignatureApi
    {
        /// <summary>
        /// 计算签名
        /// <para><pre>
        /// 1.去掉值为空的参数
        /// 2.键名转成小写
        /// 2.按键名进行排序
        /// 3.用&amp;拼接各键值对
        /// 4.SHA1，并将结果转换成Hex
        /// 6.转换成小写
        /// </pre></para>
        /// </summary>
        public static string CalculateJsSignature(Dictionary<string, object> inElements)
        {
            var elements = new Dictionary<string, object>();
            foreach (var element in inElements)
            {
                elements.Add(element.Key.ToLower(), element.Value);
            }

            var sorted = elements.Where(x => x.Value != null && !string.IsNullOrEmpty(x.Value.ToString()))
                .OrderBy(x => x.Key);
            var sb = new StringBuilder();
            int i = 0;
            foreach (var kv in sorted)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < sorted.Count())
                {
                    sb.Append("&");
                }
            }
            var hasher = SHA1.Create();
            byte[] hashData = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            string hashHex = BitConverter.ToString(hashData);
            hashHex = hashHex.Replace("-", "");
            hashHex = hashHex.ToLower();
            return hashHex;
        }
    }
}
