using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Resources;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Site.Test
{
    public class WeixinSiteMiddlewareTests
    {
        public const string SafariUserAgent = "Mozilla/5.0 (Linux; U; Android 2.3.6; zh-cn; GT-S5660 Build/GINGERBREAD) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1";
        public const string MicroMessengerUserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_1 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Mobile/9B176 MicroMessenger/4.3.2";

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
            Assert.StartsWith("You are now trying to visit a verification URL", s);
        }


        [Fact]
        public async Task HttpPost_ShouldOk()
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
                            o.Events.OnTextMessageReceived = async (x) =>
                            {
                                var resp = new WeixinResponseBuilder<WeixinResponseText>(x.Context, x.Xml);
                                resp.ResponseEntity.Content = $"Your message had been received:{x.Xml.Content}";
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
            Assert.NotNull(s);
            Assert.NotEmpty(s);
            Debug.WriteLine(s);
            Assert.StartsWith("<xml><Content>Your message had been received", s);
        }

        [Fact]
        public async Task HttpPost_InvalidSignature_Should400()
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
            var textXml = TestFile.ReadTestFile("ReceivedMessages/text.xml");
            var response = await testClient.PostAsync(WeixinSiteOptionsDefaults.Path, new StringContent(textXml));
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
}
