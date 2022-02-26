using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class QrscanEventWeixinHandler : WeixinHandler, IWeixinHandler<QrscanEventReceivedXml>
{
    public QrscanEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, optionsAccessor)
    {
    }

    public QrscanEventReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<QrscanEventReceivedXml>(Text);

        var ctx = new WeixinResultContext<QrscanEventReceivedXml>(Context, Text, Xml);
        var handled = await _options.Events.QrscanEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
