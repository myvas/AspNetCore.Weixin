using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 获取微信凭证数据服务接口
/// </summary>
/// <remarks>
/// <see cref="https://developers.weixin.qq.com/doc/offiaccount/Basic_Information/Get_access_token.html">获取access_token接口官方说明</see>
/// </remarks>
public abstract class SecureWeixinApiClient : WeixinApiClient
{
    protected IWeixinAccessTokenApi Token { get; private set; }

    protected string AppId { get { return Options.AppId; } }

    public SecureWeixinApiClient(IOptions<WeixinOptions> optionsAccessor, IWeixinAccessTokenApi tokenProvider)
        : base(optionsAccessor)
    {
        Token = tokenProvider ?? throw new ArgumentException(nameof(tokenProvider));
    }

    protected async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
        => (await Token.GetTokenAsync(cancellationToken))?.AccessToken;

    protected async Task<string> FormatUrlWithTokenAsync(string requestUriFormat, CancellationToken cancellationToken = default)
        => requestUriFormat.Contains("access_token=ACCESS_TOKEN")
            ? requestUriFormat.Replace("access_token=ACCESS_TOKEN", "access_token=" + await GetTokenAsync(cancellationToken))
            : string.Format(requestUriFormat, await GetTokenAsync(cancellationToken));

    /// <summary>
    /// Sends an HTTP GET request to the specified URL, retrieves the response as JSON, deserializes it into a TValue object, and returns the object.
    /// </summary>
    /// <typeparam name="TValue">The type of object to which the JSON response is deserializd.</typeparam>
    /// <param name="requestUriFormat">The URL to which the HTTP GET request is sent, included a placeholder of 'access_token=ACCESS_TOKEN' or '{0}'</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The deserialized object of type TValue.</returns>
    public async Task<TValue> SecureGetFromJsonAsync<TValue>(string requestUriFormat, CancellationToken cancellationToken = default)
        where TValue : IWeixinError
        => await GetFromJsonAsync<TValue>(await FormatUrlWithTokenAsync(requestUriFormat, cancellationToken), cancellationToken);

    /// <summary>
    /// Sends an HTTP POST request to the specified URL, which include a request body in JSON that serialized from TValue object, retrieves the response as JSON, deserializes it into a TResult object, and returns the object.
    /// </summary>
    /// <typeparam name="TValue">The type of object to which the JSON request is deserialed.</typeparam>
    /// <typeparam name="TResult">The type of object to which the JSON response is deserializd.</typeparam>
    /// <param name="requestUriFormat">The URL to which the HTTP GET request is sent, included a placeholder of 'access_token=ACCESS_TOKEN' or '{0}'</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The deserialized object of type TResult.</returns>
    public async Task<TResult> SecurePostAsJsonAsync<TValue, TResult>(string requestUriFormat, TValue value,
        JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        where TResult : WeixinErrorJson
        => await PostAsJsonAsync<TValue, TResult>(await FormatUrlWithTokenAsync(requestUriFormat, cancellationToken), value, options, cancellationToken);

    /// <summary>
    /// Sends an HTTP POST request to the specified URL, which include a request body in form-data with one or more file(s) that build from a Dictionary object, retrieves the response as JSON, deserializes it into a TResult object, and returns the object.
    /// </summary>
    /// <typeparam name="TResult">The type of object to which the JSON response is deserializd.</typeparam>
    /// <param name="requestUriFormat">The URL to which the HTTP GET request is sent, included a placeholder of 'access_token=ACCESS_TOKEN' or '{0}'</param>
    /// <param name="fileKeyAndPaths">The dictionary object which indicates the name and path of files which will be uploaded.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>The deserialized object of type TResult.</returns>
    // Post Files
    public async Task<TResult> SecurePostMultipleFilesAsJsonAsync<TResult>(string requestUriFormat, Dictionary<string, string> fileKeyAndPaths, CancellationToken cancellationToken = default)
        => await PostMultipleFilesAsJsonAsync<TResult>(await FormatUrlWithTokenAsync(requestUriFormat, cancellationToken), fileKeyAndPaths, cancellationToken);

    /// <summary>
    /// Post one File
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="requestUriFormat">The URL to which the HTTP GET request is sent, included a placeholder of 'access_token=ACCESS_TOKEN' or '{0}'</param>
    /// <param name="file"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult> SecurePostFileAsJsonAsync<TResult>(string requestUriFormat, Stream file, CancellationToken cancellationToken = default)
        => await PostFileAsJsonAsync<TResult>(await FormatUrlWithTokenAsync(requestUriFormat, cancellationToken), file, cancellationToken);

    /// <summary>
    /// Post HttpContent, Get TResult
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="requestUriFormat">The URL to which the HTTP GET request is sent, included a placeholder of 'access_token=ACCESS_TOKEN' or '{0}'</param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult> SecurePostContentAsJsonAsync<TResult>(string requestUriFormat, HttpContent content, CancellationToken cancellationToken = default)
    => await PostContentAsJsonAsync<TResult>(await FormatUrlWithTokenAsync(requestUriFormat, cancellationToken), content, cancellationToken);

    /// <summary>
    /// Download
    /// </summary>
    /// <param name="requestUriFormat">The URL to which the HTTP GET request is sent, included a placeholder of 'access_token=ACCESS_TOKEN' or '{0}'</param>
    /// <returns></returns>
    public async Task<Dictionary<string, string>> SecureDownload(string requestUriFormat, Stream file, int bufferSize = 81920, CancellationToken cancellationToken = default)
    => await Download(await FormatUrlWithTokenAsync(requestUriFormat, cancellationToken), file, bufferSize, cancellationToken);

    /// <summary>
    /// HTTP POST as x-www-form-urlencoded
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="requestUriFormat">The URL to which the HTTP GET request is sent, included a placeholder of 'access_token=ACCESS_TOKEN' or '{0}'</param>
    /// <param name="form"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<TResult> SecurePostFormAsJsonAsync<TResult>(string requestUriFormat, Dictionary<string, string> form, CancellationToken cancellationToken = default)
        => await PostFormAsJsonAsync<TResult>(await FormatUrlWithTokenAsync(requestUriFormat, cancellationToken), form, cancellationToken);
}
