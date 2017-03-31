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
        public Task Invoke(HttpContext context)
        {
            var welcomePath = _options.PathString;

            HttpRequest request = context.Request;
            if (request.Path != welcomePath) return _next(context);

            // Dynamically generated for LOC.
            if (string.Compare(request.Method, "POST", true) == 0)
            {
                InvokePost(context);
            }
            else
            {
                InvokeGet(context);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// 微信公众号消息接收地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InvokePost(HttpContext context)
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
                return context.Response.WriteAsync(result);
            }

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制
            int maxRecordCount = 0;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            WeixinMessageHandler messageHandler = new WeixinMessageHandler(request.Body, maxRecordCount);
            //共6种事件
            messageHandler.EnterEventReceived += messageHandler_EnterEventReceived;
            messageHandler.SubscribeEventReceived += messageHandler_SubscribeEventReceived;
            messageHandler.UnsubscribeEventReceived += messageHandler_UnsubscribeEventReceived;
            messageHandler.ClickMenuEventReceived += messageHandler_ClickMenuEventReceived;
            messageHandler.ViewMenuEventReceived += messageHandler_ViewMenuEventReceived;
            messageHandler.LocationEventReceived += messageHandler_LocationEventReceived;
            messageHandler.QrscanEventReceived += messageHandler_QrscanEventReceived;
            //共6种消息
            messageHandler.LocationMessageReceived += messageHandler_LocationMessageReceived;
            messageHandler.ImageMessageReceived += messageHandler_ImageMessageReceived;
            messageHandler.VoiceMessageReceived += messageHandler_VoiceMessageReceived;
            messageHandler.VideoMessageReceived += messageHandler_VideoMessageReceived;
            messageHandler.ShortVideoMessageReceived += messageHandler_ShortVideoMessageReceived;
            messageHandler.LinkMessageReceived += messageHandler_LinkMessageReceived;
            messageHandler.TextMessageReceived += messageHandler_TextMessageReceived;

            try
            {
                _logger.LogDebug(messageHandler.RequestDocument.ToString());
                messageHandler.Execute();
                _logger.LogDebug(messageHandler.ResponseDocument.ToString());

                if (messageHandler.ResponseDocument == null) return context.Response.WriteAsync("BadRequest");

                return context.Response.WriteAsync(messageHandler.ResponseDocument.ToString());//v0.7-
                //return new WeixinContentResult(messageHandler);//v0.8+
            }
            catch (Exception ex)
            {
                _logger.LogDebug("解析微信数据包时发生异常。", ex);
                if (messageHandler.ResponseDocument != null)
                {
                    _logger.LogDebug(messageHandler.ResponseDocument.ToString());
                }
                return context.Response.WriteAsync("解析微信数据包时发生异常。请通知系统管理员。谢谢!");
            }
        }

        /// <summary>
        /// 指示微信公众号消息接收地址是否可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InvokeGet(HttpContext context)
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
                return context.Response.WriteAsync(echostr); //返回随机字符串则表示验证通过
            }
            else
            {
                string signature2 = Signature.GetSignature(timestamp, nonce, token);
                string result = "如果你在浏览器中看到这句话，说明这个地址可以用于微信公众号消息接口地址。(开发/基本配置/URL)";
                //+"测试请用验证码(Signature)：" + signature2;
                return context.Response.WriteAsync(result); //返回随机字符串则表示验证通过
            }
        }

        #region 微信事件处理
        void messageHandler_TextMessageReceived(object sender, TextMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<TextMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnTextMessageReceived(ev)) return;
        }

        void messageHandler_LinkMessageReceived(object sender, LinkMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<LinkMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnLinkMessageReceived(ev)) return;
        }

        void messageHandler_VideoMessageReceived(object sender, VideoMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<VideoMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnVideoMessageReceived(ev)) return;
        }

        void messageHandler_ShortVideoMessageReceived(object sender, ShortVideoMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<ShortVideoMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnShortVideoMessageReceived(ev)) return;
        }

        void messageHandler_VoiceMessageReceived(object sender, VoiceMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<VoiceMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnVoiceMessageReceived(ev)) return;
        }

        void messageHandler_ImageMessageReceived(object sender, ImageMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<ImageMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnImageMessageReceived(ev)) return;
        }

        void messageHandler_LocationMessageReceived(object sender, LocationMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<LocationMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnLocationMessageReceived(ev)) return;
        }

        void messageHandler_LocationEventReceived(object sender, LocationEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<LocationEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnLocationEventReceived(ev)) return;
        }

        void messageHandler_ClickMenuEventReceived(object sender, ClickMenuEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<ClickMenuEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnClickMenuEventReceived(ev)) return;
        }

        void messageHandler_ViewMenuEventReceived(object sender, ViewMenuEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<ViewMenuEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnViewMenuEventReceived(ev)) return;
        }

        void messageHandler_UnsubscribeEventReceived(object sender, UnsubscribeEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<UnsubscribeEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnUnsubscribeEventReceived(ev)) return;
        }

        void messageHandler_EnterEventReceived(object sender, EnterEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<EnterEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnEnterEventReceived(ev)) return;
        }

        void messageHandler_SubscribeEventReceived(object sender, SubscribeEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<SubscribeEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnSubscribeEventReceived(ev)) return;
        }

        void messageHandler_QrscanEventReceived(object sender, QrscanEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<QrscanEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnQrscanEventReceived(ev)) return;
        }
        #endregion

    }
}
