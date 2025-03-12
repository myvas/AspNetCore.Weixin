using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Myvas.AspNetCore.Weixin.Site.Tests.TestServers;

public static class FakeServerBuilder
{
    public static TestServer CreateTencentServer()
    {
        return TestServerBuilder.CreateServer(app =>
        {
            app.UseMiddleware<WeixinSiteMiddleware>();
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
                o.Events.OnTextMessageReceived = async (x) =>
                {
                    var resp = new WeixinResponseBuilder<WeixinResponseText>(x.Context, x.Xml);
                    resp.ResponseEntity.Content = $"Your message had been received:{x.Xml.Content}";
                    await resp.FlushAsync();
                    return true;
                };
            })
            .AddWeixinMessageEncryptor();
        },
        async context =>
        {
            var req = context.Request;
            if (!req.Path.Value.StartsWith(WeixinSiteOptionsDefaults.Path))
            {

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var content = "404 NOT FOUND";
                await context.Response.WriteAsync(content);
                return true;
            }

            if (req.Method == "GET")
            {
                var content = "You are now trying to visit a verification URL for Weixin Site.";
                await context.Response.WriteAsync(content);
                return true;
            }

            if (req.Method != "POST")
            {
                return false;
            }

            if (!req.Query.ContainsKey("signature"))
            {
                var content = TestFile.ReadTestFile("Responses/no-signature.xml");
                await context.Response.WriteAsync(content);
                return true;
            }

            if (!req.Query.ContainsKey("timestamp"))
            {
                var content = TestFile.ReadTestFile("Responses/no-timestamp.xml");
                await context.Response.WriteAsync(content);
                return true;
            }

            if (!req.Query.ContainsKey("nonce"))
            {
                var content = TestFile.ReadTestFile("Responses/no-timestamp.xml");
                await context.Response.WriteAsync(content);
                return true;
            }
#if NET6_0_OR_GREATER
            if (!req.Headers.UserAgent.ToString().Contains("MicroMessenger"))
            {
                var content = TestFile.ReadTestFile("Responses/invalid-user-agent.xml");
                await context.Response.WriteAsync(content);
                return true;
            }
#else
            if (!req.Headers.ContainsKey("User-Agent") 
                && req.Headers["User-Agent"].ToString().Contains("MicroMessenger"))
            {
                var content = TestFile.ReadTestFile("Responses/invalid-user-agent.xml");
                await context.Response.WriteAsync(content);
                return true;
            }
#endif

            var validSignature = SignatureHelper.CalculateSignature(req.Query["timestamp"], req.Query["nonce"], "WEIXINSITETOKEN");
            if (req.Query["signature"] != validSignature)
            {
                var content = TestFile.ReadTestFile("Responses/invalid-signature.xml");
                await context.Response.WriteAsync(content);
                return true;
            }

            if (req.ContentType.Contains("text/xml"))
            {
                using var reader = new StreamReader(req.Body);
                var bodyText = await reader.ReadToEndAsync();
                if (bodyText.StartsWith("<xml>") && bodyText.Contains("<MsgType><![CDATA[text]]></MsgType>"))
                {
                    var content = TestFile.ReadTestFile("Responses/valid-message.xml");
                    await context.Response.WriteAsync(content);
                    return true;
                }
            }
            return false;
        });
    }

    public static TestServer CreateTencentDebugServer()
    {
        return TestServerBuilder.CreateServer(app =>
        {
            app.UseMiddleware<WeixinSiteMiddleware>();

        },
        services =>
        {
            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
            })
            .AddWeixinSite(o =>
            {
                o.WebsiteToken = "WEIXINSITETOKEN";
                o.Debug = true; // Important!
                o.Events.OnTextMessageReceived = async (x) =>
                {
                    var resp = new WeixinResponseBuilder<WeixinResponseText>(x.Context, x.Xml);
                    resp.ResponseEntity.Content = $"Your message had been received:{x.Xml.Content}";
                    await resp.FlushAsync();
                    return true;
                };
            });

        },
        async context =>
        {
            var req = context.Request;
            if (!req.Path.Value.StartsWith(WeixinSiteOptionsDefaults.Path))
            {

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                var content = "404 NOT FOUND";
                await context.Response.WriteAsync(content);
                return true;
            }

            if (req.Method == "GET")
            {
                var content = "You are now trying to visit a verification URL for Weixin Site.";
                await context.Response.WriteAsync(content);
                return true;
            }

            if (req.Method != "POST")
            {
                return false;
            }

            if (!req.Query.ContainsKey("signature"))
            {
                var content = TestFile.ReadTestFile("Responses/no-signature.xml");
                await context.Response.WriteAsync(content);
                return true;
            }

            if (!req.Query.ContainsKey("timestamp"))
            {
                var content = TestFile.ReadTestFile("Responses/no-timestamp.xml");
                await context.Response.WriteAsync(content);
                return true;
            }

            if (!req.Query.ContainsKey("nonce"))
            {
                var content = TestFile.ReadTestFile("Responses/no-timestamp.xml");
                await context.Response.WriteAsync(content);
                return true;
            }
#if NET6_0_OR_GREATER
            if (!req.Headers.UserAgent.ToString().Contains("MicroMessenger"))
            {
                var content = TestFile.ReadTestFile("Responses/invalid-user-agent.xml");
                await context.Response.WriteAsync(content);
                return true;
            }
#else
            if (!req.Headers.ContainsKey("User-Agent") 
                && req.Headers["User-Agent"].ToString().Contains("MicroMessenger"))
            {
                var content = TestFile.ReadTestFile("Responses/invalid-user-agent.xml");
                await context.Response.WriteAsync(content);
                return true;
            }
#endif

            var validSignature = SignatureHelper.CalculateSignature(req.Query["timestamp"], req.Query["nonce"], "WEIXINSITETOKEN");
            if (req.Query["signature"] != validSignature)
            {
                var content = TestFile.ReadTestFile("Responses/invalid-signature.xml");
                await context.Response.WriteAsync(content);
                return true;
            }

            if (req.ContentType.Contains("text/xml"))
            {
                using var reader = new StreamReader(req.Body);
                var bodyText = await reader.ReadToEndAsync();
                if (bodyText.StartsWith("<xml>") && bodyText.Contains("<MsgType><![CDATA[text]]></MsgType>"))
                {
                    var content = TestFile.ReadTestFile("Responses/valid-message.xml");
                    await context.Response.WriteAsync(content);
                    return true;
                }
            }
            return false;
        });
    }
}
