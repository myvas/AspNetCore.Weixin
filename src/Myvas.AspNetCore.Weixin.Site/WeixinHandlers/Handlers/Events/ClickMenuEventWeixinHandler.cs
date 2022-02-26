using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ClickMenuEventWeixinHandler : WeixinHandler, IWeixinHandler<ClickMenuEventReceivedXml>
{
    public ClickMenuEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public ClickMenuEventReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<ClickMenuEventReceivedXml>(Text);

        var ctx = new WeixinResultContext<ClickMenuEventReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnClickMenuEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
