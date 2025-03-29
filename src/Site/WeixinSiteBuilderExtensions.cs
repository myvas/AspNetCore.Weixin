using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Extension methods for setting up <see cref="WeixinSiteBuilder"/>.
/// </summary>
public static class WeixinSiteBuilderExtensions
{
    public static WeixinSiteBuilder AddWeixinEventSink<TWeixinEventSink>(this WeixinSiteBuilder builder)
        where TWeixinEventSink : class, IWeixinEventSink
    {
        builder.Services.Replace(ServiceDescriptor.Scoped<IWeixinEventSink, TWeixinEventSink>());
        return builder;
    }
}
