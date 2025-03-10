using StackExchange.Redis;

namespace Myvas.AspNetCore.Weixin.Api.RealTests;

public class RealRedisServerTests : RealWeixinTestBase
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
}
