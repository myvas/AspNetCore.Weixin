using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// IApplicationBuilder extensions for the WeixinWelcomePageMiddleware.
/// </summary>
public static class WeixinSiteApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the <see cref="WeixinSiteMiddleware"/> to the pipeline with the given path.
    /// </summary>
    /// <remarks>Usage:
    /// <code>
    /// app.UseWeixinSite(o =>
    /// {
    ///    o.Path = "/wx";
    ///    o.Debug = true;
    /// });
    /// </code>
    /// </remarks>
    /// <param name="app"></param>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app, Action<WeixinSiteOptions> setupAction = null)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (setupAction != null)
        {
            var optionsMonitor = app.ApplicationServices.GetRequiredService<IOptionsMonitor<WeixinSiteOptions>>();
            var options = optionsMonitor.CurrentValue;
            setupAction(options);
        }

        return app.UseMiddleware<WeixinSiteMiddleware>();
    }

    /// <summary>
    /// Adds the <see cref="WeixinSiteMiddleware"/> to the pipeline with the given path.
    /// </summary>
    /// <remarks>Usage:
    /// <code>
    /// app.UseWeixinSite("/wx");
    /// </code>
    /// </remarks>
    /// <param name="app"></param>
    /// <param name="path">The path.</param>
    /// <returns></returns>
    public static IApplicationBuilder UseWeixinSite(this IApplicationBuilder app, PathString path)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        if (string.IsNullOrEmpty(path.Value))
        {
            throw new ArgumentException("The path must be a non-empty string.", nameof(path));
        }
        else
        {
            var optionsMonitor = app.ApplicationServices.GetRequiredService<IOptionsMonitor<WeixinSiteOptions>>();
            var options = optionsMonitor.CurrentValue;
            options.Path = path.Value;
        }

        return app.UseMiddleware<WeixinSiteMiddleware>();
    }
}
