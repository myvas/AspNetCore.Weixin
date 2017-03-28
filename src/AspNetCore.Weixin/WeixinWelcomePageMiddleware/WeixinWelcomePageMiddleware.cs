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

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();

            {
                var result = new StringBuilder();
                result.AppendFormat("您刚才发送了文字信息：{0}\r\n\r\n", e.Content);

                MessageContext CurrentMessageContext = messageHandler.CurrentMessageContext;
                if (CurrentMessageContext.RequestMessages.Count > 1)
                {
                    result.AppendFormat("您刚才还发送了如下消息（{0}/{1}）：\r\n", CurrentMessageContext.RequestMessages.Count, CurrentMessageContext.StorageData);
                    for (int i = CurrentMessageContext.RequestMessages.Count - 2; i >= 0; i--)
                    {
                        var historyMessage = CurrentMessageContext.RequestMessages[i];
                        result.AppendFormat(
                            "{0} 【{1}】{2}\r\n",
                            historyMessage.CreateTime.ToString("yyyy-MM-dd"),
                            historyMessage.MsgType.ToString(),
                            (historyMessage is RequestMessageText)
                            ? (historyMessage as RequestMessageText).Content
                            : "[非文字类型]"
                            );
                    }
                    result.AppendLine("\r\n");
                }

                result.AppendFormat("如果您在{0}分钟内连续发送消息，记录将被自动保留（当前设置：最多记录{1}条）。过期后记录将会自动清除。\r\n",
                    messageHandler.WeixinContext.ExpireMinutes,
                    messageHandler.WeixinContext.MaxRecordCount);

                responseMessage.Content = result.ToString();
            }
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_LinkMessageReceived(object sender, LinkMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<LinkMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnLinkMessageReceived(ev)) return;

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format(@"您发送了一条连接信息：
            Title：{0}
            Description:{1}
            Url:{2}", e.Title, e.Description, e.Url);
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_VideoMessageReceived(object sender, VideoMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<VideoMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnVideoMessageReceived(ev)) return;

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了一条视频信息，ID：" + e.MediaId;
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_VoiceMessageReceived(object sender, VoiceMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<VoiceMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnVoiceMessageReceived(ev)) return;

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = e.MediaId;
            responseMessage.Music.Title = "语音";
            responseMessage.Music.Description = "这里是一条语音消息";
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_ImageMessageReceived(object sender, ImageMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<ImageMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnImageMessageReceived(ev)) return;

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = "您刚才发送了图片信息",
                Description = "您发送的图片将会显示在边上",
                PicUrl = e.PicUrl,
                Url = "http://wx.demo.com"
            });
            responseMessage.Articles.Add(new Article()
            {
                Title = "第二条",
                Description = "第二条带连接的内容",
                PicUrl = e.PicUrl,
                Url = "http://wx.demo.com"
            });
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_LocationMessageReceived(object sender, LocationMessageReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<LocationMessageReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnLocationMessageReceived(ev)) return;

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageNews>();

            var markersList = new List<GoogleMapMarkers>();
            markersList.Add(new GoogleMapMarkers()
            {
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                Color = "red",
                Label = "S",
                Size = GoogleMapMarkerSize.Default,
            });
            var mapSize = "480x600";
            var mapUrl = GoogleMapHelper.GetGoogleStaticMap(19 /*requestMessage.Scale*//*微信和GoogleMap的Scale不一致，这里建议使用固定值*/,
                                                            markersList, mapSize);
            responseMessage.Articles.Add(new Article()
            {
                Description = string.Format("您刚才发送了地理位置信息。Location_X：{0}，Location_Y：{1}，Scale：{2}，标签：{3}",
                              e.Latitude, e.Longitude,
                              e.Scale, e.Label),
                PicUrl = mapUrl,
                Title = "定位地点周边地图",
                Url = mapUrl
            });
            responseMessage.Articles.Add(new Article()
            {
                Title = "AspNetCore.Weixin",
                Description = "AspNetCore.Weixin",
                PicUrl = "http://wx.demo.com/logo.jpg",
                Url = "http://wx.demo.com"
            });

            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_LocationEventReceived(object sender, LocationEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<LocationEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnLocationEventReceived(ev)) return;

            //这里是微信客户端（通过微信服务器）自动发送过来的位置信息
            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("刚刚上报了一条定位信息：(Lat={0},Lon={1},Prc={2})",
                e.Latitude, e.Longitude, e.Precision);
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_ClickMenuEventReceived(object sender, ClickMenuEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<ClickMenuEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnClickMenuEventReceived(ev)) return;

            _logger.LogDebug("点击了子菜单按钮({0}): {1}", e.FromUserName, e.MenuItemKey);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("点击了子菜单按钮({0}): {1}", e.FromUserName, e.MenuItemKey);
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_ViewMenuEventReceived(object sender, ViewMenuEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<ViewMenuEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnViewMenuEventReceived(ev)) return;

            _logger.LogDebug("点击了子菜单按钮({0}): {1}", e.FromUserName, e.Url);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("点击了子菜单按钮({0}): {1}", e.FromUserName, e.Url);
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_UnsubscribeEventReceived(object sender, UnsubscribeEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<UnsubscribeEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnUnsubscribeEventReceived(ev)) return;

            _logger.LogDebug("Unsubscribe({0})", e.FromUserName);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("Unsubscribe({0})", e.FromUserName);
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_SubscribeEventReceived(object sender, SubscribeEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<SubscribeEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnSubscribeEventReceived(ev)) return;

            _logger.LogDebug("Subscribe: from:{0}", e.FromUserName);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageNews>();
            responseMessage.Articles.Add(new Article()
            {
                Title = "欢迎体验AspNetCore.Weixin演示系统",
                Description = "由AspNetCore.Weixin提供",
                PicUrl = "https://mp.weixin.qq.com/cgi-bin/getimgdata?mode=large&source=file&fileId=200121314%3E&token=977619473&lang=zh_CN",
                Url = "http://wx.demo.com"
            });
            messageHandler.ResponseMessage = responseMessage;
        }

        void messageHandler_QrscanEventReceived(object sender, QrscanEventReceivedEventArgs e)
        {
            var ev = new WeixinReceivedContext<QrscanEventReceivedEventArgs>(sender as MessageHandler<MessageContext>, e);
            if (_options.Events.OnQrscanEventReceived(ev)) return;

            _logger.LogDebug("Qrscan({0}): {1}, {2}", e.FromUserName, e.SceneId, e.Ticket);

            var messageHandler = sender as MessageHandler<MessageContext>;
            var responseMessage = messageHandler.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("Qrscan({0}): {1}, {2}", e.FromUserName, e.SceneId, e.Ticket);
            messageHandler.ResponseMessage = responseMessage;
        }
        #endregion

    }
}
