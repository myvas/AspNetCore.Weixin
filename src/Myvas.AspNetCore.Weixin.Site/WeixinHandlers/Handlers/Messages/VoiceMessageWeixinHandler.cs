using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class VoiceMessageWeixinHandler : WeixinHandler, IWeixinHandler<VoiceMessageReceivedXml>
{
    public VoiceMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, optionsAccessor)
    {
    }

    public VoiceMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<VoiceMessageReceivedXml>(Text);

        var ctx = new WeixinResultContext<VoiceMessageReceivedXml>(Context, Text, Xml);
        var handled = await _options.Events.VoiceMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
