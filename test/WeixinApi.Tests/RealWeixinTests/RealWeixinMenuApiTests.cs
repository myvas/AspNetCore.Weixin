using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class RealWeixinMenuApiTests : RealWeixinServerBase
{
    /// <summary>
    /// Whether enable to test the Weixin menu on your real Weixin official account (optional, default is false)
    /// </summary>
    /// <remarks>
    /// <para>For usersecrets, or appsettings.json:
    /// The key is `Weixin:EnableRealWeixinMenuTests`.</para><para>
    /// For GitHub secrets, or environment variables:
    /// The key is `WEIXIN__ENABLEREALWEIXINMENUTESTS`.</para>
    /// </remarks>
    protected bool EnableRealWeixinMenuTests { get; }

    public RealWeixinMenuApiTests()
    {
        EnableRealWeixinMenuTests = EnableRealWeixinTests
            && Configuration.GetValue<bool>("Weixin:EnableRealWeixinMenuTests", false);
    }

    [Fact]
    public async Task GetCurrentMenuAsync()
    {
        if (!EnableRealWeixinMenuTests) return;

        IServiceCollection services = new ServiceCollection();
        services.AddWeixin(options =>
        {
            options.AppId = Configuration["Weixin:AppId"];
            options.AppSecret = Configuration["Weixin:AppSecret"];
        })
        .AddWeixinRedisCacheProvider(options =>
        {
            options.Configuration = RedisConnectionString;
        });
        var sp = services.BuildServiceProvider();
        var api = sp.GetRequiredService<IWeixinMenuApi>();
        var result = await api.GetCurrentMenuAsync();
        Assert.NotNull(result);
        Debug.WriteLineIf(!result.Succeeded, result);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task PublishMenuAsync()
    {
        if (!EnableRealWeixinMenuTests) return;

        IServiceCollection services = new ServiceCollection();
        services.AddWeixin(options =>
        {
            options.AppId = Configuration["Weixin:AppId"];
            options.AppSecret = Configuration["Weixin:AppSecret"];
        })
        .AddWeixinRedisCacheProvider(options =>
        {
            options.Configuration = RedisConnectionString;
        });
        var sp = services.BuildServiceProvider();
        var api = sp.GetRequiredService<IWeixinMenuApi>();
        var createJson = new WeixinMenuCreateJson()
            .AddButton(new WeixinMenuJson.Button.View()
            {
                Name = "Home",
                Url = "https://wx.myvas.com"
            })
            .AddButton(new WeixinMenuJson.Button.View()
            {
                Name = "Weixin Menu",
                Url = "https://wx.myvas.com/WeixinMenu"
            })
            .AddButton(new WeixinMenuJson.Button.View()
            {
                Name = "About",
                Url = "https://wx.myvas.com/Home/About"
            });
        var result = await api.PublishMenuAsync(createJson);
        Assert.NotNull(result);
        Debug.WriteLineIf(!result.Succeeded, result);
        Assert.True(result.Succeeded);
    }
}