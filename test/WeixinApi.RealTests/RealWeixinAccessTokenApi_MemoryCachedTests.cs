using Microsoft.Extensions.DependencyInjection;

namespace Myvas.AspNetCore.Weixin.Api.RealTests;

public class RealWeixinAccessTokenApi_MemoryCachedTests : RealWeixinTestBase
{
    [Fact]
    public void GetToken_ShouldSuccess()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddWeixin(options =>
        {
            options.AppId = Configuration["Weixin:AppId"];
            options.AppSecret = Configuration["Weixin:AppSecret"];
        });
        // Implicitly .AddWeixinAccessTokenMemoryCacheProvider()
        var sp = services.BuildServiceProvider();

        var api = sp.GetRequiredService<IWeixinAccessTokenApi>();

        var result = api.GetToken();
        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.True(result.ExpiresIn > 0);

        if (result.ExpiresIn < 5)
        {
            result = api.GetToken(true);
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.True(result.ExpiresIn > 5);
        }

        var result2 = api.GetToken();
        Assert.NotNull(result);
        Assert.Equal(result.AccessToken, result2.AccessToken);
    }
}