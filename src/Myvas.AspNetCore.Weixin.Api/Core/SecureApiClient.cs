using Myvas.AspNetCore.Weixin.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class SecureApiClient
    {
        private readonly IWeixinAccessToken _accessToken;
        public HttpClient Http { get; }

        public SecureApiClient(HttpClient client, IWeixinAccessToken accessToken)
        {
            _accessToken = accessToken;
            Http = client;
        }

        // Get
        public async Task<TValue> GetFromJsonAsync<TValue>(string requestUri, CancellationToken cancellationToken = default)
        {
            return await Http.GetFromJsonAsync<TValue>(requestUri, cancellationToken);
        }

        public Task<TValue> GetFromJsonAsync<TValue>(string accessToken, string requestUriFormat, CancellationToken cancellationToken = default)
            => GetFromJsonAsync<TValue>(string.Format(requestUriFormat, accessToken), cancellationToken);

        public async Task<TValue> SecureGetFromJsonAsync<TValue>(string requestUriFormat, CancellationToken cancellationToken = default)
            => await GetFromJsonAsync<TValue>(await _accessToken.GetTokenAsync(), requestUriFormat, cancellationToken);

        // Post TValue (or anonymous json object), Get TResult
        public async Task<TResult> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            var responseMessage = await Http.PostAsJsonAsync<TValue>(requestUri, value, options, cancellationToken)
                .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
            var json = responseMessage.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<TResult>(json);
        }
        public Task<TResult> PostAsJsonAsync<TValue, TResult>(string accessToken, string requestUriFormat, TValue value, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
            => PostAsJsonAsync<TValue, TResult>(string.Format(requestUriFormat, accessToken), value, options, cancellationToken);

        public async Task<TResult> SecurePostAsJsonAsync<TValue, TResult>(string requestUriFormat, TValue value, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
            => await PostAsJsonAsync<TValue, TResult>(await _accessToken.GetTokenAsync(), requestUriFormat, value, options, cancellationToken);

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
            return JsonSerializer.Deserialize<TResult>(json);
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
            return JsonSerializer.Deserialize<TResult>(json);
        }

        // Post HttpContent, Get TResult
        public async Task<TResult> PostContentAsJsonAsync<TResult>(string requestUri, HttpContent content, CancellationToken cancellationToken = default)
        {
            var responseMessage = await Http.PostAsync(requestUri, content, cancellationToken)
                .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
            var json = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResult>(json);
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

        public async Task<TResult> PostFormAsJsonAsync<TResult>(string requestUri, Dictionary<string, string> form, CancellationToken cancellationToken = default)
        {
            Http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            string formString = BuildForm(form);
            var responseMessage = await Http.PostAsync(requestUri, new StringContent(formString), cancellationToken)
                .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
            var json = await responseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResult>(json);
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
    }
}
