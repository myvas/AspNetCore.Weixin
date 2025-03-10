using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Myvas.AspNetCore.Weixin.Api.RealTests;

public class RealWeixinTestBase
{
    protected IConfiguration Configuration { get; }

    public RealWeixinTestBase()
    {
        Configuration = new ConfigurationBuilder()
            .AddUserSecrets("Myvas.AspNetCore.Weixin.Tests")  // The UserSecretsId specified by this xunit test project.
            .AddEnvironmentVariables()
            .Build();
    }

    [Fact]
    public void TestWithEnvironmentVariable()
    {
        // 如果在您的机器上测试，请在微信公众平台(https://mp.weixin.qq.com)上获取配置，并设置User Secrets或环境变量。
        // 在本地机器上测试，推荐使用环境变量，键名分别为：
        // WEIXIN__APPID
        // WEIXIN__APPSECRET
        // WEIXIN__REDISCONNECTION
        // Notes: There are double underscores in the name of environment variables, which replace the colon in the configuration key.

        var appId = Configuration["Weixin:AppId"];
        var appSecret = Configuration["Weixin:AppSecret"];
        var redisConnection = Configuration["Weixin:RedisConnection"];

        Assert.NotNull(appId);
        Assert.NotNull(appSecret);
        Assert.NotNull(redisConnection);
    }
}