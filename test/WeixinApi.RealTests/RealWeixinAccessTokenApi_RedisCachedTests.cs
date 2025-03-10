using Microsoft.Extensions.DependencyInjection;

namespace Myvas.AspNetCore.Weixin.Api.RealTests;

public class RealWeixinAccessTokenApi_RedisCachedTests : RealWeixinTestBase
{
    [Fact]
    public void GetToken_ShouldSuccess()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddWeixin(options =>
        {
            options.AppId = Configuration["Weixin:AppId"];
            options.AppSecret = Configuration["Weixin:AppSecret"];
        })
        .AddAccessTokenRedisCacheProvider(options =>
        {
            options.Configuration = Configuration["Weixin:RedisConnection"];
        });
        var sp = services.BuildServiceProvider();

        // Get the service we need.
        var api = sp.GetRequiredService<IWeixinAccessTokenApi>();

        // We want a new token from Tencent? No.
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

        // This time, we should get cached value from Redis.
        var result2 = api.GetToken();
        Assert.Equal(result.AccessToken, result2.AccessToken);
    }
}
