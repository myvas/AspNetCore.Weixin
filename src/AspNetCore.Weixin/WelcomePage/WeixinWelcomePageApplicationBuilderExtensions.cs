using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
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
