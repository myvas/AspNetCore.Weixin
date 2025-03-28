﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EfCore.Tests;

public class WeixinEfCoreEventSinkTests
{
    public const string SafariUserAgent = "Mozilla/5.0 (Linux; U; Android 2.3.6; zh-cn; GT-S5660 Build/GINGERBREAD) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1";
    public const string MicroMessengerUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Mobile/9B176 MicroMessenger/4.3.2";

    private readonly TestServer testServer;

    public WeixinEfCoreEventSinkTests()
    {
        testServer = TestServerBuilder.CreateServer(app =>
        {
            app.UseWeixinSite();
        }, services =>
        {
            services.AddDbContext<WeixinDbContext>(o =>
            {
                o.UseInMemoryDatabase(databaseName: "WeixinTestDatabase" + Guid.NewGuid().ToString("N"));
            }, ServiceLifetime.Singleton);

            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "NOT_USED_APPSECRET";
            })
            .AddWeixinSite(o =>
            {
                o.WebsiteToken = "WEIXINSITETOKEN";
            })
            .AddMessageProtection()
            .AddWeixinEfCore<WeixinDbContext>();
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

    [Theory]
    [InlineData("uplink/msg/image.xml", "<PicUrl>https://mp.weixin.qq.com/fake.png</PicUrl>", "from-msg-image")]
    [InlineData("uplink/msg/link.xml", "<Url>https://mp.weixin.qq.com</Url>", "from-msg-link")]
    [InlineData("uplink/msg/location.xml", "<Description>Longitude: 113.358803, Latitude: 23.134521, Scale: 20, Label: Somewhere</Description>", "from-msg-location")]
    [InlineData("uplink/msg/short_video.xml", "<Content>OnShortVideoMessageReceived: MediaId: media_id, ThumbMediaId: thumb_media_id</Content>", "from-msg-short_video")]
    [InlineData("uplink/msg/text.xml", "<Content>OnTextMessageReceived: Content: content</Content>", "from-msg-text")]
    [InlineData("uplink/msg/video.xml", "<Content>OnVideoMessageReceived: MediaId: media_id, ThumbMediaId: thumb_media_id</Content>", "from-msg-video")]
    [InlineData("uplink/msg/voice-recognition.xml", "<Content>OnVoiceMessageReceived: Format: format, MediaId: media_id, Recognition: recognition</Content>", "from-msg-voice-recognition")]
    [InlineData("uplink/msg/voice.xml", "<Content>OnVoiceMessageReceived: Format: format, MediaId: media_id, Recognition: </Content>", "from-msg-voice")]
    public async Task HttpPost_WeixinMessages(string fileName, string result, string fromUserName)
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile(fileName);
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
        Assert.Contains(result, s);
        Assert.EndsWith("</xml>", s);

        // Assert db on server-side
        var db = testServer.Services.GetRequiredService<WeixinDbContext>();
        {
            var entity = await db.WeixinReceivedMessages.FirstOrDefaultAsync(x => x.FromUserName == fromUserName);
            Assert.NotNull(entity);
        }
    }

    [Theory]
    [InlineData("uplink/event/location.xml", "<Description>Longitude: 113.358803, Latitude: 23.134521, Precision: 119.385040</Description>", "from-event-location")]
    [InlineData("uplink/event/menu_click.xml", "<Content>OnClickMenuEventReceived: EventKey: EVENTKEY</Content>", "from-event-menu_click")]
    [InlineData("uplink/event/menu_view.xml", "<Content>OnViewMenuEventReceived: EventKey: www.qq.com</Content>", "from-event-menu_view")]
    [InlineData("uplink/event/scan.xml", "<Content>OnQrscanEventReceived: EventKey: SCENE_VALUE, Ticket: TICKET</Content>", "from-event-scan")]
    public async Task HttpPost_WeixinEvents(string fileName, string result, string fromUserName)
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile(fileName);
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
        Assert.Contains(result, s);
        Assert.EndsWith("</xml>", s);

        // Assert db on server-side
        var db = testServer.Services.GetRequiredService<WeixinDbContext>();
        {
            var entity = await db.WeixinReceivedEvents.FirstOrDefaultAsync(x => x.FromUserName == fromUserName);
            Assert.NotNull(entity);
        }
    }
    [Theory]
    [InlineData("uplink/event/subscribe.xml", "<Content>OnSubscribeEventReceived: EventKey: , Ticket: </Content>", "from-event-subscribe")]
    [InlineData("uplink/event/subscribe_qrscene.xml", "<Content>OnSubscribeEventReceived: EventKey: qrscene_123123, Ticket: TICKET</Content>", "from-event-subscribe-qrscene")]
    public async Task HttpPost_WeixinEvent_Subscribe(string fileName, string result, string fromUserName)
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile(fileName);
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
        Assert.Contains(result, s);
        Assert.EndsWith("</xml>", s);

        // Assert db on server-side
        var db = testServer.Services.GetRequiredService<WeixinDbContext>();
        {
            var entity = await db.WeixinReceivedEvents.FirstOrDefaultAsync(x => x.FromUserName == fromUserName);
            Assert.NotNull(entity);

            var subscriber = await db.WeixinSubscribers.FirstOrDefaultAsync(x => x.OpenId == fromUserName);
            Assert.NotNull(entity);
        }
    }
}
