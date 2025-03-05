using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using Xunit;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Weixin.Tests
{
    public class RealWeixinServiceAccountTestBase
    {
        protected IConfiguration Configuration { get; }

        public RealWeixinServiceAccountTestBase()
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
            // 在本地机器上测试，推荐使用环境变量，键名分别为：WEIXIN_APPID="wx02056e2b2b9cc4ef" dotnet test
            // WEIXIN_APPID
            // WEIXIN_APPSECRET

            var appId = Configuration["Weixin:AppId"];
            var appSecret = Configuration["Weixin:AppSecret"];
#if DEBUG
            Debug.WriteLine($"Weixin:AppId {Configuration["Weixin:AppId"]}");
            Debug.WriteLine($"WEIXIN_APPID {Configuration["WEIXIN_APPID"]}");
            Debug.WriteLine($"WEIXIN__APPID {Configuration["WEIXIN__APPID"]}");
#else
            Console.WriteLine($"Weixin:AppId {Configuration["Weixin:AppId"]}");
            Console.WriteLine($"WEIXIN_APPID {Configuration["WEIXIN_APPID"]}");
            Console.WriteLine($"WEIXIN__APPID {Configuration["WEIXIN__APPID"]}");
#endif

            Assert.NotNull(appId);
            Assert.NotNull(appSecret);
        }
    }
}