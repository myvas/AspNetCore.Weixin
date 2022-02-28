using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class WeixinMessagingBuilderExtensions
{
    public static IWeixinBuilder AddResponseBuilder(
        this IWeixinBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddWeixinResponseBuilder();

        return builder;
    }
}
