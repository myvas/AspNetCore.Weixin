using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace AspNetCore.Weixin
{
    public static class HttpUtility
    {
        /// <summary>
        /// 获取JSON数据。采用Http Get方式。
        /// </summary>
        /// <typeparam name="T">用于封装JSON数据的类</typeparam>
        /// <param name="url">API网址</param>
        /// <param name="encoding">编码。默认为UTF8</param>
        /// <returns></returns>
        public static async Task<T> GetJson<T>(string url, Encoding encoding = null)
        {
            string returnText = await HttpGet(url, encoding);

            Debug.WriteLine("HttpUtility.GetJson<T>");
            Debug.WriteLine("\turl:" + url);
            Debug.WriteLine("\treturn:" + returnText);

            T result = WeixinJsonHelper.Deserialize<T>(returnText);

            return result;
        }
        
        /// <summary>
        /// 发起Post请求
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">请求Url</param>
        /// <param name="cookieContainer">CookieContainer，如果不需要则设为null</param>
        /// <returns></returns>
        public static async Task<T> PostFileGetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, string> fileDictionary = null, Encoding encoding = null)
        {
            string returnText = await HttpPost(url, cookieContainer, null, fileDictionary, null, encoding);
            var result = WeixinJsonHelper.Deserialize<T>(returnText);
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
        public static async Task<T> PostGetJson<T>(string url, CookieContainer cookieContainer = null, Stream fileStream = null, Encoding encoding = null)
        {
            string resultText = await HttpPost(url, cookieContainer, fileStream, null, null, encoding);
            var result = WeixinJsonHelper.Deserialize<T>(resultText);
            return result;
        }

        public static async Task<T> PostGetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, string> formData = null, Encoding encoding = null)
        {
            string resultText = await HttpPost(url, cookieContainer, formData, encoding);
            var result = WeixinJsonHelper.Deserialize<T>(resultText);
            return result;
        }

        public static async Task<T> PostGetJson<T>(string url, string json, CookieContainer cookieContainer = null, Encoding encoding = null)
        {
            string resultText = await HttpPostJson(url, json, encoding);
            var result = WeixinJsonHelper.Deserialize<T>(resultText);
            return result;
        }

        public static async Task<T> PostGetXml<T>(string url, string xml)
        {
            string resultText = await HttpPostXml(url, xml, null);
            var result = XmlConvert.DeserializeObject<T>(resultText);
            return result;
        }


        /// <summary>
        /// 使用Http Get方法获取字符串结果（没有加入Cookie）
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding">返回值编码。默认为UTF8</param>
        /// <returns></returns>
        public static async Task<string> HttpGet(string url, Encoding encoding = null)
        {
            var client = new HttpClient();
            var resp = await client.GetAsync(url);

            return await resp.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="stream">输出流</param>
        /// <returns></returns>
        public static async Task<Dictionary<string, string>> Download(string url, Stream stream)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            var client = new HttpClient();
            var resp = await client.GetAsync(url);
            var data = await resp.Content.ReadAsByteArrayAsync();

            foreach (var header in resp.Headers)
            {
                var key = header.Key;
                ret.Add(key, string.Join(";", header.Value));
            }

            return ret;
        }

        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <returns></returns>
        public static async Task<string> HttpPost(string url, CookieContainer cookieContainer = null, Dictionary<string, string> formData = null, Encoding encoding = null, int timeOut = Config.TIME_OUT)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(formDataBytes, 0, formDataBytes.Length);
                ms.Seek(0, SeekOrigin.Begin);//设置指针读取位置

                return await HttpPost(url, cookieContainer, ms, null, null, encoding);
            }
        }

        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <returns></returns>
        public static async Task<string> HttpPostXml(string url, string xml, Encoding encoding = null, int timeout = Config.TIME_OUT)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
            //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text/json");//application/json

            var httpContent = new StringContent(xml, encoding, "text/xml");
            var resp = await httpClient.PostAsync(url, httpContent);

            return await resp.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <returns></returns>
        public static async Task<string> HttpPostJson(string url, string json, Encoding encoding = null, int timeout = Config.TIME_OUT)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text/json");//application/json

            var httpContent = new StringContent(json, encoding, "application/json");
            var resp = await httpClient.PostAsync(url, httpContent);

            return await resp.Content.ReadAsStringAsync();
        }

        public static async Task<HttpResponseMessage> UploadFile(string url, string filePath)
        {
            var client = new HttpClient();

            //string boundary = "----" + DateTime.Now.Ticks.ToString("x");
            var requestContent = new MultipartFormDataContent();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                byte[] buffer = new byte[stream.Length + 1];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    byte[] fileData = ms.ToArray();

                    var fileContent = new ByteArrayContent(fileData);
                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

                    var filename = Path.GetFileName(filePath);
                    requestContent.Add(fileContent, filename);
                }
            }

            return await client.PostAsync(url, requestContent);
        }

        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieContainer"></param>
        /// <param name="postStream"></param>
        /// <param name="fileDictionary">需要上传的文件，Key：对应要上传的Name，Value：本地文件名</param>
        /// <param name="encoding"></param>
        /// <param name="refererUrl"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static async Task<string> HttpPost(string url, CookieContainer cookieContainer = null,
            Stream postStream = null, Dictionary<string, string> fileDictionary = null,
            string refererUrl = null, Encoding encoding = null, int timeOut = Config.TIME_OUT)
        {
            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeOut);

            #region 处理Form表单文件上传
            var formUploadFile = fileDictionary != null && fileDictionary.Count > 0;//是否用Form上传文件
            if (formUploadFile)
            {
                //通过表单上传文件
                postStream = new MemoryStream();

                string boundary = "----" + DateTime.Now.Ticks.ToString("x");
                //byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";

                foreach (var file in fileDictionary)
                {
                    var fileName = file.Value;
                    //准备文件流
                    using (var fileStream = FileHelper.GetFileStream(fileName))
                    {
                        var formdata = string.Format(formdataTemplate, file.Key, fileName /*Path.GetFileName(fileName)*/);
                        var formdataBytes = Encoding.ASCII.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                        postStream.Write(formdataBytes, 0, formdataBytes.Length);

                        //写入文件
                        byte[] buffer = new byte[1024];
                        int bytesRead = 0;
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            postStream.Write(buffer, 0, bytesRead);
                        }
                    }
                }
                //结尾
                var footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", string.Format("multipart/form-data; boundary={0}", boundary));
            }
            else
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            }
            #endregion

            //client.ContentLength = postStream != null ? postStream.Length : 0;
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8"));
            //client..KeepAlive = true;

            if (!string.IsNullOrEmpty(refererUrl))
            {
                //client.Referer = refererUrl;
            }
            //client.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";

            if (cookieContainer != null)
            {
                //client.CookieContainer = cookieContainer;
            }

            var response = await httpClient.PostAsync(url, new StreamContent(postStream));
            if (cookieContainer != null)
            {
                //response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }

            return await response.Content.ReadAsStringAsync();
        }


        /// <summary>
        /// 组装QueryString的方法
        /// 参数之间用&amp;连接，首位没有符号，如：a=1&b=2&c=3
        /// </summary>
        /// <param name="formData"></param>
        /// <returns></returns>
        public static string GetQueryString(this Dictionary<string, string> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }
    }
}
