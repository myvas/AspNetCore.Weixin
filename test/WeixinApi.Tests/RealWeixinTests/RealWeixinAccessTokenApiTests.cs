using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Myvas.AspNetCore.Weixin.Api.RealTests;

public class RealWeixinAccessTokenApiTests : RealWeixinServerBase
{
    [Fact]
    public void GetToken_Cached()
    {
        if (!EnableRealWeixinTests) return;

        IServiceCollection services = new ServiceCollection();
        services.AddWeixin(options =>
        {
            options.AppId = Configuration["Weixin:AppId"];
            options.AppSecret = Configuration["Weixin:AppSecret"];
        })
        // .AddWeixinAccessTokenMemoryCacheProvider() implicitly
        .AddAccessTokenRedisCacheProvider(options =>
        {
            options.Configuration = RedisConnectionString;
        });
        var sp = services.BuildServiceProvider();
        var api = sp.GetRequiredService<IWeixinAccessTokenApi>();
        var result = api.GetToken();
        Assert.NotNull(result);
        Debug.WriteLineIf(!result.Succeeded, result);
        Assert.True(result.Succeeded);
        Assert.True(result.ExpiresIn > 0);

        // Give me 5 seconds to complete this test
        if (result.ExpiresIn < 5)
        {
            result = api.GetToken(true);
            Assert.NotNull(result);
            Debug.WriteLineIf(!result.Succeeded, result);
            Assert.True(result.Succeeded);
            Assert.True(result.ExpiresIn > 5);
        }

        // Now, we should get the token from Redis cache.
        var result2 = api.GetToken();
        Assert.NotNull(result2);
        Debug.WriteLineIf(!result2.Succeeded, result2);
        Assert.Equal(result.AccessToken, result2.AccessToken);
    }
}