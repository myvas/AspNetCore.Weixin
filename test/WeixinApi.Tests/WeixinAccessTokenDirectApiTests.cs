using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Api.Test;

public class WeixinAccessTokenDirectApiTests
{
    private readonly TestServer _server;
    public WeixinAccessTokenDirectApiTests()
    {
        _server = FakeServerBuilder.CreateTencentServer();
    }

    [Fact]
    public async Task GetTokenShouldSuccess()
    {
        var services = new ServiceCollection();
        services.Configure<WeixinOptions>(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var optionsAccessor = serviceProvider.GetRequiredService<IOptions<WeixinOptions>>();
        var api = new WeixinAccessTokenDirectApi(optionsAccessor); var json = await api.GetTokenAsync();

        Assert.True(json.Succeeded);
        Assert.Equal("ACCESS_TOKEN", json.AccessToken);
        Assert.Equal(7200, json.ExpiresIn);
    }

    [Fact]
    public async Task GetTokenShouldReturnInvalidAppId()
    {
        var services = new ServiceCollection();
        services.Configure<WeixinOptions>(o =>
        {
            o.AppId = "INVALID_APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var optionsAccessor = serviceProvider.GetRequiredService<IOptions<WeixinOptions>>();
        var api = new WeixinAccessTokenDirectApi(optionsAccessor);

        var ex = await Assert.ThrowsAsync<WeixinAccessTokenException>(() => api.GetTokenAsync());
        Assert.Equal(40013, ex.ErrorCode);
        Assert.StartsWith("invalid appid", ex.ErrorMessage);
    }
}
