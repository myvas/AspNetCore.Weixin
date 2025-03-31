using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ApiClient
{
    public HttpClient Http { get; }

    public ApiClient(HttpClient httpClient)
    {
        Http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <summary>
    /// Sends an HTTP GET request to the specified URL, retrieves the response as JSON, deserializes it into a TValue object, and returns the object.
    /// </summary>
    /// <typeparam name="TResult">The type of object to which the JSON response is deserializd.</typeparam>
    /// <param name="requestUri">The URL to which the HTTP GET request is sent.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The deserialized object of type TValue.</returns>
    public async Task<TResult> GetFromJsonAsync<TResult>(string requestUri, CancellationToken cancellationToken = default)
        where TResult : IWeixinErrorJson, new()
    {
        Debug.WriteLine(requestUri);
        return await WeixinErrorJson.FromResponseAsync<TResult>(async cancellationToken =>
        {
            return await Http.GetAsync(requestUri, cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Sends an HTTP POST request to the specified URL, which include a request body in JSON that serialized from TValue object, retrieves the response as JSON, deserializes it into a TResult object, and returns the object.
    /// </summary>
    /// <typeparam name="TValue">The type of object to which the JSON request is deserialed. Anonymous JSON object is also available.</typeparam>
    /// <typeparam name="TResult">The type of object to which the JSON response is deserializd.</typeparam>
    /// <param name="requestUri">The URL to which the HTTP GET request is sent.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The deserialized object of type TResult.</returns>
    /// <remarks>
    /// </remarks>
    public async Task<TResult> PostAsJsonAsync<TValue, TResult>(string requestUri, TValue value, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
           where TResult : IWeixinErrorJson, new()
    {
        Debug.WriteLine(requestUri);

        // NOTE: I am not so sure whether the HttpClient.PostAsJsonAsync() supports anonymous object very well.
        // [412] HTTP request failed with status: PreconditionFailed
        // But sometimes it does not work as expected, so I make a redirect to another method.
        //if (typeof(TValue) == typeof(object))
        {
            return await PostAnonymousObjectAsJsonAsync<TResult>(requestUri, value, options, cancellationToken);
        }
        
        // return await WeixinErrorJson.FromResponseAsync<TResult>(async cancellationToken =>
        // {
        //     Debug.WriteLine(JsonSerializer.Serialize(value));
        //     return await Http.PostAsJsonAsync(requestUri, value, options, cancellationToken);
        // }, cancellationToken);
    }
    
    /// <summary>
    /// Post an anonymous object, and get the response as TResult json.
    /// </summary>
    /// <typeparam name="TResult">A class suitable for Json serializer.</typeparam>
    /// <param name="requestUri"></param>
    /// <param name="anonymousObject"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// A sample for anonymous object (or anonymous type):
    /// <code>
    /// var body = new
    /// {
    ///     grant_type = "client_credential",
    ///     appid = Options.AppId,
    ///     secret = Options.AppSecret,
    ///     force_refresh = forceRefresh
    /// };
    /// </code>
    /// </remarks>
    public async Task<TResult> PostAnonymousObjectAsJsonAsync<TResult>(string requestUri, object anonymousObject, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        where TResult : IWeixinErrorJson, new()
    {
        Debug.WriteLine(requestUri);
        return await WeixinErrorJson.FromResponseAsync<TResult>(async ct =>
        {
            var jsonBody = JsonSerializer.Serialize(anonymousObject, options);
            Debug.WriteLine(jsonBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            return await Http.PostAsync(requestUri, content, ct).ConfigureAwait(false);
        }, cancellationToken);
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
        where TResult : IWeixinErrorJson, new()
    {
        Debug.WriteLine(requestUri);
        return await WeixinErrorJson.FromResponseAsync<TResult>(async cancellationToken =>
        {
            Debug.WriteLine(JsonSerializer.Serialize(fileKeyAndPaths));
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
            return await Http.PostAsync(requestUri, new StreamContent(postStream));
        }, cancellationToken);
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
        where TResult : IWeixinErrorJson, new()
    {
        Debug.WriteLine(requestUri);
        return await WeixinErrorJson.FromResponseAsync<TResult>(async cancellationToken =>
        {
            Http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            return await Http.PostAsync(requestUri, new StreamContent(file), cancellationToken);
        }, cancellationToken);
    }

    /// <summary>
    /// Post HttpContent, Get TResult
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="requestUri"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <noexception/>
    public async Task<TResult> PostContentAsJsonAsync<TResult>(string requestUri, HttpContent content, CancellationToken cancellationToken = default)
        where TResult : IWeixinErrorJson, new()
    {
        Debug.WriteLine(requestUri);
        return await WeixinErrorJson.FromResponseAsync<TResult>(async ct =>
        {
            return await Http.PostAsync(requestUri, content, ct).ConfigureAwait(false);
        }, cancellationToken);
    }

    public class WeixinDownloadResultJson : WeixinErrorJson
    {
        [JsonPropertyName("headers")]
        public Dictionary<string, string> Headers { get; set; } = [];
    }

    /// <summary>
    /// Download file and return the headers in response.
    /// </summary>
    /// <param name="requestUri"></param>
    /// <returns>The headers in response.</returns>
    public async Task<WeixinDownloadResultJson> Download(string requestUri, Stream file, int bufferSize = 81920, CancellationToken cancellationToken = default)
    {
        Debug.WriteLine(requestUri);
        var result = new WeixinDownloadResultJson();

        try
        {
            using var response = await Http.GetAsync(requestUri);

            if (!response.IsSuccessStatusCode)
            {
                result.ErrorCode = (int)response.StatusCode;
                result.ErrorMessage = $"HTTP request failed with status: {response.StatusCode}";
                return result;
            }

            var data = await response.Content.ReadAsStreamAsync();
            foreach (var header in response.Headers)
            {
                var key = header.Key;
                result.Headers.Add(key, string.Join(";", header.Value));
            }
            if (data != null)
            {
                await data.CopyToAsync(file, bufferSize, cancellationToken);
            }
            return result;
        }
        catch (OperationCanceledException)
        {
            result.ErrorCode = (int)HttpStatusCode.RequestTimeout;
            result.ErrorMessage = "Request was canceled";
            return result;
        }
        catch (HttpRequestException httpEx)
        {
            result.ErrorCode = httpEx.GetStatusCode() ?? (int)HttpStatusCode.BadRequest;
            result.ErrorMessage = httpEx.Message;
            result.Exception = httpEx;
            return result;
        }
        catch (JsonException jsonEx)
        {
            result.ErrorCode = (int)HttpStatusCode.BadRequest;
            result.ErrorMessage = "Invalid JSON response";
            result.Exception = jsonEx;
            return result;
        }
        catch (NotSupportedException notSuppEx)
        {
            result.ErrorCode = (int)HttpStatusCode.UnsupportedMediaType;
            result.ErrorMessage = "Unsupported response format";
            result.Exception = notSuppEx;
            return result;
        }
        catch (Exception ex)
        {
            result.ErrorCode = (int)HttpStatusCode.InternalServerError;
            result.ErrorMessage = "An unexpected error occurred";
            result.Exception = ex;
            return result;
        }
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
        where TResult : IWeixinErrorJson, new()
    {
        Debug.WriteLine(requestUri);
        return await WeixinErrorJson.FromResponseAsync<TResult>(async cancellationToken =>
        {
            Http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            string formString = BuildForm(form);
            return await Http.PostAsync(requestUri, new StringContent(formString), cancellationToken);
        }, cancellationToken);
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
