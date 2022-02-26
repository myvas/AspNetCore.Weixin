using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ViewMenuEventWeixinHandler : WeixinHandler, IWeixinHandler<ViewMenuEventReceivedXml>
{
    public ViewMenuEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, optionsAccessor)
    {
    }

    public ViewMenuEventReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<ViewMenuEventReceivedXml>(Text);

        var ctx = new WeixinResultContext<ViewMenuEventReceivedXml>(Context, Text, Xml);
        var handled = await _options.Events.ViewMenuEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}