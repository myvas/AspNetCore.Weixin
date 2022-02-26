﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Site.ResponseBuilder;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// This middleware provides an endpoint to validate Weixin signature, and a series of message handlers to communicate to Tencent servers.
    /// </summary>
    public class WeixinSiteMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WeixinSiteEndpointOptions _options;
        private readonly IWeixinResponseBuilder WeixinResponseBuilder;

        private readonly WeixinSiteOptions _serviceOptions;
        private readonly ILogger _logger;
        private readonly WeixinSite _site;

        public WeixinSiteMiddleware(
            RequestDelegate next,
            IOptions<WeixinSiteEndpointOptions> optionsAccessor,
            WeixinSite site,
            IOptions<WeixinSiteOptions> siteOptionsAccessor,
            IWeixinResponseBuilder responseBuilder,
            ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            WeixinResponseBuilder = responseBuilder ?? throw new ArgumentNullException(nameof(responseBuilder));

            if (string.IsNullOrWhiteSpace(_options.Path))
            {
                throw new ArgumentException($"参数 {nameof(_options.Path)} 不能为空。");
            }

            _site = site;
            _logger = loggerFactory?.CreateLogger<WeixinSiteMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _serviceOptions = siteOptionsAccessor?.Value ?? throw new ArgumentNullException(nameof(siteOptionsAccessor));

            //入参检查
            if (string.IsNullOrEmpty(_serviceOptions.WebsiteToken))
            {
                throw new ArgumentException($"参数 {nameof(_serviceOptions.WebsiteToken)} 不能为空。");
            }
        }

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;
            //if (!_options.Path.HasValue || _options.Path == request.Path)
            if (_options.Path == request.Path)
            {
                // Dynamically generated for LOC.
                if (request.Method == HttpMethods.Get)
                {
                    return InvokeGetAsync(context);
                }
                else
                {
                    return InvokePostAsync(context);
                }
            }

            return _next(context);
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

            var websiteToken = _serviceOptions.WebsiteToken;

            if (SignatureHelper.ValidateSignature(signature, timestamp, nonce, websiteToken)) //【腾讯微信公众号后台程序】发起服务器地址验证
            {
                //返回随机字符串则表示验证通过
                await WeixinResponseBuilder.FlushPlainText(context, echostr);
            }
            else//【配置管理员】核实AspNetCore.Weixin服务地址
            {
                var s = "如果你在浏览器中看到这句话，说明本网站是微信公众号服务器，可以在微信公众号后台的“开发/基本配置/服务器配置/服务器地址(URL)”字段中填写此URL！";
                await WeixinResponseBuilder.FlushPlainText(context, s);
            }
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

            var websiteToken = _serviceOptions.WebsiteToken;

            if (!_serviceOptions.Debug && !SignatureHelper.ValidateSignature(signature, timestamp, nonce, websiteToken))
            {
                var s = "这是一个微信程序，请用微信客户端访问!";
                await WeixinResponseBuilder.FlushPlainText(context, s);
                return;
            }
            else
            {
                await _site.ProcessAsync(context);
                return;
            }
        }
    }
}