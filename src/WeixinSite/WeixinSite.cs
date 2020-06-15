using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinSite
    {
        private readonly WeixinSiteOptions _options;
        private readonly ILogger _logger;
        private readonly IWeixinHandler _handler;


        public WeixinSite(IOptions<WeixinSiteOptions> optionsAccessor,
            IWeixinHandler handler,
            ILoggerFactory loggerFactory)
        {
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _handler = handler ?? throw new ArgumentNullException();
            _logger = loggerFactory?.CreateLogger<WeixinSite>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task ProcessAsync(HttpContext context)
        {
            try { context.Request.EnableBuffering(); } catch(Exception ex)
            {
                throw new NotSupportedException($"系统异常", ex);
            }

            using (var stream = new MemoryStream())
            {
                // make sure that body is read from the beginning
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                await context.Request.Body.CopyToAsync(stream);
                var text = Encoding.UTF8.GetString(stream.ToArray());
                // this is required, otherwise model bind will return null
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                _logger.LogDebug($"Request body({text?.Length}): {text}");
                Debug.WriteLine($"Request body({text?.Length}):");
                Debug.WriteLine(text);

                try
                {
                    IWeixinHandler handler = _handler;
                    handler.Context = context;
                    handler.Text = text;
                    await handler.HandleAsync();
                }
                catch (Exception ex)
                {
                    throw new NotSupportedException($"消息无法识别", ex);
                }
            }
        }
    }
}
