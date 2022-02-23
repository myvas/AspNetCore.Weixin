using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer;

public static class AccessTokenServerBuilderConfigurationExtensions
{
    public static IAccessTokenServerBuilder AddAccessToken<TContext>(
        this IAccessTokenServerBuilder builder)
        where TContext : DbContext, IAccessTokenDbContext
    {
        builder.AddAccessToken<TContext>(o => { });
        return builder;
    }

    public static IAccessTokenServerBuilder AddAccessToken<TContext>(
        this IAccessTokenServerBuilder builder,
        Action<WeixinAccessTokenOptions> configure)
        where TContext : DbContext, IAccessTokenDbContext
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        builder.AddWeixin();
        builder.Services.Configure(configure);
        return builder;
    }

}
