﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Myvas.AspNetCore.Weixin.Site.Tests.TestServers;

public static class FakeServerBuilder
{
    public static TestServer CreateTencentServer()
    {
        return TestServerBuilder.CreateServer(app =>
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

    public static TestServer CreateTencentDebugServer()
    {
        return TestServerBuilder.CreateServer(app =>
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
                o.Debug = true; // Important in this test suit!
            })
            .AddDebugEventSink();
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
}
