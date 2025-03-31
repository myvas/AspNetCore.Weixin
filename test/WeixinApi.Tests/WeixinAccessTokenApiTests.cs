using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class WeixinAccessTokenApiTests
{
    private readonly TestServer _server;
    public WeixinAccessTokenApiTests()
    {
        _server = FakeTencentServerBuilder.CreateTencentServer();
    }

    [Fact]
    public async Task GetTokenShouldSuccess()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();

        var api = serviceProvider.GetRequiredService<IWeixinAccessTokenApi>();
        var json = await api.GetTokenAsync();

        Assert.Equal("ACCESS_TOKEN", json.AccessToken);
    }

    [Fact]
    public async Task GetTokenShouldReturnInvalidAppId()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "INVALID_APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();

        var api = serviceProvider.GetRequiredService<IWeixinAccessTokenApi>();

        var result = await api.GetTokenAsync();
        Assert.NotNull(result);
        Assert.Equal(40013, result.ErrorCode);
        Assert.StartsWith("invalid appid", result.ErrorMessage);
    }
}
