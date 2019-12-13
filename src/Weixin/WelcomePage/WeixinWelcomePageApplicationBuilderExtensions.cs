using Microsoft.AspNetCore.Builder;
using System;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// IApplicationBuilder extensions for the WeixinWelcomePageMiddleware.
    /// </summary>
    public static class WeixinWelcomePageApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the WeixinWelcomePageMiddleware to the pipeline with the DI options.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseWeixinWelcomePage(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<WeixinWelcomePageMiddleware>();
        }
    }
}
