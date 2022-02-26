using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class QrscanEventWeixinHandler : IWeixinHandler<QrscanEventReceivedXml>
{
    private readonly ILogger _logger;
    private readonly IWeixinHandlerFactory _handlerFactory;
    private readonly WeixinSiteOptions _options;

    public QrscanEventWeixinHandler(ILogger<WeixinHandler> logger,
        IOptions<WeixinSiteOptions> optionsAccessor,
        IWeixinHandlerFactory handlerFactory
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
    }

    public HttpContext Context { get; set; }
    public string Text { get; set; }
    public QrscanEventReceivedXml Xml { get; set; }

    public async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<QrscanEventReceivedXml>(Text);

        var ctx = new WeixinReceivedContext<QrscanEventReceivedXml>(Context, Text, Xml);
        var handled = await _options.Events.QrscanEventReceived(ctx);
        if (!handled)
        {
            return await DefaultResponseAsync();
        }
        return true;
    }

    private async Task<bool> DefaultResponseAsync()
    {
        await WeixinResponseBuilder.FlushStatusCode(Context);
        return true;
    }
}
