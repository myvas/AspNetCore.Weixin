﻿using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class WeixinCommonApiTests
{
    private readonly TestServer _server;
    public WeixinCommonApiTests()
    {
        _server = FakeTencentServerBuilder.CreateTencentServer();
    }

    [Fact]
    public async Task GetCallbackIpsShouldSuccess()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinCommonApi>();
        var json = await api.GetCallbackIpsAsync();

        Assert.True(json.Succeeded);
        Assert.Contains("127.0.0.1", json.Ips);
        Assert.Contains("127.0.0.2", json.Ips);
        Assert.Contains("101.226.103.0/25", json.Ips);
    }

    [Fact]
    public async Task GetTencentServerIpsShouldSuccess()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinCommonApi>();
        var json = await api.GetTencentServerIpsAsync();

        Assert.True(json.Succeeded);
        Assert.Contains("127.0.0.1", json.Ips);
        Assert.Contains("127.0.0.2", json.Ips);
        Assert.Contains("101.226.103.0/25", json.Ips);
    }

    [Fact]
    public async Task CheckNetworkShouldSuccess()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinCommonApi>();
        var data = new WeixinCheckNetworkRequestJson
        {
            Action = WeixinCheckNetworkActions.All,
            CheckOperator = WeixinCheckNetworkOperators.Default
        };
        var json = await api.CheckNetworkAsync(data);

        Assert.True(json.Succeeded);
        Assert.NotNull(json.Dns);
        Assert.NotNull(json.Ping);
        var dnsIps = json.Dns.Select(x => x.ip).ToArray();
        var PingIps = json.Ping.Select(x => x.ip).ToArray();
        Assert.Contains("111.161.64.40", dnsIps);
        Assert.Contains("111.161.64.48", dnsIps);
    }
}
