using Microsoft.Extensions.DependencyInjection;
using System;

namespace Myvas.AspNetCore.Weixin;
public static class WeixinBuilderExtensions
{
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
