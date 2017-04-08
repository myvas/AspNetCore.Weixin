using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// This middleware provides a default welcome/validation page for new Weixin App.
    /// </summary>
    public class WeixinWelcomePageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _backchannel;
        private readonly WeixinWelcomePageOptions _options;
        private readonly ILogger _logger;

        public WeixinWelcomePageMiddleware(
            RequestDelegate next,
            IOptions<WeixinWelcomePageOptions> options,
            ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<WeixinWelcomePageMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

            //入参检查
            if (string.IsNullOrEmpty(_options.WebsiteToken))
            {
                throw new ArgumentException($"参数 {nameof(_options.WebsiteToken)} 不能为空。");
            }

            if (string.IsNullOrEmpty(_options.Path))
            {
                throw new ArgumentException($"参数 {nameof(_options.Path)} 不能为空。");
            }

            _backchannel = new HttpClient(new HttpClientHandler());
            _backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("AspNetCoreWeixin");
            _backchannel.Timeout = TimeSpan.FromSeconds(60);
            _backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
        }

        //protected WeixinMessageHandler CreateHandler()
        //{
        //    return new WeixinMessageHandler(_backchannel);
        //}

        /// <summary>
        /// Process an individual request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var welcomePath = _options.PathString;

            HttpRequest request = context.Request;
            if (request.Path != welcomePath) await _next(context);

            // Dynamically generated for LOC.
            if (string.Compare(request.Method, "POST", true) == 0)
            {
                await InvokePost(context);
            }
            else
            {
                await InvokeGet(context);
            }
        }

        /// <summary>
        /// 微信公众号消息接收地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokePost(HttpContext context)
        {
            HttpRequest request = context.Request;
            var signature = request.Query["signature"];
            var timestamp = request.Query["timestamp"];
            var nonce = request.Query["nonce"];
            var echostr = request.Query["echostr"];
            var token = _options.WebsiteToken;

            context.Response.Clear();
            context.Response.ContentType = "text/plain;charset=utf-8";
            if (_options.WeixinClientAccessOnly && !Signature.Check(signature, timestamp, nonce, token))
            {
                var result = "这是一个微信程序，请用微信客户端访问。";
                await context.Response.WriteAsync(result);
            }

            await InvokePostInternal(context);
        }

        public async Task InvokePostInternal(HttpContext context)
        {
            var messageHandler = new WeixinMessageHandler();
            try
            {
                await messageHandler.InitializeAsync(_options, context, _logger);
                var result = await messageHandler.HandleAsync();
                if (!result.Handled)
                {
                    await _next(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "处理POST报文时发生异常");
            }
            finally
            {
                try { await messageHandler.TeardownAsync(); }
                catch (Exception)
                {
                    // Don't mask the original exception, if any
                }
            }
        }

        /// <summary>
        /// 指示微信公众号消息接收地址是否可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeGet(HttpContext context)
        {
            HttpRequest request = context.Request;
            var signature = request.Query["signature"];
            var timestamp = request.Query["timestamp"];
            var nonce = request.Query["nonce"];
            var echostr = request.Query["echostr"];
            var token = _options.WebsiteToken;

            context.Response.Clear();
            context.Response.ContentType = "text/plain;charset=utf-8";

            if (Signature.Check(signature, timestamp, nonce, token))
            {
                var query = new QueryBuilder() {
                    { "signature",signature.ToString() },
                    { "timestamp",timestamp.ToString() },
                    { "nonce",nonce.ToString() },
                    { "echostr",echostr.ToString() },
                    { "token",token.ToString() }
                };
                _logger.LogDebug(query.ToString());
                await context.Response.WriteAsync(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                string signature2 = Signature.GetSignature(timestamp, nonce, token);
                string result = "如果你在浏览器中看到这句话，说明这个地址可以用于微信公众号消息接口地址。(开发/基本配置/URL)";
                //+"测试请用验证码(Signature)：" + signature2;
                await context.Response.WriteAsync(result); //返回随机字符串则表示验证通过
            }
        }
    }
}
