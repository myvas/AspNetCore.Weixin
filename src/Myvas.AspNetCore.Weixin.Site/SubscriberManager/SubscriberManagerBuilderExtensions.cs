using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;
using Myvas.AspNetCore.Weixin.Models;
using System;

namespace Microsoft.Extensions.DependencyInjection;

public static class SubscriberManagerBuilderExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <param name="builder"></param>
    /// <param name="storeOptionsAction"></param>
    /// <returns></returns>
    public static IWeixinBuilder AddSubscriberManager<TContext>(this IWeixinBuilder builder,
        Action<WeixinStoreOptions> storeOptionsAction = null)
        where TContext : DbContext, IWeixinDbContext<Subscriber>
    {
        return builder.AddSubscriberManager<Subscriber, TContext>();
    }

    public static IWeixinBuilder AddSubscriberManager<TSubscriber, TContext>(
        this IWeixinBuilder builder,
        Action<WeixinStoreOptions> storeOptionsAction = null)
        where TSubscriber : Subscriber, new()
        where TContext : DbContext, IWeixinDbContext<TSubscriber>
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        //builder.Services.AddHttpClient<UserApi>();
        //builder.Services.AddHttpClient<UserProfileApi>();
        //builder.Services.AddTransient<ISubscriberStore<TSubscriber>, SubscriberStore<TSubscriber, TContext>>();
        //builder.Services.AddTransient<IReceivedEntryStore<EventReceivedEntry>, EventReceivedEntryStore>();
        //builder.Services.AddTransient<IReceivedEntryStore<MessageReceivedEntry>, MessageReceivedEntryStore>();
        builder.Services.TryAddScoped<WeixinSubscriberManager<TSubscriber>>();

        return builder;
    }
}