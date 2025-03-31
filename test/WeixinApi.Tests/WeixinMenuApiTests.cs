using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class WeixinMenuApiTests
{
    private readonly TestServer _server;
    public WeixinMenuApiTests()
    {
        _server = FakeTencentServerBuilder.CreateTencentServer();
    }

    [Fact]
    public async Task GetCurrentMenuAsync_Random()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
        var menu = await api.GetCurrentMenuAsync();

        Assert.NotNull(menu);
        Assert.NotNull(menu.SelfMenuInfo);
        Assert.NotNull(menu.SelfMenuInfo.Buttons);
        Assert.Contains(menu.SelfMenuInfo.Buttons, p => p is WeixinMenuJson.Button);
        if (menu.HasButtonClick())
        {
            Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "Today Music");
            Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "Menu");
        }
        else
        {
            Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "text");
            Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "photo");
        }
    }

    [Fact]
    public async Task GetCurrentMenuAsync_0()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
        var menu = await api.GetCurrentMenuAsync("0");

        Assert.NotNull(menu);
        Assert.True(menu.HasButtonClick());
        Assert.NotNull(menu.SelfMenuInfo);
        Assert.NotNull(menu.SelfMenuInfo.Buttons);
        Assert.Contains(menu.SelfMenuInfo.Buttons, p => p is WeixinMenuJson.Button);
        Assert.Equal(2, menu.SelfMenuInfo.Buttons.OfType<WeixinMenuJson.Button>().Count());
        Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "Today Music");
        Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "Menu");
    }

    [Fact]
    public async Task GetCurrentMenuAsync_1()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
        var menu = await api.GetCurrentMenuAsync("1");

        Assert.NotNull(menu);
        Assert.False(menu.HasButtonClick());
        Assert.NotNull(menu.SelfMenuInfo);
        Assert.NotNull(menu.SelfMenuInfo.Buttons);
        Assert.Equal(3, menu.SelfMenuInfo.Buttons.OfType<WeixinMenuJson.Button>().Count());
        Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "text");
        Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "photo");
        Assert.Contains(menu.SelfMenuInfo.Buttons, p => p.Name == "button");
    }

    [Fact]
    public async Task PublishMenuAsync()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
        var menu = new WeixinMenuCreateJson()
            .AddButton(new WeixinMenuJson.Button.Click()
            {
                Name = "Today Music",
                Key = "V1001_TODAY_MUSIC"
            })
            .AddButton(new WeixinMenuJson.Button.Container("Contains buttons")
                .AddButton(new WeixinMenuJson.Button.View
                {
                    Name = "Search",
                    Url = "http://www.soso.com"
                })
                .AddButton(new WeixinMenuJson.Button.View
                {
                    Name = "Video",
                    Url = "http://v.qq.com"
                }).AddButton(new WeixinMenuJson.Button.Click
                {
                    Name = "Like",
                    Key = "V1001_LIKE"
                }));

        var result = await api.PublishMenuAsync(menu);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task PublishConditionalMenuAsync()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
        var menu = new WeixinConditionalMenuCreateJson()
            .AddMatchRule(new WeixinConditionalMenuJson.MatchRule
            {
                GroupId = 2,
                Sex = 1,
                Country = "中国",
                Province = "广东",
                City = "广州",
                ClientPlatformType = 2
            })
            .AddButton(new WeixinMenuJson.Button.Click()
            {
                Name = "Today Music",
                Key = "V1001_TODAY_MUSIC"
            })
            .AddButton(new WeixinMenuJson.Button.Container("Contains buttons")
                .AddButton(new WeixinMenuJson.Button.View
                {
                    Name = "Search",
                    Url = "http://www.soso.com"
                })
                .AddButton(new WeixinMenuJson.Button.View
                {
                    Name = "Video",
                    Url = "http://v.qq.com"
                }).AddButton(new WeixinMenuJson.Button.Click
                {
                    Name = "Like",
                    Key = "V1001_LIKE"
                }));
        var result = await api.PublishConditionalMenuAsync(menu);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task TryMatchMenuAsync()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
        var result = await api.TryMatchMenuAsync("openid_or_wxid");
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteConditionalMenuAsync()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
        var result = await api.DeleteConditionalMenuAsync("menu_id");
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteMenuAsync()
    {
        var services = new ServiceCollection();
        services.AddWeixin(o =>
        {
            o.AppId = "APPID";
            o.AppSecret = "APPSECRET";
            o.Backchannel = _server.CreateClient();
        });
        var serviceProvider = services.BuildServiceProvider();
        var api = serviceProvider.GetRequiredService<IWeixinMenuApi>();
        var result = await api.DeleteMenuAsync();
        Assert.True(result.Succeeded);
    }
}
