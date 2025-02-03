using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin.Site.Test
{
    public static class FakeServerBuilder
    {
        public static TestServer CreateTencentServer()
        {
            return TestServerBuilder.CreateServer(null, null,
            async context =>
            {
                var req = context.Request;
                if (req.Path.Value.EndsWith("/cgi-bin/token"))
                {
                    var appId = req.Query["appid"];
                    if (appId != "APPID")
                    {
                        context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadTestFile("AccessToken/invalid_appid.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                    else
                    {
                        context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                        var content = TestFile.ReadTestFile("AccessToken/ok.json");
                        await context.Response.WriteAsync(content);
                        return true;
                    }
                }
                else if (req.Path.Value.EndsWith("/cgi-bin/getcallbackip"))
                {
                    context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                    var content = TestFile.ReadTestFile("Common/getcallbackip.json");
                    await context.Response.WriteAsync(content);
                    return true;
                }
                else if (req.Path.Value.EndsWith("/cgi-bin/get_api_domain_ip"))
                {
                    context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                    var content = TestFile.ReadTestFile("Common/get_api_domain_ip.json");
                    await context.Response.WriteAsync(content);
                    return true;
                }
                else if (req.Path.Value.EndsWith("/cgi-bin/callback/check"))
                {
                    context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                    var content = TestFile.ReadTestFile("Common/callback_check.json");
                    await context.Response.WriteAsync(content);
                    return true;
                }
                else if (req.Path.Value.EndsWith("/cgi-bin/menu/create"))
                {
                    context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                    var content = TestFile.ReadTestFile("Menu/menu_create.json");
                    await context.Response.WriteAsync(content);
                    return true;
                }
                else if (req.Path.Value.EndsWith("/cgi-bin/get_current_selfmenu_info"))
                {
                    context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                    var content = TestFile.ReadTestFile("Menu/get_current_selfmenu_info.api.json");
                    await context.Response.WriteAsync(content);
                    return true;
                }
                else if (req.Path.Value.EndsWith("/cgi-bin/menu/delete"))
                {
                    context.Response.Headers.Add(HeaderNames.ContentType, "application/json");
                    var content = TestFile.ReadTestFile("Menu/menu_delete.json");
                    await context.Response.WriteAsync(content);
                    return true;
                }
                return false;
            });
        }
    }
}
