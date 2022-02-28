using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Helpers;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class ClickMenuEventWeixinHandler : WeixinHandler, IWeixinHandler<ClickMenuEventReceivedXml>
{
    public ClickMenuEventWeixinHandler(ILogger<WeixinHandler> logger, IWeixinResponseBuilder responseBuilder, IWeixinEventSink eventSink, IOptions<WeixinSiteOptions> optionsAccessor) : base(logger, responseBuilder, eventSink, optionsAccessor)
    {
    }

    ///<inheritdoc />
    public ClickMenuEventReceivedXml Xml { get; set; }

    ///<inheritdoc />
    public override async Task<bool> ProcessAsync()
    {
        Xml = WeixinXmlConvert.DeserializeObject<ClickMenuEventReceivedXml>(Text);

        var ctx = new WeixinEventContext<ClickMenuEventReceivedXml>(Context, Text, Xml);
        var handled = await _eventSink.OnClickMenuEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }
}
