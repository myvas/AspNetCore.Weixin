using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class LocationMessageWeixinHandler : IWeixinHandler<LocationMessageReceivedXml>
{
    private readonly ILogger _logger;
    private readonly IWeixinHandlerFactory _handlerFactory;
    private readonly WeixinSiteOptions _options;

    public LocationMessageWeixinHandler(ILogger<WeixinHandler> logger,
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
    public LocationMessageReceivedXml Xml { get; set; }

    public async Task<bool> ProcessAsync()
    {
        Xml = XmlConvert.DeserializeObject<LocationMessageReceivedXml>(Text);

        var ctx = new WeixinReceivedContext<LocationMessageReceivedXml>(Context, Text, Xml);
        var handled = await _options.Events.LocationMessageReceived(ctx);
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
