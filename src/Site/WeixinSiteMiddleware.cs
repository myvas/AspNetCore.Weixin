using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// This middleware provides a default welcome/validation page for new Weixin App.
/// </summary>
public class WeixinSiteMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly WeixinSiteOptions _options;
    private readonly IWeixinHandler _handler;

    public WeixinSiteMiddleware(
        RequestDelegate next,
        IWeixinHandler handler,
        IOptions<WeixinSiteOptions> siteOptionsAccessor,
        ILoggerFactory loggerFactory)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = loggerFactory?.CreateLogger<WeixinSiteMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        _options = siteOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(siteOptionsAccessor));

        //入参检查
        if (string.IsNullOrEmpty(_options.WebsiteToken))
        {
            throw new ArgumentException($"Options '{nameof(_options.WebsiteToken)}' cannot be null or empty");
        }

        if (string.IsNullOrEmpty(_options.Path))
        {
            throw new ArgumentException($"Options '{nameof(_options.Path)}' cannot be null or empty");
        }

        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }

    /// <summary>
    /// Process an individual request.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <returns></returns>
    public Task Invoke(HttpContext context)
    {
        var path = _options.Path;

        HttpRequest request = context.Request;
        if (path.Equals(request.Path, StringComparison.OrdinalIgnoreCase))
        {
#if DEBUG
            var sanitizedPath = request.Path.Value?.Replace(Environment.NewLine, "")?.Replace("\n", "")?.Replace("\r", "");
            _logger.LogDebug($"Request matched the WeixinSite path '{sanitizedPath}'");
#endif
            if (HttpMethods.IsPost(request.Method))
            {
                return InvokePostAsync(context);
            }
            else
            {
                _logger.LogDebug($"A WeixinSite verification request found");
                return InvokeGetAsync(context);
            }
        }
        else
        {
#if DEBUG
            var sanitizedPath = request.Path.Value?.Replace(Environment.NewLine, "")?.Replace("\n", "")?.Replace("\r", "");
            _logger.LogDebug($"The request path '{sanitizedPath}' does not match the WeixinSite path '{path}'");
#endif
            return _next(context);
        }
    }

    /// <summary>
    /// 指示微信公众号消息接收地址是否可用
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeGetAsync(HttpContext context)
    {
        // 明文模式：GET http://wx.steamlet.com/wx?signature=c48abd8c533cba0ff7230e9b5b78d8970bef70e8&echostr=3851421201668733949&timestamp=1553423097&nonce=1968330093
        // 兼容模式：GET http://wx.steamlet.com/wx?signature=2b3a50d442885f8eef72f4d10bef472df12d1675&echostr=4780759847666154895&timestamp=1553423758&nonce=2002608571
        // 安全模式：GET http://wx.steamlet.com/wx?signature=153cf3051ee1de57b31cab8c16a1c08e7ebc7a18&echostr=4882545160925183999&timestamp=1553423855&nonce=659627225

        HttpRequest request = context.Request;
        var signature = request.Query["signature"];
        var echostr = request.Query["echostr"];
        var timestamp = request.Query["timestamp"];
        var nonce = request.Query["nonce"];

        var websiteToken = _options.WebsiteToken;

        var response = new PlainTextResponseBuilder(context);
        if (SignatureHelper.ValidateSignature(signature, timestamp, nonce, websiteToken)) //【腾讯微信公众号后台程序】发起服务器地址验证
        {
            response.Content = echostr;//返回随机字符串则表示验证通过
        }
        else//【配置管理员】核实AspNetCore.Weixin服务地址
        {
            response.Content = Resources.WeixinSiteVerificationPathNotice;
        }
        await response.FlushAsync();
    }

    /// <summary>
    /// 微信公众号消息接收地址
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokePostAsync(HttpContext context)
    {
        HttpRequest request = context.Request;
        var signature = request.Query["signature"];
        var timestamp = request.Query["timestamp"];
        var nonce = request.Query["nonce"];

        var websiteToken = _options.WebsiteToken;

        if (_options.Debug)
        {
            await HandleAsync(context);
            return;
        }

        if (!SignatureHelper.ValidateSignature(signature, timestamp, nonce, websiteToken))
        {
            var response = new PlainTextResponseBuilder(context);
            response.Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Content = Resources.InvalidSignatureDenied;
            await response.FlushAsync();
            return;
        }

        (bool isMicroMessenger, string version) = BrowserDetector.DetectMicroMessenger(context);
        if (!isMicroMessenger)
        {
            var response = new PlainTextResponseBuilder(context);
            response.Context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Content = Resources.NonMicroMessengerDenied;
            await response.FlushAsync();
            return;
        }

        {
            await HandleAsync(context);
            return;
        }
    }


    private async Task HandleAsync(HttpContext context)
    {
        using (var stream = new StreamReader(context.Request.Body))
        {
            var text = await stream.ReadToEndAsync();
#if DEBUG
            var sanitizedText = text?.Replace(Environment.NewLine, "")?.Replace("\n", "")?.Replace("\r", "");
            _logger.LogDebug($"Request body({text?.Length}): {sanitizedText}");
#endif
            Debug.WriteLine($"Request body({text?.Length}):");
            Debug.WriteLine(text);

            IWeixinHandler handler = _handler;
            handler.Context = context;
            handler.Text = text;
            await handler.HandleAsync();
        }
    }
}
