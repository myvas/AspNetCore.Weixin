using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ApiClient
{
    public HttpClient Http { get; }

    public ApiClient(HttpClient client)
    {
        Http = client;
    }

    /// <summary>
    /// Sends an HTTP GET request to the specified URL, retrieves the response as JSON, deserializes it into a TValue object, and returns the object.
    /// </summary>
    /// <typeparam name="TValue">The type of object to which the JSON response is deserializd.</typeparam>
    /// <param name="requestUri">The URL to which the HTTP GET request is sent.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The deserialized object of type TValue.</returns>
    public async Task<TValue> GetFromJsonAsync<TValue>(string requestUri, CancellationToken cancellationToken = default)
    {
        return await Http.GetFromJsonAsync<TValue>(requestUri, cancellationToken);
    }

    /// <summary>
    /// Sends an HTTP POST request to the specified URL, which include a request body in JSON that serialized from TValue object, retrieves the response as JSON, deserializes it into a TResult object, and returns the object.
    /// </summary>
    /// <typeparam name="TValue">The type of object to which the JSON request is deserialed. Anonymous JSON object is also available.</typeparam>
    /// <typeparam name="TResult">The type of object to which the JSON response is deserializd.</typeparam>
    /// <param name="requestUri">The URL to which the HTTP GET request is sent.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The deserialized object of type TResult.</returns>
    public async Task<TResult> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
    {
        var responseMessage = await Http.PostAsJsonAsync<TValue>(requestUri, value, options, cancellationToken)
            .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
        var json = responseMessage.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<TResult>(json);
    }

    /// <summary>
    /// Sends an HTTP POST request to the specified URL, which include a request body in form-data with one or more file(s) that build from a Dictionary object, retrieves the response as JSON, deserializes it into a TResult object, and returns the object.
    /// </summary>
    /// <typeparam name="TResult">The type of object to which the JSON response is deserializd.</typeparam>
    /// <param name="requestUri">The URL to which the HTTP GET request is sent.</param>
    /// <param name="fileKeyAndPaths">The dictionary object which indicates the name and path of files which will be uploaded.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The deserialized object of type TResult.</returns>
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

    /// <summary>
    /// Post HttpContent, Get TResult
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
    /// <param name="requestUri"></param>
    /// <returns></returns>
    public async Task<Dictionary<string, string>> Download(string requestUri, Stream file, int bufferSize = 81920, CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> ret = new Dictionary<string, string>();

        var resp = await Http.GetAsync(requestUri)
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

    /// <summary>
    /// HTTP POST as x-www-form-urlencoded
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="form"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult> PostFormAsJsonAsync<TResult>(string requestUri, Dictionary<string, string> form, CancellationToken cancellationToken = default)
    {
        Http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
        string formString = BuildForm(form);
        var responseMessage = await Http.PostAsync(requestUri, new StringContent(formString), cancellationToken)
            .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());
        var json = await responseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResult>(json);
    }

    #region private utilities
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
    #endregion
}
