using Microsoft.Extensions.DependencyInjection;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinBuilderAccessTokenApiExtensions{

    public static WeixinBuilder AddWeixinAccessTokenApi(this WeixinBuilder builder)
    {
        builder.Services.AddSingleton<WeixinAccessTokenDirectApi>();
        builder.Services.AddTransient<IWeixinAccessTokenApi, WeixinAccessTokenApi>();
        return builder;
    }
}
