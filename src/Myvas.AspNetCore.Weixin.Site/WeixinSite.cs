using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Site.ResponseBuilder
{
    public class WeixinSite
    {
        private readonly SiteOptions _options;
        private readonly ILogger _logger;

        public readonly ResponseBuilderFactory Response = new ResponseBuilderFactory();

        public readonly IWeixinHandlerFactory _handlerFactory;


        public WeixinSite(IOptions<SiteOptions> optionsAccessor,
            IWeixinHandlerFactory handlerFactory,
            ILogger<WeixinSite> logger)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _handlerFactory = handlerFactory ?? throw new ArgumentNullException(nameof(handlerFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ProcessAsync(HttpContext context)
        {
            var text = new StreamReader(context.Request.Body).ReadToEnd();
            _logger.LogDebug("Request Body({0}): {1}", text?.Length, text);

            try
            {
                IWeixinHandler handler = _handlerFactory.Create<WeixinHandler>();
                handler.Context = context;
                handler.Text = text;
                await handler.ProcessAsync();
            }
            catch (Exception ex)
            {
                throw new NotSupportedException($"消息无法识别", ex);
            }
        }
    }
}
