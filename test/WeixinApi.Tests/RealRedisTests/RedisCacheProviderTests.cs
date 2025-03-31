using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class RealRedisCacheProviderTests : RealRedisServerBase
{
    TestServer FakeTencentServer;
    public RealRedisCacheProviderTests()
    {
        // Build a fake tencent server to generate access token
        FakeTencentServer = TestServerBuilder.CreateServer(null, null, async context =>
        {
            var req = context.Request;
            switch (req.Path.Value)
            {
                case "/cgi-bin/token":
                    throw new NotImplementedException();
                case "/cgi-bin/stable_token":
                    {
#if NET5_0_OR_GREATER
                        if (!req.HasJsonContentType())
                        {
                            context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("");
                            return true;
                        }
#else
                        if (req.ContentType == null || !req.ContentType!.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                            await context.Response.WriteAsync("");
                            return true;
                        }
#endif
                        var accessToken = new WeixinAccessTokenJson
                        {
                            ExpiresIn = 7200,
                            AccessToken = Guid.NewGuid().ToString("N")
                        };
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = JsonSerializer.Serialize(accessToken);
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                    }
                default:
                    throw new NotImplementedException();
            }
        });
    }

    [Fact]
    public void AddAccessTokenRedisCacheProvider()
    {
        if (!EnableRealRedisTests) return;

        IServiceCollection services = new ServiceCollection();
        services.AddWeixin(options =>
        {
            options.AppId = "FAKE_APPID";
            options.AppSecret = "FAKE_APPSECRET";
            options.Backchannel = FakeTencentServer.CreateClient();
        })
        .AddWeixinRedisCacheProvider(options =>
        {
            options.Configuration = RedisConnectionString;
        });
        var sp = services.BuildServiceProvider();
        var api = sp.GetRequiredService<IWeixinAccessTokenApi>();

        var result = api.GetToken();
        Assert.NotNull(result);
        Debug.WriteLineIf(!result.Succeeded, result);
        Assert.True(result.Succeeded);

        // Give me 5 seconds to complete this test
        if (result.ExpiresIn < 5)
        {
            result = api.GetToken(true);
            Assert.NotNull(result);
            Debug.WriteLineIf(!result.Succeeded, result);
            Assert.True(result.Succeeded);
            Assert.True(result.ExpiresIn > 100);
        }

        // Now, we should get the token from Redis cache.
        var result2 = api.GetToken();
        Debug.WriteLineIf(!result2.Succeeded, result2);
        Assert.Equal(result.AccessToken, result2.AccessToken);
    }
}