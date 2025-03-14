using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.Site.Tests.TestServers;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Site.Tests;

public class WeixinSiteMiddlewareDebugTests
{
    public const string SafariUserAgent = "Mozilla/5.0 (Linux; U; Android 2.3.6; zh-cn; GT-S5660 Build/GINGERBREAD) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1";
    public const string MicroMessengerUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Mobile/9B176 MicroMessenger/4.3.2";

    private readonly TestServer testServer;

    public WeixinSiteMiddlewareDebugTests()
    {
        testServer = FakeServerBuilder.CreateTencentDebugServer();
    }

    [Fact]
    public async Task HttpPost_DebugAndNotValidSignature_ShouldSuccess()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
        var response = await testClient.PostAsync(WeixinSiteOptionsDefaults.Path, new StringContent(textXml));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var s = await response.Content.ReadAsStringAsync();
        Assert.NotNull(s);
        Assert.NotEmpty(s);
        Debug.WriteLine(s);
        Assert.StartsWith("<xml>", s);
        Assert.Contains("<Content>Your message had been received", s);
        Assert.EndsWith("</xml>", s);
    }

    [Fact]
    public async Task HttpPost_DebugAndValidSignature_ShouldSuccess()
    {
        var testClient = testServer.CreateClient();
        var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
        var response = await testClient.PostAsync(WeixinSiteOptionsDefaults.Path, new StringContent(textXml));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var s = await response.Content.ReadAsStringAsync();
        Assert.NotNull(s);
        Assert.NotEmpty(s);
        Debug.WriteLine(s);
        Assert.StartsWith("<xml>", s);
        Assert.Contains("<Content>Your message had been received", s);
        Assert.EndsWith("</xml>", s);
    }
}
