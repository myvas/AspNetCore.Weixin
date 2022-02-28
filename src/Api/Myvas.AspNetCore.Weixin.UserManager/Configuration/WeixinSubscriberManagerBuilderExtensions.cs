using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;
using Myvas.AspNetCore.Weixin.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class WeixinSubscriberManagerBuilderExtensions
{
    public static IWeixinBuilder AddSubscriberManager(
        this IWeixinBuilder builder,
        Action<WeixinStoreOptions> storeOptionsAction = null)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.AddOperationalStore(storeOptionsAction);

        return builder;
    }

    public static IWeixinBuilder AddSubscriberManager<TContext>(
        this IWeixinBuilder builder,
        Action<WeixinSubscriberManagerOptions> setupAction = null,
        Action<WeixinStoreOptions> storeOptionsAction = null)
        where TContext : DbContext, IWeixinDbContext
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddOperationalDbContext<TContext>(storeOptionsAction);
        builder.AddOperationalStore<TContext>(storeOptionsAction);

        builder.Services.AddHttpClient<UserApi>();
        builder.Services.AddHttpClient<UserProfileApi>();
        builder.Services.AddTransient<IWeixinUserStore, WeixinUserStore>();
        builder.Services.AddTransient<ISubscribeEventReceivedEntryStore, SubscribeEventReceivedEntryStore>();
        builder.Services.AddTransient<IUnsubscribeEventReceivedEntryStore, UnsubscribeEventReceivedEntryStore>();
        builder.Services.AddTransient<IWeixinSubscriberManager, WeixinSubscriberManager>();

        return builder;
    }
}