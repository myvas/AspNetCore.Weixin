using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Site.Test
{
    public class WeixinSiteMiddlewareDebugTests
    {
        [Fact]
        public async Task HttpGet_ShouldSuccess()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddWeixinSite(o =>
                        {
                            o.AppId = "APPID";
                            o.AppSecret = "APPSECRET";
                            o.WebsiteToken = "WEIXINSITETOKEN";
                        });
                    })
                    .Configure(app =>
                    {
                        app.UseMiddleware<WeixinSiteMiddleware>();
                    });
                })
                .StartAsync();

            var testServer = host.GetTestServer();
            var testClient = testServer.CreateClient();
            var response = await testClient.GetAsync(WeixinSiteOptionsDefaults.Path);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var s = await response.Content.ReadAsStringAsync();
            Assert.NotNull(s);
            Assert.NotEmpty(s);
            Debug.WriteLine(s);
            Assert.Contains("You are now trying to visit a verification URL for a WeChat Official Account server.  You can fill this URL in the “Development/Basic Configuration/Server Configuration/Server Address (URL)” field in the WeChat Official Account backend (https://mp.weixin.qq.com).", s);
        }

        [Fact]
        public async Task MessageText_ShouldSuccess()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddWeixinSite(o =>
                        {
                            o.AppId = "APPID";
                            o.AppSecret = "APPSECRET";
                            o.WebsiteToken = "WEIXINSITETOKEN";
                            o.Debug = true; // Important!
                            o.Events.OnTextMessageReceived = async (x) =>
                            {
                                var resp = new WeixinResponseBuilder<WeixinResponseText>(x.Context, x.Xml);
                                resp.ResponseEntity.Content = $"您的消息已收到:{x.Xml.Content}";
                                await resp.FlushAsync();
                                return true;
                            };
                        });
                    })
                    .Configure(app =>
                    {
                        app.UseMiddleware<WeixinSiteMiddleware>();
                    });
                })
                .StartAsync();

            var testServer = host.GetTestServer();
            var testClient = testServer.CreateClient();
            var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
            var response = await testClient.PostAsync(WeixinSiteOptionsDefaults.Path, new StringContent(textXml));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var s = await response.Content.ReadAsStringAsync();
            Assert.NotNull(s);
            Assert.NotEmpty(s);
            Debug.WriteLine(s);
            Assert.Contains("您的消息已收到", s);
        }

        [Fact]
        public async Task HttpPost_DebugAndNotValidSignature_ShouldSuccess()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddWeixinSite(o =>
                        {
                            o.AppId = "APPID";
                            o.AppSecret = "APPSECRET";
                            o.Debug = true;  // Important!
                            o.WebsiteToken = "WEIXINSITETOKEN";
                        });
                    })
                    .Configure(app =>
                    {
                        app.UseMiddleware<WeixinSiteMiddleware>();
                    });
                })
                .StartAsync();

            var testServer = host.GetTestServer();
            var testClient = testServer.CreateClient();
            var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
            var response = await testClient.PostAsync(WeixinSiteOptionsDefaults.Path, new StringContent(textXml));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var s = await response.Content.ReadAsStringAsync();
            Assert.NotNull(s);
            Assert.NotEmpty(s);
            Debug.WriteLine(s);
            Assert.Contains("We have received you message.", s);
        }

        [Fact]
        public async Task HttpPost_DebugAndValidSignature_ShouldSuccess()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddWeixinSite(o =>
                        {
                            o.AppId = "APPID";
                            o.AppSecret = "APPSECRET";
                            o.Debug = true;  // Important!
                            o.WebsiteToken = "WEIXINSITETOKEN";
                        });
                    })
                    .Configure(app =>
                    {
                        app.UseMiddleware<WeixinSiteMiddleware>();
                    });
                })
                .StartAsync();

            var testServer = host.GetTestServer();
            var testClient = testServer.CreateClient();
            var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
            var response = await testClient.PostAsync(WeixinSiteOptionsDefaults.Path, new StringContent(textXml));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var s = await response.Content.ReadAsStringAsync();
            Assert.NotNull(s);
            Assert.NotEmpty(s);
            Debug.WriteLine(s);
            Assert.Contains("We have received you message.", s);
        }
    }
}
