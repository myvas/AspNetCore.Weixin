using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Site.ResponseBuilder;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class SubscribeEventWeixinHandler : IWeixinHandler<SubscribeEventReceivedXml>
    {
        private readonly ILogger _logger;
        private readonly IWeixinHandlerFactory _handlerFactory;
        private readonly IResponseBuilderFactory _responseFactory;
        private readonly SiteOptions _options;

        public SubscribeEventWeixinHandler(ILogger<WeixinHandler> logger,
            IOptions<SiteOptions> optionsAccessor,
            IWeixinHandlerFactory handlerFactory,
            IResponseBuilderFactory responseFactory
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
        }

        public HttpContext Context { get; set; }
        public string Text { get; set; }
        public SubscribeEventReceivedXml Xml { get; set; }

        public async Task<bool> ProcessAsync()
        {
            Xml = XmlConvert.DeserializeObject<SubscribeEventReceivedXml>(Text);

            var ctx = new WeixinReceivedContext<SubscribeEventReceivedXml>(Context, Text, Xml);
            var handled = await _options.Events.SubscribeEventReceived(ctx);
            if (!handled)
            {
                return await DefaultResponseAsync();
            }
            return true;
        }

        private async Task<bool> DefaultResponseAsync()
        {
            var responseBuilder = _responseFactory.Create<TextResponseBuilder>(Context);
            responseBuilder.SetText("");
            await responseBuilder.FlushAsync();
            return true;
        }
    }
}
