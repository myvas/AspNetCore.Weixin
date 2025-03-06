using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http;

namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinApiBuilderExtensions
    {
        public static WeixinApiBuilder AddWeixinAccessTokenServices(this WeixinApiBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            //Here assert IOptions<WeixinApiOptions> had already injected!
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<WeixinApiOptions>, WeixinApiPostConfigureOptions<WeixinApiOptions>>());
            builder.Services.AddTransient<WeixinAccessTokenApi>();
            builder.Services.AddSingleton<IWeixinAccessToken, WeixinAccessTokenService>();
            return builder;
        }

        public static WeixinApiBuilder AddWeixinApiServices(this WeixinApiBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            //Here assert IOptions<WeixinApiOptions> had already injected!
            builder.Services.AddTransient<IWeixinCommonApi, WeixinCommonApi>();
            builder.Services.AddTransient<IWeixinMenuApi, WeixinMenuApi>();
            builder.Services.AddTransient<CardApiTicketApi>();
            builder.Services.AddTransient<CustomerSupportApi>();
            builder.Services.AddTransient<GroupMessageApi>();
            builder.Services.AddTransient<JsapiTicketApi>();
            builder.Services.AddTransient<MediaApi>();
            builder.Services.AddTransient<QrcodeApi>();
            builder.Services.AddTransient<UserApi>();
            builder.Services.AddTransient<GroupsApi>();
            builder.Services.AddTransient<UserProfileApi>();
            builder.Services.AddTransient<WifiApi>();
            return builder;
        }

        public static WeixinApiBuilder AddDefaultAccessTokenCacheProvider(this WeixinApiBuilder builder)
        {
            builder.Services.AddMemoryCache();
            builder.Services.Where(x => x.ImplementationType == typeof(IWeixinAccessTokenCacheProvider)).ToList()
                .ForEach(x => builder.Services.Remove(x));
            builder.Services.AddSingleton<IWeixinAccessTokenCacheProvider, WeixinAccessTokenMemoryCacheProvider>();
            return builder;
        }

        public static WeixinApiBuilder AddAccessTokenCacheProvider<TCacheProvider>(this WeixinApiBuilder builder)
            where TCacheProvider : class, IWeixinAccessTokenCacheProvider
        {
            builder.Services.Where(x => x.ImplementationType == typeof(IWeixinAccessTokenCacheProvider)).ToList()
                .ForEach(x => builder.Services.Remove(x));
            builder.Services.AddSingleton<IWeixinAccessTokenCacheProvider, TCacheProvider>();
            builder.Services.AddSingleton<TCacheProvider>();
            return builder;
        }
    }
}
