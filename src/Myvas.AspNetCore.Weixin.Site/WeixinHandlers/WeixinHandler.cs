using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinHandler : IWeixinHandler
    {
        private readonly ILogger _logger;
        private readonly IWeixinHandlerFactory _handlerFactory;

        public WeixinHandler(ILogger<WeixinHandler> logger,
            IWeixinHandlerFactory handlerFactory
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
        }

        public HttpContext Context { get; set; }
        public string Text { get; set; }
        public ReceivedXml Xml { get; set; }

        public async Task<bool> ProcessAsync()
        {
            var doc = XDocument.Parse(Text);

            RequestMsgType msgType = RequestMsgType.Unknown;
            RequestEventType? eventType =  RequestEventType.Unknown;
            try
            {
                string sMsgType = doc.Root.Element("MsgType").Value;
                msgType = (RequestMsgType)Enum.Parse(typeof(RequestMsgType), sMsgType, true);
            }
            catch { msgType = RequestMsgType.Unknown; }
            if(msgType== RequestMsgType.@event)
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
                        handler = _handlerFactory.Create<TextMessageWeixinHandler>(); break;
                    case RequestMsgType.image:
                        //handler = _handlerFactory.Create<ImageWeixinHandler>(); break;
                    case RequestMsgType.@event:
                            switch (eventType)
                            {
                                case RequestEventType.subscribe:
                                    handler = _handlerFactory.Create<SubscribeEventWeixinHandler>(); break;
                                case RequestEventType.CLICK:
                                    //handler = _handlerFactory.Create<ClickEventWeixinHandler>(); break;
                                default:
                                throw new NotSupportedException($"系统无法处理此事件");
                            }break;
                    default:
                        throw new NotSupportedException($"系统无法处理此消息");
                }
                handler.Context = Context;
                handler.Text = Text;
                return await handler.ProcessAsync();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException($"系统异常", ex);
            }
        }
    }
}
