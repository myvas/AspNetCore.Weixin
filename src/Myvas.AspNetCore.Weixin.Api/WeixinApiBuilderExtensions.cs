using Myvas.AspNetCore.Weixin;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WeixinApiBuilderExtensions
	{
		public static IWeixinBuilder AddWeixinApi(this IWeixinBuilder builder, Action<WeixinApiOptions> setupAction)
		{
			if (builder == null)
			{
				throw new ArgumentNullException(nameof(builder));
			}

			// 找出本Assembly中所有继承于ApiClient的类，并注入
			builder.Services.AddHttpClient<CustomerSupportApi>();
			builder.Services.AddHttpClient<GroupMessageApi>();
			builder.Services.AddHttpClient<MediaApi>();
			builder.Services.AddHttpClient<MenuApi>();
			builder.Services.AddHttpClient<QrCodeApi>();
			builder.Services.AddHttpClient<UserApi>();
			builder.Services.AddHttpClient<GroupsApi>();
			builder.Services.AddHttpClient<UserProfileApi>();
			builder.Services.AddHttpClient<WifiApi>();

			return builder;
		}
	}
}
