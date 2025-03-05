using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;
using Myvas.AspNetCore.Weixin.Services; // Add this line to include the correct namespace

namespace Weixin.Tests.ServiceTests
{
    public class RealAddWeixinAccessTokenTests : RealWeixinServiceAccountTestBase
    {
        [Fact]
        public void GetToken_ShouldSuccess()
        {
            IServiceCollection services = new ServiceCollection();
            services.Configure((Action<WeixinApiOptions>)(options =>
            {
                options.AppId = Configuration["Weixin:AppId"];
                options.AppSecret = Configuration["Weixin:AppSecret"];
            }));
            services.AddLogging();
            services.AddMemoryCache();
            services.AddWeixinApi();
            services.AddSingleton<IWeixinAccessToken, MemoryCachedWeixinAccessToken>();
            var sp = services.BuildServiceProvider();

            var testService = sp.GetRequiredService<IWeixinAccessToken>();
            var _loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var _logger = _loggerFactory.CreateLogger("test");

            var result = testService.GetToken();

            Assert.NotNull(result);
        }
    }
}