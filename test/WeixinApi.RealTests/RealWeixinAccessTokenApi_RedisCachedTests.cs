using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin;
using StackExchange.Redis;
using System.Threading.Tasks;
using Xunit;

namespace WeixinApi.RealTests;

public class RealWeixinAccessTokenApi_RedisCachedTests : RealWeixinRedisCacheTestBase
{
    [Fact]
    public void RedisServer_ShouldWork()
    {
        var redisConfig = Configuration["Weixin:RedisConnection"];
        var connection = ConnectionMultiplexer.Connect(redisConfig);
        Assert.NotNull(connection);
        Assert.True(connection.IsConnected);

        var db = connection.GetDatabase();
        Assert.NotNull(db);

        var testKey = Guid.NewGuid().ToString();
        var testValue = Guid.NewGuid().ToString();

        var resultSet = db.StringSet(testKey, testValue);
        Assert.True(resultSet);

        var resultGet = db.StringGet(testKey);
        Assert.False(resultGet.IsInteger);
        Assert.True(resultGet.HasValue);
        Assert.Equal(testValue, resultGet.ToString());

        var resultDelete = db.KeyDelete(testKey);
        Assert.True(resultDelete);
    }

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
