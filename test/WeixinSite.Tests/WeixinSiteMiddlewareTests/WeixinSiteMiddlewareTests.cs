using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.Weixin.Site.Tests.TestServers;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Site.Tests;

public class WeixinSiteMiddlewareTests
{
    public const string SafariUserAgent = "Mozilla/5.0 (Linux; U; Android 2.3.6; zh-cn; GT-S5660 Build/GINGERBREAD) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1";
    public const string MicroMessengerUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Mobile/9B176 MicroMessenger/4.3.2";
    private readonly TestServer testServer;

    public WeixinSiteMiddlewareTests()
    {
        testServer = TestServerBuilder.CreateServer(app =>
        {
            app.UseWeixinSite();
        }, services =>
        {
            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
            })
            .AddWeixinSite(o =>
            {
                o.WebsiteToken = "WEIXINSITETOKEN";
            })
            .AddMessageProtection();
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
    public async Task HttpGet_ShouldReturnPredefinedContent()
    {
        var testClient = testServer.CreateClient();
        var response = await testClient.GetAsync(WeixinSiteOptionsDefaults.Path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var s = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("You are now trying to visit a verification URL", s);
    }

    [Fact]
    public async Task HttpPost_InvalidSignature_Should400()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("uplink/msg/text.xml");
        // We intentionally do NOT prepare a query string with signature|timestamp|nonce here!
        var url = WeixinSiteOptionsDefaults.Path;
        testClient.DefaultRequestHeaders.Add("User-Agent", MicroMessengerUserAgent);

        var response = await testClient.PostAsync(url, new StringContent(textXml));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var s = await response.Content.ReadAsStringAsync();
        Assert.Contains("Failed on WeChat message signature verification!", s);
    }

    [Fact]
    public async Task HttpPost_NoUserAgentHeader_Should400()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("uplink/msg/text.xml");
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
        //We intentionally do NOT add User-Agent header here!
        //testClient.DefaultRequestHeaders.Remove("User-Agent");
        var response = await testClient.PostAsync(uri, new StringContent(textXml));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var s = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("Please access this page via the WeChat client", s);
    }

    [Fact]
    public async Task HttpPost_NotMicroMessenger_Should400()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("uplink/msg/text.xml");
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
        testClient.DefaultRequestHeaders.Add("User-Agent", SafariUserAgent);
        var response = await testClient.PostAsync(uri, new StringContent(textXml));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var s = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("Please access this page via the WeChat client", s);
    }

    [Theory]
    [InlineData("uplink/msg/image.xml", "OnImageMessageReceived: MediaId: media_id, PicUrl: https://mp.weixin.qq.com/fake.png")]
    [InlineData("uplink/msg/link.xml", "OnLinkMessageReceived: Url: https://mp.weixin.qq.com")]
    [InlineData("uplink/msg/location.xml", "OnLocationMessageReceived: Longitude: 113.358803, Latitude: 23.134521, Label: Somewhere")]
    [InlineData("uplink/msg/short_video.xml", "OnShortVideoMessageReceived: MediaId: media_id, ThumbMediaId: thumb_media_id")]
    [InlineData("uplink/msg/text.xml", "OnTextMessageReceived: Content: content")]
    [InlineData("uplink/msg/video.xml", "OnVideoMessageReceived: MediaId: media_id, ThumbMediaId: thumb_media_id")]
    [InlineData("uplink/msg/voice-recognition.xml", "OnVoiceMessageReceived: Format: format, MediaId: media_id, Recognition: recognition")]
    [InlineData("uplink/msg/voice.xml", "OnVoiceMessageReceived: Format: format, MediaId: media_id, Recognition: ")]
    [InlineData("uplink/event/location.xml", "OnLocationEventReceived: Longitude: 113.358803, Latitude: 23.134521, Precision: 119.385040")]
    [InlineData("uplink/event/menu_click.xml", "OnClickMenuEventReceived: EventKey: EVENTKEY")]
    [InlineData("uplink/event/menu_view.xml", "OnViewMenuEventReceived: EventKey: www.qq.com")]
    [InlineData("uplink/event/scan.xml", "OnQrscanEventReceived: EventKey: SCENE_VALUE, Ticket: TICKET")]
    [InlineData("uplink/event/subscribe.xml", "OnSubscribeEventReceived: EventKey: , Ticket: ")]
    [InlineData("uplink/event/subscribe_qrscene.xml", "OnSubscribeEventReceived: EventKey: qrscene_123123, Ticket: TICKET")]
    public async Task HttpPost_WeixinEvents(string fileName, string result)
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

        var s = await response.Content.ReadAsStringAsync();
        Assert.StartsWith("<xml>", s);
        Assert.Contains($"<Content>{result}</Content>", s);
        Assert.EndsWith("</xml>", s);
    }
}
