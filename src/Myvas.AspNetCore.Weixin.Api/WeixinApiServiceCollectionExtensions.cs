using Myvas.AspNetCore.Weixin;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class WeixinApiServiceCollectionExtensions
	{
		public static IServiceCollection AddWeixinApi(this IServiceCollection services, Action<WeixinApiOptions> setupAction)
		{
			if (services == null)
			{
				throw new ArgumentNullException(nameof(services));
			}
			services.AddOptions();
            if (setupAction != null)
			{
				services.Configure(setupAction);
			}
			services.AddWeixinApiClients();
			return services;
		}

		private static IServiceCollection AddWeixinApiClients(this IServiceCollection services)
		{
			// 找出本Assembly中所有继承于ApiClient的类，并注入
			services.AddHttpClient<CustomerSupportApi>();
			services.AddHttpClient<GroupMessageApi>();
			services.AddHttpClient<MediaApi>();
			services.AddHttpClient<MenuApi>();
			services.AddHttpClient<QrCodeApi>();
			services.AddHttpClient<UserApi>();
			services.AddHttpClient<GroupsApi>();
			services.AddHttpClient<UserProfileApi>();
			services.AddHttpClient<WifiApi>();
			return services;
		}
	}
}
