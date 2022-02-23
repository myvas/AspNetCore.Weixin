using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.WeixinSiteHost.Extensions;
using Serilog;
using Serilog.Events;

namespace Myvas.AspNetCore.WeixinSiteHost;

internal static class HostingExtensions
{
    internal static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages()
            .AddRazorRuntimeCompilation();

        builder.Services.AddControllers();

        builder.Services.AddSameSiteCookiePolicy();


        builder.Services.AddLocalApiAuthentication(principal =>
        {
            principal.Identities.First().AddClaim(new Claim("additional_claim", "additional_value"));

            return Task.FromResult(principal);
        });

        return builder.Build();
    }

    internal static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging(
            options => options.GetLevel = (httpContext, elapsed, ex) => LogEventLevel.Debug);

        app.UseCookiePolicy();

        app.UseDeveloperExceptionPage();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        // local API endpoints
        app.MapControllers()
            .RequireAuthorization(IdentityServerConstants.LocalApi.PolicyName);

        // UI
        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }

}
