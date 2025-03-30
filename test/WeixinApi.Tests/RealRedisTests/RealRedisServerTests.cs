using System;
using StackExchange.Redis;

namespace Myvas.AspNetCore.Weixin.Api.RealTests;

public partial class RealRedisServerTests : RealRedisServerBase
{
    [Fact]
    public void RedisServer_ShouldWork()
    {
        if (!EnableRealRedisTests) return;

        // connect
        var connection = ConnectionMultiplexer.Connect(RedisConnectionString);
        Assert.NotNull(connection);
        Assert.True(connection.IsConnected);

        // get db
        var db = connection.GetDatabase();
        Assert.NotNull(db);

        var testKey = Guid.NewGuid().ToString();
        var testValue = Guid.NewGuid().ToString();

        // set string
        var resultSet = db.StringSet(testKey, testValue);
        Assert.True(resultSet);

        // get string
        var resultGet = db.StringGet(testKey);
        Assert.True(resultGet.HasValue);
        Assert.Equal(testValue, resultGet.ToString());

        // del key
        var resultDelete = db.KeyDelete(testKey);
        Assert.True(resultDelete);
    }
}