using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinHandler : IWeixinHandler<ReceivedXml>
    {
        private readonly ILogger _logger;
        private readonly WeixinSiteOptions _options;

        public WeixinHandler(ILoggerFactory logger,
            IOptions<WeixinSiteOptions> optionsAccessor
        )
        {
            _logger = logger?.CreateLogger<WeixinHandler>() ?? throw new ArgumentNullException(nameof(logger));
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public HttpContext Context { get; set; }
        public string Text { get; set; }
        public ReceivedXml Xml { get; set; }

        public async Task<bool> HandleAsync()
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
                switch (msgType)
                {
                    case RequestMsgType.text:
                        return await FireEventAsync(_options.Events.OnTextMessageReceived);
                    case RequestMsgType.image:
                        return await FireEventAsync(_options.Events.OnImageMessageReceived);
                    case RequestMsgType.voice:
                        return await FireEventAsync(_options.Events.OnVoiceMessageReceived);
                    case RequestMsgType.video:
                        return await FireEventAsync(_options.Events.OnVideoMessageReceived);
                    case RequestMsgType.shortvideo:
                        return await FireEventAsync(_options.Events.OnShortVideoMessageReceived);
                    case RequestMsgType.location:
                        return await FireEventAsync(_options.Events.OnLocationMessageReceived);
                    case RequestMsgType.@event:
                        switch (eventType)
                        {
                            case RequestEventType.subscribe:
                                return await FireEventAsync(_options.Events.OnSubscribeEventReceived);
                            case RequestEventType.CLICK:
                                return await FireEventAsync(_options.Events.OnClickMenuEventReceived);
                            case RequestEventType.VIEW:
                                return await FireEventAsync(_options.Events.OnViewMenuEventReceived);
                            case RequestEventType.SCAN:
                                return await FireEventAsync(_options.Events.OnQrscanEventReceived);
                            case RequestEventType.unsubscribe:
                                return await FireEventAsync(_options.Events.OnUnsubscribeEventReceived);
                            default:
                                throw new NotSupportedException($"系统无法识别处理此事件");
                        }
                    default:
                        throw new NotSupportedException($"系统无法识别处理此消息");
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException($"系统在解析处理微信消息时发生异常", ex);
            }
        }

        private async Task<bool> FireEventAsync<TReceivedXml>(Func<WeixinReceivedContext<TReceivedXml>, Task<bool>> eventHandler)
            where TReceivedXml : ReceivedXml
        {
            Xml = MyvasXmlConvert.DeserializeObject<TReceivedXml>(Text);
            var ctx = new WeixinReceivedContext<TReceivedXml>(Context, Text, Xml);
            var handled = await eventHandler(ctx);
            if (!handled)
            {
                return await DefaultResponseAsync();
            }
            return true;
        }

        private async Task<bool> DefaultResponseAsync()
        {
            var responseBuilder = new PlainTextResponseBuilder(Context);
            responseBuilder.Content = "信息已收到";
            await responseBuilder.FlushAsync();
            return true;
        }
    }
}
