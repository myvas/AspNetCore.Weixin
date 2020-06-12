using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// IApplicationBuilder extensions for the WeixinWelcomePageMiddleware.
    /// </summary>
    public static class WeixinSiteApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the WeixinWelcomePageMiddleware to the pipeline with the DI options.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<WeixinSiteMiddleware>();
        }

        public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app, WeixinSiteOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<WeixinSiteMiddleware>(Options.Create(options));
        }

        public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app, string path)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseWeixinSite(new WeixinSiteOptions
            {
                Path = new PathString(path)
            });
        }
    }
}
