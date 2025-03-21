using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EfCore.Tests.HostedServiceTests;

public class WeixinSubscriberSyncHostedServiceTests
{
    public const string MicroMessengerUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Mobile/9B176 MicroMessenger/4.3.2";

    private readonly TestServer fakeTencentServer;
    private readonly TestServer testServer;

    public WeixinSubscriberSyncHostedServiceTests()
    {
        fakeTencentServer = TestServerBuilder.CreateServer(null, null, async context =>
        {
            var req = context.Request;
            switch (req.Path.Value)
            {
                case "/cgi-bin/user/get":
                    {
                        var next_openids = req.Query["next_openid"];
                        Debug.WriteLine($"/cgi-bin/user/get: {next_openids}");
                        if (next_openids.Count < 1) next_openids = new[] { "" };
                        var next_openid = next_openids[0];
                        switch (next_openid)
                        {
                            case "OPENID2":
                                {
                                    context.Response.ContentType = "application/json";
                                    var content = TestFile.ReadTestFile("userapi/user-get-OPENID2.json");
                                    await context.Response.WriteAsync(content);
                                    return true;
                                }
                            case "OPENID3":
                                {
                                    context.Response.ContentType = "application/json";
                                    var content = TestFile.ReadTestFile("userapi/user-get-OPENID3.json");
                                    await context.Response.WriteAsync(content);
                                    return true;
                                }
                            default:
                                {
                                    context.Response.ContentType = "application/json";
                                    var content = TestFile.ReadTestFile("userapi/user-get.json");
                                    await context.Response.WriteAsync(content);
                                    return true;
                                }
                        }
                    }
                case "/cgi-bin/user/info":
                    {
                        var openids = req.Query["openid"];
                        Debug.WriteLine($"/cgi-bin/user/info: {openids}");
                        if (openids.Count > 0)
                        {
                            var openid = openids[0];
                            if (openid != null)
                            {
                                context.Response.ContentType = "application/json";
                                var content = TestFile.ReadTestFile("userapi/user-info-" + openid + ".json");
                                await context.Response.WriteAsync(content);
                                return true;
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            var content = "400 BAD REQUEST";
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        return true;
                    }
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
                        using var reader = new StreamReader(req.Body, Encoding.UTF8, leaveOpen: true);
                        var s = await reader.ReadToEndAsync();
                        if (!s.Contains(@"""appid"":""APPID"""))
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadTestFile("AccessToken/invalid_appid.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        else
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadTestFile("AccessToken/ok.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                    }
                default:
                    throw new NotImplementedException();
            }
        });

        testServer = TestServerBuilder.CreateServer(app =>
        {
            app.UseWeixinSite();
        }, services =>
        {
            services.AddDbContext<CustomizedSubscriberDbContext>(o =>
            {
                o.UseInMemoryDatabase(databaseName: "SyncHostedTestDb" + Guid.NewGuid().ToString("N"));
            }, ServiceLifetime.Singleton); // Singleton this InMemory db instance because sync service is running in background

            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = fakeTencentServer.CreateClient();
            })
            .AddWeixinSite(o =>
            {
                o.WebsiteToken = "WEIXINSITETOKEN";
            })
            .AddMessageProtection()
            .AddWeixinEfCore<CustomizedSubscriberDbContext>(o =>
            {
                // Autostart a hosted service to pull subscribers
                o.EnableSyncForWeixinSubscribers = true;
            });
        }, async context =>
        {
            var req = context.Request;
            if (req.Path.Value != WeixinSiteOptionsDefaults.Path)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                var content = "404 NOT FOUND";
                await context.Response.WriteAsync(content);
                return true;
            }
            return false;
        });
    }

    [Fact]
    public async Task SubscribeAndWaitForSync()
    {
        var filename = "uplink/event/subscribe_qrscene.xml";
        var result = "OnSubscribeEventReceived: EventKey: qrscene_123123, Ticket: TICKET";
        var fromUserName = "from-event-subscribe-qrscene";

        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile(filename);
        var timestamp = DateTime.Now.Ticks.ToString();
        var nonce = "nonce";
        var signature = SignatureHelper.CalculateSignature(timestamp, nonce, "WEIXINSITETOKEN");
        var query = new QueryBuilder
        {
            { "signature", signature },
            { "timestamp", timestamp },
            { "nonce", nonce }
        };
        var uri = WeixinSiteOptionsDefaults.Path + query.ToString();
        testClient.DefaultRequestHeaders.Add("User-Agent", MicroMessengerUserAgent);

        var response = await testClient.PostAsync(uri, new StringContent(textXml));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Assert message on client-side
        var s = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("<xml>", s);
        Assert.Contains($"<Content>{result}</Content>", s);
        Assert.EndsWith("</xml>", s);

        // Assert db on server-side
        var db = testServer.Services.GetRequiredService<CustomizedSubscriberDbContext>();
        {
            var entity = await db.WeixinReceivedEvents.FirstOrDefaultAsync(x => x.FromUserName == fromUserName);
            Assert.NotNull(entity);

            var subscriber = await db.WeixinSubscribers.FirstOrDefaultAsync(x => x.OpenId == fromUserName);
            Assert.True(subscriber.Subscribed);
        }

        Debug.WriteLine("Begin to wait for Sync in 5 seconds...");
        await WaitingAsync(TimeSpan.FromSeconds(5), () =>
        {
            Debug.WriteLine(".");
        });

        {
            var syncSubscriber = await db.WeixinSubscribers.FirstOrDefaultAsync(x => x.OpenId == "OPENID1");
            Assert.True(syncSubscriber.Subscribed);

            var syncSubscriber2 = await db.WeixinSubscribers.FirstOrDefaultAsync(x => x.OpenId == "OPENID2");
            Assert.False(syncSubscriber2.Subscribed);
        }
    }

    private static async Task WaitingAsync(TimeSpan delay, Action action)
    {
        // Start a timer for 2 minutes
        var delayTask = Task.Delay(delay);

        // Write a debug message every 1 second
        while (!delayTask.IsCompleted)
        {
            action();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        Debug.WriteLine($"{delay.TotalSeconds} seconds have passed.");
    }
}
