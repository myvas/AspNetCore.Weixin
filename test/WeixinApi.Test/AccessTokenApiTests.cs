using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Myvas.AspNetCore.Weixin;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Api.Test
{
    public class AccessTokenApiTests
    {
        private readonly TestServer _server;
        public AccessTokenApiTests()
        {
            _server = FakeServerBuilder.CreateTencentServer();
        }

        [Fact]
        public async Task GetTokenShouldSuccess()
        {
            var services = new ServiceCollection();
            services.Configure<WeixinApiOptions>(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();
            var optionsAccessor = serviceProvider.GetRequiredService<IOptions<WeixinApiOptions>>();
            var api = new WeixinAccessTokenApi(optionsAccessor); var json = await api.GetTokenAsync();

            Assert.True(json.Succeeded);
            Assert.Equal("ACCESS_TOKEN", json.access_token);
            Assert.Equal(7200, json.expires_in);
        }

        [Fact]
        public async Task GetTokenShouldReturnInvalidAppId()
        {
            var services = new ServiceCollection();
            services.Configure<WeixinApiOptions>(o =>
            {
                o.AppId = "INVALID_APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();
            var optionsAccessor = serviceProvider.GetRequiredService<IOptions<WeixinApiOptions>>();
            var api = new WeixinAccessTokenApi(optionsAccessor);

            var ex = await Assert.ThrowsAsync<WeixinException>(() => api.GetTokenAsync());
            Assert.Equal(40013, ex.ErrorCode);
            Assert.Contains("invalid appid", ex.Message);
        }
    }
}
