using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class WeixinExpirationMemoryCacheProviderTests
{
    [Fact]
    public void Test()
    {
        var randomAppId = Guid.NewGuid().ToString("N");

        IServiceCollection services = new ServiceCollection();
        services.AddWeixinCore(o =>
        {
            o.AppId = randomAppId;
            o.AppSecret = "FAKE_INVALID_SECRET";
        })
        .AddWeixinCacheProvider<WeixinAccessTokenJson, WeixinExpirationMemoryCacheProvider<WeixinAccessTokenJson>>();
        var sp = services.BuildServiceProvider();
        var api = sp.GetRequiredService<IWeixinCacheProvider<WeixinAccessTokenJson>>();

        var randomAccessToken = Guid.NewGuid().ToString("N");
        var replaceResult = api.Replace(randomAppId, new WeixinAccessTokenJson { AccessToken = randomAccessToken, ExpiresIn = 15 });
        Assert.True(replaceResult);

        var accessToken = api.Get(randomAppId);
        Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        Debug.WriteLine(JsonSerializer.Serialize(accessToken));
        Assert.True(accessToken.ExpiresIn > 9);
        Assert.Equal(randomAccessToken, accessToken.AccessToken);

        for (int i = 0; i < 5; i++)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var t = api.Get(randomAppId);
            Debug.WriteLine(JsonSerializer.Serialize(t));
        }

        var accessToken2 = api.Get(randomAppId);
        Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        Debug.WriteLine(JsonSerializer.Serialize(accessToken2));
        Assert.True(accessToken2.ExpiresIn < 11);
        Assert.Equal(randomAccessToken, accessToken2.AccessToken);

        Thread.Sleep(TimeSpan.FromSeconds(10));

        var accessToken3 = api.Get(randomAppId);
        Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        Debug.WriteLine(JsonSerializer.Serialize(accessToken3));
        Assert.False(accessToken3?.Validate() ?? false);
    }
}
