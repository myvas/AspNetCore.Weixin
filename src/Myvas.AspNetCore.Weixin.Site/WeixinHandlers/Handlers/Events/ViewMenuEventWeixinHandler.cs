using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ViewMenuEventWeixinHandler : WeixinHandler, IWeixinHandler<ViewMenuEventReceivedXml>
{
    public ViewMenuEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public ViewMenuEventReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<ViewMenuEventReceivedXml>(Text);

        var ctx = new WeixinEventContext<ViewMenuEventReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnViewMenuEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}