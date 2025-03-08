using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin;
using System;
using Xunit;

namespace Weixin.Tests.ServiceTests
{
    public class RealAddWeixinAccessTokenTests : RealWeixinServiceAccountTestBase
    {
        [Fact]
        public void GetToken_ShouldSuccess()
        {
            IServiceCollection services = new ServiceCollection();
            services.Configure((Action<WeixinOptions>)(options =>
            {
                options.AppId = Configuration["Weixin:AppId"];
                options.AppSecret = Configuration["Weixin:AppSecret"];
            }));
            services.AddLogging();
            services.AddMemoryCache();
            services.AddWeixin()
                .AddWeixinAccessTokenApi()
                .AddAccessTokenMemoryCacheProvider();
            var sp = services.BuildServiceProvider();

            var testService = sp.GetRequiredService<IWeixinAccessTokenApi>();
            var _loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var _logger = _loggerFactory.CreateLogger("test");

            var result = testService.GetToken();

            Assert.NotNull(result);
        }
    }
}