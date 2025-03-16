using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin.Api.Tests.TestServers;

public static class FakeServerBuilder
{
    public static TestServer CreateTencentServer()
    {
        return TestServerBuilder.CreateServer(null, null,
        async context =>
        {
            var req = context.Request;
            // It's wired that there is a query string in the req.Path, so we must remove it!
            string[] wiredPath = req.Path.Value.Split('?');
            switch (wiredPath[0])
            {
                case "/cgi-bin/token":
                    {
                        var appId = req.Query["appid"];
                        if (appId != "APPID")
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadTestJsonFile("AccessToken/invalid_appid.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        else
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadTestJsonFile("AccessToken/ok.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
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
                            var content = TestFile.ReadTestJsonFile("AccessToken/invalid_appid.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        else
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadTestJsonFile("AccessToken/ok.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                    }
                case "/cgi-bin/getcallbackip":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadTestJsonFile("Common/getcallbackip.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/get_api_domain_ip":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadTestJsonFile("Common/get_api_domain_ip.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/callback/check":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadTestJsonFile("Common/callback_check.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/menu/create":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadTestJsonFile("Menu/menu_create.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/get_current_selfmenu_info":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadTestJsonFile("Menu/get_current_selfmenu_info.api.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/menu/delete":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadTestJsonFile("Menu/menu_delete.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                default:
                    throw new NotImplementedException();
            }
        });
    }
}
