using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EfCore;
using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
/// </summary>
public static class WeixinSiteBuilderEfCoreExtensions
{
    public static WeixinSiteBuilder AddWeixinEfCore<TWeixinDbContext>(this WeixinSiteBuilder builder, Action<WeixinSiteEfCoreOptions> setupAction = null)
        where TWeixinDbContext : DbContext
    {
        AddWeixinEfCore(builder.Services, typeof(TWeixinDbContext), typeof(WeixinSubscriberEntity), typeof(string));

        builder.Services.TryAddScoped<WeixinSubscriberSyncService<WeixinSubscriberEntity, string>>();
        builder.Services.AddHostedService<WeixinSubscriberSyncHostedService<WeixinSubscriberEntity, string>>();
        return builder;
    }

    public static WeixinSiteBuilder AddWeixinEfCore<TWeixinDbContext, TWeixinSubscriberEntity>(this WeixinSiteBuilder builder, Action<WeixinSiteEfCoreOptions> setupAction = null)
        where TWeixinDbContext : DbContext
        where TWeixinSubscriberEntity : class, IWeixinSubscriber<string>, IEntity, new()
    {
        AddWeixinEfCore(builder.Services, typeof(TWeixinDbContext), typeof(TWeixinSubscriberEntity), typeof(string));

        builder.Services.TryAddScoped<WeixinSubscriberSyncService<TWeixinSubscriberEntity, string>>();
        builder.Services.AddHostedService<WeixinSubscriberSyncHostedService<TWeixinSubscriberEntity, string>>();
        return builder;
    }

    public static WeixinSiteBuilder AddWeixinEfCore<TWeixinDbContext, TWeixinSubscriberEntity, TKey>(this WeixinSiteBuilder builder, Action<WeixinSiteEfCoreOptions> setupAction = null)
        where TWeixinDbContext : DbContext
        where TWeixinSubscriberEntity : class, IWeixinSubscriber<TKey>, IEntity, new()
        where TKey : IEquatable<TKey>
    {
        AddWeixinEfCore(builder.Services, typeof(TWeixinDbContext), typeof(TWeixinSubscriberEntity), typeof(TKey));

        builder.Services.TryAddScoped<WeixinSubscriberSyncService<TWeixinSubscriberEntity, TKey>>();
        builder.Services.AddHostedService<WeixinSubscriberSyncHostedService<TWeixinSubscriberEntity, TKey>>();
        return builder;
    }

    private static void AddWeixinEfCore(IServiceCollection services, Type contextType, Type subscriberType, Type keyType)
    {
        Type subscriberStoreType = null;
        Type receivedMessageStoreType = null;
        Type receivedEventStoreType = null;
        Type responseMessageStoreType = null;
        Type sendMessageStoreType = null;

        var tryWeixinDbContext2Type = FindGenericBaseType(contextType, typeof(IWeixinDbContext<,>));
        if (tryWeixinDbContext2Type != null)
        {
            // IWeixinDbContext<TWeixinSubscriber, TKey>
            keyType = tryWeixinDbContext2Type.GenericTypeArguments[1];
            subscriberType = tryWeixinDbContext2Type.GenericTypeArguments[0];
            services.TryAddScoped(typeof(IWeixinDbContext<,>).MakeGenericType(subscriberType, keyType), contextType);
        }
        else
        {
            // IWeixinDbContext<TWeixinSubscriber>
            var tryWeixinDbContext1Type = FindGenericBaseType(contextType, typeof(IWeixinDbContext<>));
            if (tryWeixinDbContext1Type != null)
            {
                subscriberType = tryWeixinDbContext1Type.GenericTypeArguments[0];
                services.TryAddScoped(typeof(IWeixinDbContext<>).MakeGenericType(subscriberType), contextType);
            }
            else
            {
                services.TryAddScoped(typeof(IWeixinDbContext), contextType);
            }

            var trySubscriberType1Type = FindGenericBaseType(subscriberType, typeof(IWeixinSubscriber<>));
            if (trySubscriberType1Type != null)
            {
                // IWeixinSubscriber<TKey>
                keyType = trySubscriberType1Type.GenericTypeArguments[0];
            }
            else
            {
                // Set default key type
                keyType = typeof(string);
            }
        }

        subscriberStoreType = typeof(WeixinSubscriberStore<,,>).MakeGenericType(subscriberType, keyType, contextType);
        receivedMessageStoreType = typeof(WeixinReceivedMessageStore<,>).MakeGenericType(typeof(WeixinReceivedMessageEntity), contextType);
        receivedEventStoreType = typeof(WeixinReceivedEventStore<,>).MakeGenericType(typeof(WeixinReceivedEventEntity), contextType);
        responseMessageStoreType = typeof(WeixinResponseMessageStore<,>).MakeGenericType(typeof(WeixinResponseMessageEntity), contextType);
        sendMessageStoreType = typeof(WeixinSendMessageStore<,>).MakeGenericType(typeof(WeixinSendMessageEntity), contextType);

        services.TryAddScoped(typeof(IWeixinSubscriberStore<,>).MakeGenericType(subscriberType, keyType), subscriberStoreType);
        services.TryAddScoped(typeof(IWeixinReceivedMessageStore<>).MakeGenericType(typeof(WeixinReceivedMessageEntity)), receivedMessageStoreType);
        services.TryAddScoped(typeof(IWeixinReceivedEventStore<>).MakeGenericType(typeof(WeixinReceivedEventEntity)), receivedEventStoreType);
        services.TryAddScoped(typeof(IWeixinResponseMessageStore<>).MakeGenericType(typeof(WeixinResponseMessageEntity)), responseMessageStoreType);
        services.TryAddScoped(typeof(IWeixinSendMessageStore<>).MakeGenericType(typeof(WeixinSendMessageEntity)), sendMessageStoreType);
        if (keyType == typeof(string))
        {
            services.TryAddScoped(typeof(IWeixinSubscriberStore<>).MakeGenericType(subscriberType), subscriberStoreType);
            if (subscriberType == typeof(WeixinSubscriberEntity))
            {
                services.TryAddScoped(typeof(IWeixinSubscriberStore), subscriberStoreType);
            }
        }
        services.TryAddScoped(typeof(IWeixinReceivedMessageStore), receivedMessageStoreType);
        services.TryAddScoped(typeof(IWeixinReceivedEventStore), receivedEventStoreType);
        services.TryAddScoped(typeof(IWeixinResponseMessageStore), responseMessageStoreType);
        services.TryAddScoped(typeof(IWeixinSendMessageStore), sendMessageStoreType);

        // Add event sink
        services.Where(x => x.ServiceType == typeof(IWeixinEventSink)).ToList()
            .ForEach(x => services.Remove(x));
        var eventSinkImplType = typeof(WeixinEfCoreEventSink<,>).MakeGenericType(subscriberType, keyType);
        services.TryAddTransient(typeof(IWeixinEventSink), eventSinkImplType);
    }

    private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
    {
        var type = currentType;
        while (type != null)
        {
            var typeInfo = type.GetTypeInfo();
            var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
            if (genericType != null && genericType == genericBaseType)
            {
                return typeInfo;
            }
            type = type.BaseType;
        }
        return null;
    }
}
