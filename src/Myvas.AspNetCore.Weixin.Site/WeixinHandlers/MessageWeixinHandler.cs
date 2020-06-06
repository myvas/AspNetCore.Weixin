using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class MessageWeixinHandler : IWeixinHandler<MessageReceivedXml>
    {
        private readonly ILogger _logger;
        private readonly IWeixinHandlerFactory _handlerFactory;

        public MessageWeixinHandler(ILogger<WeixinHandler> logger,
            IWeixinHandlerFactory handlerFactory
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
        }

        public HttpContext Context { get; set; }
        public string Text { get; set; }
        public MessageReceivedXml Xml { get; set; }

        public async Task<bool> ProcessAsync()
        {
            var xml = XmlConvert.DeserializeObject<MessageReceivedXml>(Text);
            Xml = xml;

            IWeixinHandler handler = null;
            switch (xml.MsgType)
            {
                case RequestMsgType.text:
                    //handler = _handlerFactory.Create<TextMessageWeixinHandler>();
                    break;
                case RequestMsgType.location:
                    //handler = _handlerFactory.Create<LocationMessageWeixinHandler>();
                    break;
                default:
                    throw new NotSupportedException($"此消息系统无法识别。请通知系统管理员，谢谢。");
            }
            handler.Context = Context;
            handler.Text = Text;
            return await handler.ProcessAsync();
        }
    }
}
