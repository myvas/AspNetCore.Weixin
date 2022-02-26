using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class TextMessageWeixinHandler : WeixinHandler, IWeixinHandler<TextMessageReceivedXml>
{
    public TextMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public TextMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<TextMessageReceivedXml>(Text);

        var ctx = new WeixinResultContext<TextMessageReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnTextMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
