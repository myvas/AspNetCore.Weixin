using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public abstract class WeixinHandler : IWeixinHandler
    {
        protected readonly ILogger _logger;
        protected readonly WeixinSiteOptions _options;
        protected readonly IWeixinResponseBuilder WeixinResponseBuilder;

        public WeixinHandler(ILogger<WeixinHandler> logger,
            IWeixinResponseBuilder responseBuilder,
            IOptions<WeixinSiteOptions> optionsAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            WeixinResponseBuilder = responseBuilder ?? throw new ArgumentNullException(nameof(responseBuilder));
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
        }

        public HttpContext Context { get; set; }
        public string Text { get; set; }

        public abstract Task<bool> ProcessAsync();

        protected virtual async Task<bool> DefaultResponseAsync()
        {
            await WeixinResponseBuilder.FlushStatusCode(Context);
            return true;
        }
    }
}
