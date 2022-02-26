using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class LocationMessageWeixinHandler : WeixinHandler,IWeixinHandler<LocationMessageReceivedXml>
{
    public LocationMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public LocationMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<LocationMessageReceivedXml>(Text);

        var ctx = new WeixinResultContext<LocationMessageReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnLocationMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
