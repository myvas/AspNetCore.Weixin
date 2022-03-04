using Myvas.AspNetCore.Weixin.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class LocationEventWeixinHandler : WeixinHandler, IWeixinHandler<LocationEventReceivedXml>
{
    public LocationEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public LocationEventReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = WeixinXmlConvert.DeserializeObject<LocationEventReceivedXml>(Text);

        var ctx = new WeixinEventContext<LocationEventReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnLocationEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
