using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class QrscanEventWeixinHandler : WeixinHandler, IWeixinHandler<QrscanEventReceivedXml>
{
    public QrscanEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public QrscanEventReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<QrscanEventReceivedXml>(Text);

        var ctx = new WeixinResultContext<QrscanEventReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnQrscanEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
