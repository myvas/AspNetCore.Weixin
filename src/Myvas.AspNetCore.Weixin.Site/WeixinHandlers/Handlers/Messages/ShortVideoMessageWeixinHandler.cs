using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ShortVideoMessageWeixinHandler : WeixinHandler,IWeixinHandler<ShortVideoMessageReceivedXml>
{
    public ShortVideoMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, optionsAccessor)
    {
    }

    public ShortVideoMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<ShortVideoMessageReceivedXml>(Text);

        var ctx = new WeixinResultContext<ShortVideoMessageReceivedXml>(Context, Text, Xml);
        var handled = await _options.Events.ShortVideoMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}