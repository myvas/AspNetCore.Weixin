using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class SubscribeEventWeixinHandler : WeixinHandler, IWeixinHandler<SubscribeEventReceivedXml>
{
    public SubscribeEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public SubscribeEventReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<SubscribeEventReceivedXml>(Text);

        var ctx = new WeixinEventContext<SubscribeEventReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnSubscribeEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
