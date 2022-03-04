using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Helpers;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class LinkMessageWeixinHandler : WeixinHandler,IWeixinHandler<LinkMessageReceivedXml>
{
    public LinkMessageWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public LinkMessageReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = WeixinXmlConvert.DeserializeObject<LinkMessageReceivedXml>(Text);

        var ctx = new WeixinEventContext<LinkMessageReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnLinkMessageReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}