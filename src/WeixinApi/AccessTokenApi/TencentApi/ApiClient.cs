using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 获取微信凭证数据服务接口
    /// </summary>
    /// <remarks>
    /// <see cref="https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Get_access_token.html">获取access_token接口官方说明</see>
    /// </remarks>
    public abstract class ApiClient
    {
        public ApiClient(IOptions<WeixinApiOptions> optionsAccessor)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public WeixinApiOptions _options { get; private set; }
        public HttpClient Http { get { return _options.Backchannel; } }

        // Get
        public virtual async Task<TValue> GetFromJsonAsync<TValue>(string requestUri, CancellationToken cancellationToken = default)
            where TValue : IWeixinError
        {
            //var result = await Http.GetFromJsonAsync<TValue>(requestUri, cancellationToken); // bug: not support inheritance class
            var s = await Http.GetStringAsync(requestUri);
            Debug.WriteLine("Response from remote server:");
            Debug.WriteLine("****************************");
            Debug.WriteLine(s);
            Debug.WriteLine("****************************");
            var result = JsonConvert.DeserializeObject<TValue>(s);
            if (result.Succeeded)
            {
                return result;
            }
            else
            {
                throw new WeixinException(result);
            }
        }

        // Post TValue (or anonymous json object), Get TResult
        public async Task<TResult> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            var responseMessage = await Http.PostAsJsonAsync<TValue>(requestUri, value, options, cancellationToken)
                .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
            var json = responseMessage.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TResult>(json);
        }

        // Post HttpContent, Get TResult
        public async Task<TResult> PostContentAsJsonAsync<TResult>(string requestUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            var responseMessage = await Http.PostAsync(requestUri, content, cancellationToken)
                .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
            var json = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(json);
        }

        // Post Form
        public async Task<TResult> PostFormAsJsonAsync<TResult>(string requestUri, Dictionary<string, string> form, CancellationToken cancellationToken = default)
        {
            Http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            string formString = BuildForm(form);
            var responseMessage = await Http.PostAsync(requestUri, new StringContent(formString), cancellationToken)
                .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
            var json = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(json);
        }

        private string BuildForm(Dictionary<string, string> formData)
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

        // Post Files
        public async Task<TResult> PostMultipleFilesAsJsonAsync<TResult>(string requestUri, Dictionary<string, string> fileKeyAndPaths, CancellationToken cancellationToken = default)
        {
            var boundary = "----" + DateTime.Now.Ticks.ToString("x");
            //byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            string formdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";

            var postStream = new MemoryStream();
            foreach (var fileKeyAndPath in fileKeyAndPaths)
            {
                var fileKey = fileKeyAndPath.Key;
                var filePath = fileKeyAndPath.Value;
                var fileStream = FileHelper.GetFileStream(filePath);
                var formdata = string.Format(formdataTemplate, fileKey, filePath);
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
            //EndOfForm
            var footer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            postStream.Write(footer, 0, footer.Length);

            Http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", string.Format("multipart/form-data; boundary={0}", boundary));
            var responseMessage = await Http.PostAsync(requestUri, new StreamContent(postStream));
            var json = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(json);
        }


        /// <summary>
        /// Post one File
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TResult> PostFileAsJsonAsync<TResult>(string requestUri, Stream file, CancellationToken cancellationToken = default)
        {
            Http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            var responseMessage = await Http.PostAsync(requestUri, new StreamContent(file), cancellationToken)
                .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
            var json = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResult>(json);
        }

        /// <summary>
        /// Download
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> Download(string url, Stream file, int bufferSize = 81920, CancellationToken cancellationToken = default)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            var resp = await Http.GetAsync(url)
                .ContinueWith((getTask) => getTask.Result.EnsureSuccessStatusCode());

            var data = await resp.Content.ReadAsStreamAsync();
            foreach (var header in resp.Headers)
            {
                var key = header.Key;
                ret.Add(key, string.Join(";", header.Value));
            }
            if (data != null)
            {
                await data.CopyToAsync(file, bufferSize, cancellationToken);
            }

            return ret;
        }
    }
}
