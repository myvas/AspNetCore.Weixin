using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class WeixinMemoryCacheProviderTests
{
    [Fact]
    public void Test()
    {
        var randomAppId = Guid.NewGuid().ToString("N");

        IServiceCollection services = new ServiceCollection();
        services.AddMemoryCache();
        services.AddSingleton<IWeixinCacheProvider, WeixinMemoryCacheProvider>();
        var sp = services.BuildServiceProvider();
        var api = sp.GetRequiredService<IWeixinCacheProvider>();

        var randomAccessToken = Guid.NewGuid().ToString("N");
        Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        var replaceResult = api.Replace(randomAppId, new WeixinAccessTokenJson { AccessToken = randomAccessToken, ExpiresIn = 15 });
        Assert.True(replaceResult);

        Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        var accessToken = api.Get<WeixinAccessTokenJson>(randomAppId);
        Assert.NotNull(accessToken);
        Debug.WriteLineIf(!accessToken.Succeeded, JsonSerializer.Serialize(accessToken));
        Assert.True(accessToken.ExpiresIn > 9);
        Assert.Equal(randomAccessToken, accessToken.AccessToken);

        for (int i = 0; i < 5; i++)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
            var t = api.Get<WeixinAccessTokenJson>(randomAppId);
            Assert.NotNull(t);
            Debug.WriteLineIf(!t.Succeeded, JsonSerializer.Serialize(t));
        }

        Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        var accessToken2 = api.Get<WeixinAccessTokenJson>(randomAppId);
        Assert.NotNull(accessToken2);
        Debug.WriteLineIf(!accessToken2.Succeeded, JsonSerializer.Serialize(accessToken2));
        Assert.True(accessToken2.ExpiresIn < 11);
        Assert.Equal(randomAccessToken, accessToken2.AccessToken);

        Thread.Sleep(TimeSpan.FromSeconds(10));

        Debug.WriteLine($"Time: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}");
        var accessToken3 = api.Get<WeixinAccessTokenJson>(randomAppId);
        Debug.WriteLineIf(accessToken3?.Succeeded ?? false, JsonSerializer.Serialize(accessToken3));
        Assert.False(accessToken3?.Succeeded ?? false);
    }
}
