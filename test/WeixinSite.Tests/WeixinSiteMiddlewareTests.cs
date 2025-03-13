using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Myvas.AspNetCore.Weixin.Site.Tests.TestServers;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Site.Tests;

public class WeixinSiteMiddlewareTests
{
    public const string SafariUserAgent = "Mozilla/5.0 (Linux; U; Android 2.3.6; zh-cn; GT-S5660 Build/GINGERBREAD) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1";
    public const string MicroMessengerUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Mobile/9B176 MicroMessenger/4.3.2";
    private readonly TestServer testServer;

    public WeixinSiteMiddlewareTests()
    {
        testServer = FakeServerBuilder.CreateTencentServer();
    }

    [Fact]
    public async Task HttpGet_ShouldReturnPredefinedContent()
    {
        var testClient = testServer.CreateClient();
        var response = await testClient.GetAsync(WeixinSiteOptionsDefaults.Path);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var s = await response.Content.ReadAsStringAsync();
        Assert.NotNull(s);
        Assert.NotEmpty(s);
        Debug.WriteLine(s);
        Assert.StartsWith("You are now trying to visit a verification URL", s);
    }

    [Fact]
    public async Task HttpPost_ShouldReturnWeixinXmlContent()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
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
        Assert.Contains("<Content>Your message had been received", s);
        Assert.EndsWith("</xml", s);
    }

    [Fact]
    public async Task HttpPost_InvalidSignature_Should400()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
        // We intentionally do NOT prepare a query string with signature|timestamp|nonce here!
        var url = WeixinSiteOptionsDefaults.Path;

        var response = await testClient.PostAsync(url, new StringContent(textXml));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var s = await response.Content.ReadAsStringAsync();
        Assert.NotNull(s);
        Assert.NotEmpty(s);
        Debug.WriteLine(s);
        Assert.Contains("Failed on WeChat message signature verification!", s);
    }

    [Fact]
    public async Task HttpPost_NoUserAgentHeader_Should400()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
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
        Assert.NotNull(s);
        Assert.NotEmpty(s);
        Debug.WriteLine(s);
        Assert.StartsWith("Please access this page via the WeChat client", s);
    }

    [Fact]
    public async Task HttpPost_NotMicroMessenger_Should400()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
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
        Assert.NotNull(s);
        Assert.NotEmpty(s);
        Debug.WriteLine(s);
        Assert.StartsWith("Please access this page via the WeChat client", s);
    }
}
