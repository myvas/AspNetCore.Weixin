using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin.Api.Tests.TestServers;

public static class FakeTencentServerBuilder
{
    public static TestServer CreateTencentServer()
    {
        return TestServerBuilder.CreateServer(null, null, async context =>
        {
            var req = context.Request;
            switch (req.Path.Value)
            {
                case "/cgi-bin/token":
                    {
                        var appId = req.Query["appid"];
                        if (appId != "APPID")
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadAllText("AccessToken/invalid_appid.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        else
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadAllText("AccessToken/ok.json");
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
                            var content = TestFile.ReadAllText("AccessToken/invalid_appid.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        else
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadAllText("AccessToken/ok.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                    }
                case "/cgi-bin/getcallbackip":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadAllText("Common/getcallbackip.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/get_api_domain_ip":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadAllText("Common/get_api_domain_ip.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/callback/check":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadAllText("Common/callback_check.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/menu/create":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadAllText("Menu/menu_create.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/get_current_selfmenu_info":
                    {
                        var version = req.Query["v"].ToString();
                        if (string.IsNullOrEmpty(version)) version = (new Random().Next(0, 2) == 0) ? "0" : "1";
                        if (version == "0")
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadAllText("Menu/get_current_selfmenu_info.api.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        else if (version == "1")
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadAllText("Menu/get_current_selfmenu_info.web.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        else
                        {
                            // Impossible here!
                            throw new InvalidOperationException();
                        }
                    }
                case "/cgi-bin/menu/delete":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadAllText("Menu/menu_delete.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/menu/addconditional":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadAllText("Menu/menu_addconditional.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/menu/trymatch":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadAllText("Menu/menu_trymatch.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/menu/delconditional":
                    {
                        context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadAllText("Menu/0.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                case "/cgi-bin/menu/get":
                    {
                        var version = (new Random().Next(0, 2) == 0) ? "0" : "1";
                        if (version == "0")
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadAllText("Menu/menu_get.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                        else
                        {
                            context.Response.Headers.TryAdd(HeaderNames.ContentType, "application/json");
                            var content = TestFile.ReadAllText("Menu/menu_get_with_conditional.json");
                            await context.Response.WriteAsync(content);
                            return true;
                        }
                    }
                default:
                    throw new NotImplementedException();
            }
        });
    }
}
