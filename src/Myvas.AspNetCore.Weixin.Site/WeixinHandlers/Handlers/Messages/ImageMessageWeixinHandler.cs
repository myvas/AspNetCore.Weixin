using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Helpers;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ImageMessageWeixinHandler : WeixinHandler,IWeixinHandler<ImageMessageReceivedXml>
{
    public ImageMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public ImageMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = WeixinXmlConvert.DeserializeObject<ImageMessageReceivedXml>(Text);

        var ctx = new WeixinEventContext<ImageMessageReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnImageMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
