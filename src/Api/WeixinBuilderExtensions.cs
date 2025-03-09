using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinBuilderExtensions
{
    public static WeixinBuilder AddWeixinAccessTokenApi(this WeixinBuilder builder)
    {
        builder.Services.AddSingleton<WeixinAccessTokenDirectApi>();
        builder.Services.AddTransient<IWeixinAccessTokenApi, WeixinAccessTokenApi>();
        return builder;
    }

    public static WeixinBuilder AddAccessTokenMemoryCacheProvider(this WeixinBuilder builder)
    {
        builder.Services.Where(x => x.ImplementationType == typeof(IWeixinAccessTokenCacheProvider)).ToList()
            .ForEach(x => builder.Services.Remove(x));

        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IWeixinAccessTokenCacheProvider, WeixinAccessTokenMemoryCacheProvider>();

        return builder;
    }

    public static WeixinBuilder AddAccessTokenCacheProvider<TCacheProvider>(this WeixinBuilder builder)
        where TCacheProvider : class, IWeixinAccessTokenCacheProvider
    {
        builder.Services.Where(x => x.ImplementationType == typeof(IWeixinAccessTokenCacheProvider)).ToList()
            .ForEach(x => builder.Services.Remove(x));

        builder.Services.AddSingleton<IWeixinAccessTokenCacheProvider, TCacheProvider>();
        //builder.Services.AddSingleton<TCacheProvider>();

        return builder;
    }

    public static WeixinBuilder AddWeixinJsapiTicketApi(this WeixinBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddSingleton<WeixinJsapiTicketDirectApi>();
        builder.Services.AddTransient<IWeixinJsapiTicketApi, WeixinJsapiTicketApi>();
        return builder;
    }

    public static WeixinBuilder AddJsapiTicketMemoryCacheProvider(this WeixinBuilder builder)
    {
        builder.Services.Where(x => x.ImplementationType == typeof(IWeixinJsapiTicketCacheProvider)).ToList()
            .ForEach(x => builder.Services.Remove(x));

        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<IWeixinJsapiTicketCacheProvider, WeixinJsapiTicketMemoryCacheProvider>();

        return builder;
    }

    public static WeixinBuilder AddJsapiTicketCacheProvider<TCacheProvider>(this WeixinBuilder builder)
        where TCacheProvider : class, IWeixinJsapiTicketCacheProvider
    {
        builder.Services.Where(x => x.ImplementationType == typeof(IWeixinJsapiTicketCacheProvider)).ToList()
            .ForEach(x => builder.Services.Remove(x));

        builder.Services.AddSingleton<IWeixinJsapiTicketCacheProvider, TCacheProvider>();
        //builder.Services.AddSingleton<TCacheProvider>();

        return builder;
    }
    
    public static WeixinBuilder AddBusinessApis(this WeixinBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddTransient<IWeixinCommonApi, WeixinCommonApi>();
        builder.Services.AddTransient<IWeixinMenuApi, WeixinMenuApi>();
        builder.Services.AddTransient<ICardApiTicketApi, CardApiTicketApi>();
        builder.Services.AddTransient<ICustomerSupportApi, CustomerSupportApi>();
        builder.Services.AddTransient<IGroupMessageApi, GroupMessageApi>();
        builder.Services.AddTransient<IMediaApi, MediaApi>();
        builder.Services.AddTransient<IQrcodeApi, QrcodeApi>();
        builder.Services.AddTransient<IUserApi, UserApi>();
        builder.Services.AddTransient<IGroupsApi, GroupsApi>();
        builder.Services.AddTransient<IUserProfileApi, UserProfileApi>();
        builder.Services.AddTransient<IWifiApi, WifiApi>();

        return builder;
    }
}
