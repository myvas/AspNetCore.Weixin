using Microsoft.Extensions.Configuration;

namespace Myvas.AspNetCore.Weixin.Api.Tests;

public class RealWeixinServerBase : RealRedisServerBase
{
    /// <summary>
    /// Whether enable to test on a real weixin server and official account (optional, default is false)
    /// </summary>
    /// <remarks>
    /// <para>For usersecrets, or appsettings.json:
    /// The key is `Weixin:EnableRealWeixinTests`.</para><para>
    /// For GitHub secrets, or environment variables:
    /// The key is `WEIXIN__ENABLEREALWEIXINTESTS`.</para>
    /// </remarks>
    protected bool EnableRealWeixinTests { get; }
    
    public RealWeixinServerBase()
    {
        // We rule this real weixin tests must also use real redis server
        EnableRealWeixinTests = EnableRealRedisTests
            && Configuration.GetValue<bool>("Weixin:EnableRealWeixinTests", false);
    }

    [Fact]
    public void TestWithEnvironmentVariable()
    {
        if(!EnableRealWeixinTests) return;
        
        var appId = Configuration["Weixin:AppId"];
        var appSecret = Configuration["Weixin:AppSecret"];

        Assert.NotNull(appId);
        Assert.NotNull(appSecret);
    }
}