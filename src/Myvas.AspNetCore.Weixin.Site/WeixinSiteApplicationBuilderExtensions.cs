using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// IApplicationBuilder extensions for the <see cref="WeixinSiteMiddleware"/>.
    /// </summary>
    public static class WeixinSiteApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="WeixinSiteMiddleware"/> to the pipeline with the given options.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app, WeixinSiteEndpointOptions options)
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

        /// <summary>
        /// Adds the <see cref="WeixinSiteMiddleware"/> to the pipeline with the given path.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app, PathString path)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            return app.UseWeixinSite(new WeixinSiteEndpointOptions
            {
                Path = path
            });
        }

        /// <summary>
        /// Adds the <see cref="WeixinSiteMiddleware"/> to the pipeline with the given path.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app, string path)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            return app.UseWeixinSite(new WeixinSiteEndpointOptions
            {
                Path = new PathString(path)
            });
        }

        /// <summary>
        /// Adds the <see cref="WeixinSiteMiddleware"/> to the pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<WeixinSiteMiddleware>();
        }
    }
}
