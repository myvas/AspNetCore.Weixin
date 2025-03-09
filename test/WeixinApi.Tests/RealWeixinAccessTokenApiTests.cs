using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Api.Test;
public class RealWeixinAccessTokenApiTests
{
    [Fact]
    public async Task GetTokenShouldReturnInvalidAppId()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinAccessTokenApi>();

        var ex = await Assert.ThrowsAsync<WeixinAccessTokenException>(() => api.GetTokenAsync());
        Assert.Equal(40013, ex.ErrorCode);
        Assert.StartsWith("invalid appid", ex.ErrorMessage);
    }
}
