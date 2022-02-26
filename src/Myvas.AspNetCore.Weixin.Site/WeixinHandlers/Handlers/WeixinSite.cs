using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin.Site.ResponseBuilder
{
    public class WeixinSite
    {
        private readonly WeixinSiteOptions _options;
        private readonly ILogger _logger;
        private readonly IWeixinResponseBuilder WeixinResponseBuilder;

        public readonly IWeixinHandlerFactory _handlerFactory;

        public readonly IServiceProvider _serviceProvider;


        public WeixinSite(IOptions<WeixinSiteOptions> optionsAccessor,
            IWeixinResponseBuilder responseBuilder,
            IWeixinHandlerFactory handlerFactory,
            IServiceProvider serviceProvider,
            ILogger<WeixinSite> logger)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            WeixinResponseBuilder = responseBuilder ?? throw new ArgumentNullException(nameof(responseBuilder));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task ProcessAsync(HttpContext context)
        {
            var text = "";
            context.Request.EnableBuffering();
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                text = await reader.ReadToEndAsync();
            }
            _logger.LogTrace("Request Body({length}): {text}", text?.Length, text);

            try
            {
                //IWeixinHandler handler = _handlerFactory.Create<WeixinHandler>();
                //handler.Context = context;
                //handler.Text = text;
                //await handler.
                await ProcessAsync(context, text);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException($"消息无法识别", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="Text">
        /// <code>
        ///<?xml version="1.0" encoding="utf-8"?>
        ///<xml>
        ///  <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
        ///  <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
        ///  <CreateTime>1357986928</CreateTime>
        ///  <MsgType><![CDATA[text]]></MsgType>
        ///  <Content><![CDATA[中文]]></Content>
        ///  <MsgId>5832509444155992350</MsgId>
        ///</xml>
        /// </code>
        /// </param>
        /// <returns></returns>
        private async Task<bool> ProcessAsync(HttpContext Context, string Text)
        {
            var doc = XDocument.Parse(Text);

            RequestMsgType msgType = RequestMsgType.Unknown;
            RequestEventType? eventType = RequestEventType.Unknown;
            try
            {
                string sMsgType = doc.Root.Element("MsgType").Value;
                msgType = (RequestMsgType)Enum.Parse(typeof(RequestMsgType), sMsgType, true);
            }
            catch { msgType = RequestMsgType.Unknown; }
            if (msgType == RequestMsgType.@event)
            {
                var sEventType = doc.Root.Element("Event").Value;
                try
                {
                    eventType = (RequestEventType)Enum.Parse(typeof(RequestEventType), sEventType, true);
                    msgType = RequestMsgType.@event;
                }
                catch { eventType = RequestEventType.Unknown; }
            }

            try
            {
                IWeixinHandler handler = null;
                switch (msgType)
                {
                    case RequestMsgType.text:
                        handler = _handlerFactory.Create<TextMessageWeixinHandler>(_serviceProvider); break;
                    case RequestMsgType.image:
                        handler = _handlerFactory.Create<ImageMessageWeixinHandler>(_serviceProvider); break;
                    case RequestMsgType.video:
                        handler = _handlerFactory.Create<VideoMessageWeixinHandler>(_serviceProvider); break;
                    case RequestMsgType.shortvideo:
                        handler = _handlerFactory.Create<ShortVideoMessageWeixinHandler>(_serviceProvider); break;
                    case RequestMsgType.voice:
                        handler = _handlerFactory.Create<VoiceMessageWeixinHandler>(_serviceProvider); break;
                    case RequestMsgType.location:
                        handler = _handlerFactory.Create<LocationMessageWeixinHandler>(_serviceProvider); break;
                    case RequestMsgType.link:
                        handler = _handlerFactory.Create<LinkMessageWeixinHandler>(_serviceProvider); break;
                    case RequestMsgType.@event:
                        switch (eventType)
                        {
                            case RequestEventType.subscribe:
                                handler = _handlerFactory.Create<SubscribeEventWeixinHandler>(_serviceProvider); break;
                            case RequestEventType.CLICK:
                                handler = _handlerFactory.Create<ClickMenuEventWeixinHandler>(_serviceProvider); break;
                            case RequestEventType.VIEW:
                                handler = _handlerFactory.Create<ViewMenuEventWeixinHandler>(_serviceProvider); break;
                            case RequestEventType.SCAN:
                                handler = _handlerFactory.Create<QrscanEventWeixinHandler>(_serviceProvider); break;
                            case RequestEventType.LOCATION:
                                handler = _handlerFactory.Create<LocationEventWeixinHandler>(_serviceProvider); break;
                            default:
                                //throw new NotSupportedException($"系统无法处理此事件");
                                _logger.LogWarning("微信事件类型为[{eventType}]的事件未得到处理。", eventType);
                                await WeixinResponseBuilder.FlushStatusCode(Context, StatusCodes.Status400BadRequest);
                                return false;
                        }
                        break;
                    default:
                        //throw new NotSupportedException($"系统无法处理此消息");
                        _logger.LogWarning("微信消息类型为[{msgType}]的消息未得到处理。", msgType);
                        await WeixinResponseBuilder.FlushStatusCode(Context, StatusCodes.Status400BadRequest);
                        return false;
                }
                handler.Context = Context;
                handler.Text = Text;
                return await handler.ProcessAsync();
            }
            catch (Exception ex)
            {
                //throw new NotSupportedException($"系统异常", ex);
                _logger.LogError("微信请求处理时发生异常！{exception}", ex.Message);
                await WeixinResponseBuilder.FlushStatusCode(Context, StatusCodes.Status400BadRequest);
                return false;
            }
        }
    }
}
