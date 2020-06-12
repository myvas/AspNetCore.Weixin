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
        private readonly IWeixinReceivedMessageStore _store;

        public MessageWeixinHandler(ILogger<WeixinHandler> logger,
            IWeixinReceivedMessageStore store,
            IWeixinHandlerFactory handlerFactory
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _store = store ?? throw new ArgumentNullException(nameof(store));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
        }

        public HttpContext Context { get; set; }
        public string Text { get; set; }
        public MessageReceivedXml Xml { get; set; }

        public async Task<bool> ProcessAsync()
        {
            var xml = XmlConvert.DeserializeObject<MessageReceivedXml>(Text);
            Xml = xml;
            try
            {
                var entity = new WeixinReceivedMessage();
                entity.FromUserName = xml.FromUserName;
                entity.ToUserName = xml.ToUserName;
                entity.CreateTime = xml.CreateTimeStr;
                entity.MsgType = xml.MsgTypeStr;
                entity.MsgId = xml.MsgId;
                await _store.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "收到一个微信上行消息，但在存储时发生异常");
            }

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
