using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Api.Tests.TestServers;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class WeixinAccessTokenDirectApiTests
{
    private readonly TestServer _server;
    public WeixinAccessTokenDirectApiTests()
    {
        _server = FakeTencentServerBuilder.CreateTencentServer();
    }

    [Fact]
    public async Task GetToken_ShouldSuccess()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var sp = services.BuildServiceProvider();
        //Notice: We have removed IWeixinAccessTokenDirectApi from Dependency Injection (DI) because it fetches a new access token each time it is called.
        //var api = sp.GetRequiredService<IWeixinAccessTokenDirectApi>();
        var optionsAccessor = sp.GetRequiredService<IOptions<WeixinOptions>>();
        var api = new WeixinAccessTokenDirectApi(optionsAccessor);

        var result = await api.GetTokenAsync();
        Assert.True(result.Succeeded);
        Assert.Equal(7200, result.ExpiresIn);
        Assert.Equal("ACCESS_TOKEN", result.AccessToken);
    }

    [Fact]
    public async Task GetToken_ShouldReturnInvalidAppId()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "INVALID_APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var sp = services.BuildServiceProvider();
        //Notice: We have removed IWeixinAccessTokenDirectApi from Dependency Injection (DI) because it fetches a new access token each time it is called.
        //var api = sp.GetRequiredService<IWeixinAccessTokenDirectApi>();
        var optionsAccessor = sp.GetRequiredService<IOptions<WeixinOptions>>();
        var api = new WeixinAccessTokenDirectApi(optionsAccessor);

        var result=await api.GetTokenAsync();
        Assert.Equal(40013, result.ErrorCode);
        Assert.StartsWith("invalid appid", result.ErrorMessage);
    }
}
