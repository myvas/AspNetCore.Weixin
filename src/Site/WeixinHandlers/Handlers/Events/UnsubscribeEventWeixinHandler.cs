using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Helpers;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class UnsubscribeEventWeixinHandler : WeixinHandler, IWeixinHandler<UnsubscribeEventReceivedXml>
{
    public UnsubscribeEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    public UnsubscribeEventReceivedXml Xml { get; set; }

    public override async Task<bool> ProcessAsync()
    {
        Xml = WeixinXmlConvert.DeserializeObject<UnsubscribeEventReceivedXml>(Text);

        var ctx = new WeixinEventContext<UnsubscribeEventReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnUnsubscribeEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
