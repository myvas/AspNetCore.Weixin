using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class EventWeixinHandler : IWeixinHandler<EventReceivedXml>
{
    private readonly ILogger _logger;
    private readonly IWeixinHandlerFactory _handlerFactory;

    public EventWeixinHandler(ILogger<WeixinHandler> logger,
        IWeixinHandlerFactory handlerFactory
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
    }

    public HttpContext Context { get; set; }
    public string Text { get; set; }
    public EventReceivedXml Xml { get; set; }

    public async Task<bool> ProcessAsync()
    {
        var xml = XmlConvert.DeserializeObject<EventReceivedXml>(Text);
        Xml = xml;

        IWeixinHandler handler = null;
        switch (xml.Event)
        {
            case RequestEventType.subscribe:
                handler = _handlerFactory.Create<SubscribeEventWeixinHandler>();
                break;
            case RequestEventType.SCAN:
                //handler = _handlerFactory.Create<ScanEventWeixinHandler>();
                break;
            case RequestEventType.unsubscribe:
                throw new NotSupportedException($"此接口已被微信遗弃。请通知系统管理员，谢谢。");
            default:
                throw new NotSupportedException($"微信新增接口，系统无法识别。请通知系统管理员，谢谢。");
        }
        handler.Context = Context;
        handler.Text = Text;
        return await handler.ProcessAsync();
    }
}
