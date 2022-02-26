using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class VideoMessageWeixinHandler : WeixinHandler, IWeixinHandler<VideoMessageReceivedXml>
{
    public VideoMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public VideoMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<VideoMessageReceivedXml>(Text);

        var ctx = new WeixinResultContext<VideoMessageReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnVideoMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
