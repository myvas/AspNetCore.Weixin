using Myvas.AspNetCore.Weixin;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class WeixinSiteEntityFrameworkCoresBuilderExtensions
    {
        public static WeixinSiteBuilder AddEntityFrameworkCores<TWeixinDbContext>(this WeixinSiteBuilder builder)
        {
            AddStore(builder.Services, builder.SubscriberType, typeof(TWeixinDbContext));
            return builder;
        }

        private static void AddStore(IServiceCollection services, Type subscriberType, Type contextType)
        {
            var foundSubscriberType = FindGenericBaseType(subscriberType, typeof(WeixinSubscriber<>));
            if (foundSubscriberType == null)
            {
                throw new InvalidOperationException(Resources.NotWeixinSubscriber);
            }

            var keyType = foundSubscriberType.GenericTypeArguments[0];

            Type subscriberStoreType = null;
            var weixinDbContext = FindGenericBaseType(contextType, typeof(WeixinDbContext<,>));
            if (weixinDbContext == null)
            {
                // If its a custom DbContext, we can only add the default POCOs
                subscriberStoreType = typeof(WeixinSubscriberStore<,,>).MakeGenericType(subscriberType, keyType, contextType);
            }
            else
            {
                subscriberStoreType = typeof(WeixinSubscriberStore<,,>).MakeGenericType(subscriberType, keyType, contextType);
            }
            services.TryAddScoped(typeof(IWeixinSubscriberStore<,>).MakeGenericType(subscriberType, keyType), subscriberStoreType);
            services.TryAddScoped(typeof(IWeixinReceivedMessageStore<>).MakeGenericType(typeof(WeixinReceivedMessage)), typeof(WeixinReceivedMessageStore<WeixinReceivedMessage>));
            services.TryAddScoped(typeof(IWeixinReceivedEventStore<>).MakeGenericType(typeof(WeixinReceivedEvent)), typeof(WeixinReceivedEventStore<WeixinReceivedEvent>));
            services.TryAddScoped(typeof(IWeixinResponseMessageStore<>).MakeGenericType(typeof(WeixinResponseMessage)), typeof(WeixinResponseMessageStore<WeixinResponseMessage>));
            services.TryAddScoped(typeof(IWeixinSendMessageStore<>).MakeGenericType(typeof(WeixinSendMessage)), typeof(WeixinSendMessageStore<WeixinSendMessage>));
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
