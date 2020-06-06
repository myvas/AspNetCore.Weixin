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
	public static class WeixinEntityFrameworBuilderExtensions
	{
		public static WeixinBuilder AddEntityFrameworkCores<TWeixinDbContext>(this WeixinBuilder builder)
		{
			AddStore(builder.Services, builder.SubscriberType, builder.ReceivedTextMessageType, typeof(TWeixinDbContext));
			return builder;
		}

		private static void AddStore(IServiceCollection services, Type subscriberType, Type receivedTextMessageType, Type contextType)
		{
			var foundSubscriberType = FindGenericBaseType(subscriberType, typeof(WeixinSubscriber));
			if (foundSubscriberType == null)
			{
				throw new InvalidOperationException($"{nameof(subscriberType)} is not a WeixinSubscriber type");
			}
			var keyType = foundSubscriberType.GenericTypeArguments[0];//ignore it!

			if (receivedTextMessageType == null)
			{
				// no XMessageType
				Type dbContextType = null;
				var weixinDbContext = FindGenericBaseType(contextType, typeof(WeixinDbContext<>));
				if (weixinDbContext == null)
				{
					dbContextType = typeof(SubscriberOnlyStore<,>).MakeGenericType(subscriberType, contextType);
				}
				else
				{
					dbContextType = typeof(SubscriberOnlyStore<,>).MakeGenericType(subscriberType, contextType,
						weixinDbContext.GenericTypeArguments[1],
						weixinDbContext.GenericTypeArguments[2]);
				}
				services.TryAddScoped(typeof(IWeixinSubscriberStore<>).MakeGenericType(subscriberType), dbContextType);
			}
			else
			{
				var foundMessageType = FindGenericBaseType(receivedTextMessageType, typeof(ReceivedTextMessage));
				if (foundMessageType == null)
				{
					throw new InvalidOperationException($"{nameof(receivedTextMessageType)} is not a ReceivedTextMessageType");
				}

				Type dbContextType = null;
				var weixinDbContext = FindGenericBaseType(contextType, typeof(WeixinDbContext<,>));
				if (weixinDbContext == null)
				{
					dbContextType = typeof(SubscriberOnlyStore<,>).MakeGenericType(foundSubscriberType, foundMessageType, contextType);
				}
				else
				{
					dbContextType = typeof(SubscriberOnlyStore<,>).MakeGenericType(subscriberType, foundMessageType, contextType,
						weixinDbContext.GenericTypeArguments[1],
						weixinDbContext.GenericTypeArguments[2],
						weixinDbContext.GenericTypeArguments[3]);
				}
				services.TryAddScoped(typeof(IWeixinSubscriberStore<>).MakeGenericType(subscriberType, receivedTextMessageType), dbContextType);
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
