﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class TextMessageWeixinHandler : IWeixinHandler<TextMessageReceivedXml>
    {
        private readonly ILogger _logger;
        private readonly IWeixinHandlerFactory _handlerFactory;
        private readonly IResponseBuilderFactory _responseFactory;
        private readonly WeixinSiteOptions _options;

        public TextMessageWeixinHandler(ILogger<WeixinHandler> logger,
            IOptions<WeixinSiteOptions> optionsAccessor,
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
        public TextMessageReceivedXml Xml { get; set; }

        public async Task<bool> ProcessAsync()
        {
            Xml = XmlConvert.DeserializeObject<TextMessageReceivedXml>(Text);

            var ctx = new WeixinReceivedContext<TextMessageReceivedXml>(Context, Text, Xml);
            var handled = await _options.Events.TextMessageReceived(ctx);
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