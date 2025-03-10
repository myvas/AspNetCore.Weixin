using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.Weixin.Api.Tests.TestServers;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class WeixinAccessTokenApiTests
{
    private readonly TestServer _server;
    public WeixinAccessTokenApiTests()
    {
        _server = FakeServerBuilder.CreateTencentServer();
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

        var ex = await Assert.ThrowsAsync<WeixinAccessTokenException>(() => api.GetTokenAsync());
        Assert.Equal(40013, ex.ErrorCode);
        Assert.StartsWith("invalid appid", ex.ErrorMessage);
    }
}
