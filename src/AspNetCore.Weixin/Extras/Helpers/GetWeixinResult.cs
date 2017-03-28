using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpUtilityPost
    {
        /// <summary>
        /// 获取Post结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnText"></param>
        /// <returns></returns>
        public static T GetResult<T>(string returnText)
        {
            if (returnText.Contains("errcode"))
            {
                //可能发生错误
                WeixinErrorJson errorResult = JsonConvert.DeserializeObject<WeixinErrorJson>(returnText);
                if (errorResult.errcode != WeixinResponseStatus.OK)
                {
                    //发生错误
                    throw new WeixinErrorResultException(
                        string.Format("微信Post请求发生错误！错误代码：{0}，说明：{1}",
                                      (int)errorResult.errcode,
                                      errorResult.errmsg),
                        null, errorResult);
                }
            }

            T result = JsonConvert.DeserializeObject<T>(returnText);
            return result;
        }

        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">请求Url</param>
        /// <param name="cookieContainer">CookieContainer，如果不需要则设为null</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="fileDictionary"></param>
        /// <returns></returns>
        public static async Task<T> PostFileGetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, string> fileDictionary = null, Encoding encoding = null)
        {
            string returnText = await HttpUtility.HttpPost(url, cookieContainer, null, fileDictionary, null, encoding);
            var result = GetResult<T>(returnText);
            return result;
        }

        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">请求Url</param>
        /// <param name="cookieContainer">CookieContainer，如果不需要则设为null</param>
        /// <param name="fileStream">文件流</param>
        /// <returns></returns>
        public static async Task<T> PostGetJson<T>(string url, CookieContainer cookieContainer = null,
            Stream fileStream = null, Encoding encoding = null)
        {
            string returnText = await HttpUtility.HttpPost(url, cookieContainer, fileStream, null, null, encoding);
            var result = GetResult<T>(returnText);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="formData"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<T> PostGetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, string> formData = null, Encoding encoding = null)
        {
            string returnText = await HttpUtility.HttpPost(url, cookieContainer, formData, encoding);
            var result = GetResult<T>(returnText);
            return result;
        }
    }

}
