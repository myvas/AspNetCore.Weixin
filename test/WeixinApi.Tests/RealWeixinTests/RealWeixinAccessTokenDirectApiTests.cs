using System.Diagnostics;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.Api.RealTests;

public class RealWeixinAccessTokenDirectApiTests : RealWeixinServerBase
{
    [Fact]
    public void GetToken_Direct()
    {
        if (!EnableRealWeixinTests) return;

        var services = new ServiceCollection();
        services.Configure<WeixinOptions>(options =>
        {
            options.AppId = Configuration["Weixin:AppId"];
            options.AppSecret = Configuration["Weixin:AppSecret"];
            options.Backchannel = new HttpClient();
        });
        services.AddTransient<IWeixinAccessTokenDirectApi, WeixinAccessTokenDirectApi>();
        var sp = services.BuildServiceProvider();
        var api = sp.GetRequiredService<IWeixinAccessTokenDirectApi>();
        var result = api.GetToken();

        Assert.NotNull(result);
        Debug.WriteLineIf(!(result?.Succeeded ?? false), result);
        Assert.True(result.Succeeded);
    }
}