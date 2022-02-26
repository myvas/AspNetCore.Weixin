using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ShortVideoMessageWeixinHandler : WeixinHandler,IWeixinHandler<ShortVideoMessageReceivedXml>
{
    public ShortVideoMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public ShortVideoMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<ShortVideoMessageReceivedXml>(Text);

        var ctx = new WeixinResultContext<ShortVideoMessageReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnShortVideoMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}