using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Site.Properties;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// This middleware provides a default welcome/validation page for new Weixin App.
/// </summary>
public class WeixinSiteMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly WeixinSiteOptions _options;
    private readonly IWeixinSite _site;

    public WeixinSiteMiddleware(
        RequestDelegate next,
        IWeixinSite site,
        IOptions<WeixinSiteOptions> optionsAccessor,
        ILoggerFactory loggerFactory)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = loggerFactory?.CreateLogger<WeixinSiteMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));

        if (string.IsNullOrEmpty(_options.WebsiteToken))
        {
            throw new ArgumentException($"Options '{nameof(_options.WebsiteToken)}' cannot be null or empty");
        }

        if (string.IsNullOrEmpty(_options.Path))
        {
            throw new ArgumentException($"Options '{nameof(_options.Path)}' cannot be null or empty");
        }

        // _options.Debug

        _site = site ?? throw new ArgumentNullException(nameof(site));
    }

    /// <summary>
    /// Process an individual request on specific <see cref="WeixinSiteOptions.Path"/> .
    /// </summary>
    public Task Invoke(HttpContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // Early exit if path doesn't match
        if (!IsRequestPathMatch(context.Request, _options.Path))
        {
            return _next(context);
        }

        LogRequestPath(context.Request); // Debug logging

        // Handle POST/GET requests
        return HttpMethods.IsPost(context.Request.Method)
            ? InvokePostAsync(context)
            : InvokeGetAsync(context);
    }

    /// <summary>
    /// Checks if the request path matches the configured path (case-insensitive).
    /// </summary>
    private bool IsRequestPathMatch(HttpRequest request, string expectedPath)
    {
        return expectedPath.Equals(request.Path, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Logs the request path in DEBUG mode (sanitized for safety).
    /// </summary>
    [Conditional("DEBUG")]
    private void LogRequestPath(HttpRequest request)
    {
        var sanitizedPath = request.Path.Value?
            .Replace(Environment.NewLine, string.Empty)
            .Replace("\n", string.Empty)
            .Replace("\r", string.Empty);

        var msg = $"Request matched the WeixinSite path '{sanitizedPath}'";
        Debug.WriteLine(msg);
        _logger.LogDebug(msg);
    }

    /// <summary>
    /// 指示微信公众号消息接收地址是否可用
    /// </summary>
    /// <remarks>
    /// 明文模式：GET http://wx.steamlet.com/wx?signature=c48abd8c533cba0ff7230e9b5b78d8970bef70e8&echostr=3851421201668733949&timestamp=1553423097&nonce=1968330093
    /// 兼容模式：GET http://wx.steamlet.com/wx?signature=2b3a50d442885f8eef72f4d10bef472df12d1675&echostr=4780759847666154895&timestamp=1553423758&nonce=2002608571
    /// 安全模式：GET http://wx.steamlet.com/wx?signature=153cf3051ee1de57b31cab8c16a1c08e7ebc7a18&echostr=4882545160925183999&timestamp=1553423855&nonce=659627225
    /// </remarks>
    public async Task InvokeGetAsync(HttpContext context)
    {
        _logger.LogDebug("[WeixinSiteMiddleware] Server URL verification requested.");

        HttpRequest request = context.Request;
        var websiteToken = _options.WebsiteToken;
        var signature = request.Query["signature"];
        var echostr = request.Query["echostr"];
        var timestamp = request.Query["timestamp"];
        var nonce = request.Query["nonce"];

        if (SignatureHelper.ValidateSignature(signature, timestamp, nonce, websiteToken))
        {
            await ResponsePlainTextAsync(context, echostr);
        }
        else
        {
            await ResponsePlainTextAsync(context, Resources.WeixinSiteVerificationPathNotice);
        }
    }

    /// <summary>
    /// 微信公众号消息接收地址
    /// </summary>
    public async Task InvokePostAsync(HttpContext context)
    {
        if (!_options.Debug)
        {
            var websiteToken = _options.WebsiteToken;
            HttpRequest request = context.Request;
            var signature = request.Query["signature"];
            var timestamp = request.Query["timestamp"];
            var nonce = request.Query["nonce"];

            // Validate the signature in query string
            if (!SignatureHelper.ValidateSignature(signature, timestamp, nonce, websiteToken))
            {
                await ResponsePlainTextAsync(context, Resources.InvalidSignatureDenied, StatusCodes.Status400BadRequest);
                return;
            }

            // Validate the User-Agent in request headers
            (bool isMicroMessenger, string version) = BrowserDetector.DetectMicroMessenger(context);
            if (!isMicroMessenger)
            {
                await ResponsePlainTextAsync(context, Resources.NonMicroMessengerDenied, StatusCodes.Status400BadRequest);
                return;
            }
        }

        await HandleAsync(context);
        return;
    }

    /// <summary>
    /// Handle the request body.
    /// </summary>
    private async Task HandleAsync(HttpContext context)
    {
        // To limit the content length to avoid abnormal requests.
        var maxRequestContentLength = _options.MaxRequestContentLength;
        var contentLength = context.Request.ContentLength ?? 0;
        if (contentLength > maxRequestContentLength)
        {
            await ResponsePlainTextAsync(context, Resources.TooLargeWeixinRequestContent, StatusCodes.Status400BadRequest);
            return;
        }

        // Read and process request body
        string text;
        try
        {
            text = await ReadRequestBodyAsync(context.Request);//.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to read request body.");
            await ResponsePlainTextAsync(context, Resources.FailedToReadRequestBody, StatusCodes.Status400BadRequest);
            return;
        }

        LogRequestBody(text); // Debug logging

        _site.Context = new WeixinContext(context, text);
        var handled = await _site.HandleAsync();
        if (!handled)
        {
            await ResponsePlainTextAsync(context, Resources.Response501NotImplemented, StatusCodes.Status501NotImplemented);
            return;
        }
    }

    /// <summary>
    /// Reads the request body as a string.
    /// </summary>
    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        // Ensure the body stream is read from the beginning
        request.EnableBuffering();

        try
        {
            using var streamReader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 1024,
                leaveOpen: true); // Leave the stream open for potential re-reading

            string body = await streamReader.ReadToEndAsync();
            request.Body.Position = 0; // Reset the stream position for future reads
            return body;
        }
        catch
        {
            request.Body.Position = 0; // Ensure stream is reset even on failure
            throw;
        }
    }

    /// <summary>
    /// Logs the request body in DEBUG mode (sanitized for safety).
    /// </summary>
    [Conditional("DEBUG")]
    private void LogRequestBody(string body)
    {
        var sanitizedBody = body?
            .Replace(Environment.NewLine, string.Empty)
            .Replace("\n", string.Empty)
            .Replace("\r", string.Empty);

        var msg = $"Request body({body?.Length}):\n{sanitizedBody}";
        Debug.WriteLine(msg);
        _logger.LogDebug(msg);
    }

    protected virtual async Task ResponsePlainTextAsync(HttpContext context, string content, int statusCode = StatusCodes.Status200OK)
    {
        var responseBuilder = new WeixinResponsePlainTextBuilder(context)
        {
            StatusCode = statusCode,
            Content = content ?? ""
        };
        await responseBuilder.FlushAsync();
    }
}
