using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class LocationMessageWeixinHandler : WeixinHandler,IWeixinHandler<LocationMessageReceivedXml>
{
    public LocationMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, optionsAccessor)
    {
    }

    public LocationMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<LocationMessageReceivedXml>(Text);

        var ctx = new WeixinResultContext<LocationMessageReceivedXml>(Context, Text, Xml);
        var handled = await _options.Events.LocationMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
