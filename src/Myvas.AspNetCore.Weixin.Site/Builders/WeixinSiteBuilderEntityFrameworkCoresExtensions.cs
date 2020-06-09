using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class WeixinSiteEntityFrameworkCoresBuilderExtensions
    {
        public static WeixinSiteBuilder AddEntityFrameworkCores<TWeixinDbContext>(this WeixinSiteBuilder builder)
            where TWeixinDbContext : DbContext
        {
            AddStore(builder.Services, typeof(TWeixinDbContext), typeof(WeixinSubscriber<string>), typeof(string));
            return builder;
        }

        public static WeixinSiteBuilder AddEntityFrameworkCores<TWeixinDbContext, TKey>(this WeixinSiteBuilder builder)
            where TWeixinDbContext : DbContext
            where TKey : IEquatable<TKey>
        {
            AddStore(builder.Services, typeof(TWeixinDbContext), typeof(WeixinSubscriber<TKey>), typeof(TKey));
            return builder;
        }

        public static WeixinSiteBuilder AddEntityFrameworkCores<TWeixinDbContext, TWeixinSubscriber, TKey>(this WeixinSiteBuilder builder)
            where TWeixinDbContext : DbContext
            where TWeixinSubscriber : WeixinSubscriber<TKey>
            where TKey : IEquatable<TKey>
        {
            AddStore(builder.Services, typeof(TWeixinDbContext), typeof(TWeixinSubscriber), typeof(TKey));
            return builder;
        }


        private static void AddStore(IServiceCollection services, Type contextType, Type subscriberType, Type keyType)
        {
            Type subscriberStoreType = null;
            Type receivedMessageStoreType = null;
            Type receivedEventStoreType = null;
            Type responseMessageStoreType = null;
            Type sendMessageStoreType = null;

            var weixinDbContext = FindGenericBaseType(contextType, typeof(IWeixinDbContext<,>));
            if (weixinDbContext != null)
            {
                keyType = weixinDbContext.GenericTypeArguments[1];
                subscriberType = weixinDbContext.GenericTypeArguments[0];
            }
            else
            {
                // it cannot be known the keyType or subscriberType from contextType
                var trySubscriberType = FindGenericBaseType(subscriberType, typeof(WeixinSubscriber<>));
                if (trySubscriberType != null)
                {
                    keyType = trySubscriberType.GenericTypeArguments[0];
                }
                else
                {
                    throw new InvalidOperationException(Resources.NotWeixinSubscriber);
                    //if (keyType == null)
                    //{
                    //    keyType = typeof(string);
                    //}
                    //subscriberType = typeof(WeixinSubscriber<>).MakeGenericType(keyType);
                }
            }
            subscriberStoreType = typeof(WeixinSubscriberStore<,,>).MakeGenericType(subscriberType, keyType, contextType);
            receivedMessageStoreType = typeof(WeixinReceivedMessageStore<,>).MakeGenericType(typeof(WeixinReceivedMessage), contextType);
            receivedEventStoreType = typeof(WeixinReceivedEventStore<,>).MakeGenericType(typeof(WeixinReceivedEvent), contextType);
            responseMessageStoreType = typeof(WeixinResponseMessageStore<,>).MakeGenericType(typeof(WeixinResponseMessage), contextType);
            sendMessageStoreType = typeof(WeixinSendMessageStore<,>).MakeGenericType(typeof(WeixinSendMessage), contextType);

            services.TryAddScoped(typeof(IWeixinSubscriberStore<,>).MakeGenericType(subscriberType, keyType), subscriberStoreType);
            services.TryAddScoped(typeof(IWeixinReceivedMessageStore<>).MakeGenericType(typeof(WeixinReceivedMessage)), receivedMessageStoreType);
            services.TryAddScoped(typeof(IWeixinReceivedEventStore<>).MakeGenericType(typeof(WeixinReceivedEvent)), receivedEventStoreType);
            services.TryAddScoped(typeof(IWeixinResponseMessageStore<>).MakeGenericType(typeof(WeixinResponseMessage)), responseMessageStoreType);
            services.TryAddScoped(typeof(IWeixinSendMessageStore<>).MakeGenericType(typeof(WeixinSendMessage)), sendMessageStoreType);
            if (keyType == typeof(string))
            {
                services.TryAddScoped(typeof(IWeixinSubscriberStore), typeof(WeixinSubscriberStore<>).MakeGenericType(contextType));
                services.TryAddScoped(typeof(IWeixinReceivedMessageStore), typeof(WeixinReceivedMessageStore<>).MakeGenericType(contextType));
                services.TryAddScoped(typeof(IWeixinReceivedEventStore), typeof(WeixinReceivedEventStore<>).MakeGenericType(contextType));
                services.TryAddScoped(typeof(IWeixinResponseMessageStore), typeof(WeixinResponseMessageStore<>).MakeGenericType(contextType));
                services.TryAddScoped(typeof(IWeixinSendMessageStore), typeof(WeixinSendMessageStore<>).MakeGenericType(contextType));
            }
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
}
