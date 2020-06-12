using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.AccessToken.Test
{
    public class AccessTokenApiServiceTests
    {
        private readonly TestServer _server;
        public AccessTokenApiServiceTests()
        {
            _server = FakeServerBuilder.CreateTencentServer();
        }

        [Fact]
        public async Task GetTokenShouldSuccess()
        {
            var services = new ServiceCollection();
            services.AddWeixinApi(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();

            var api = serviceProvider.GetRequiredService<IWeixinAccessToken>();
            var json = await api.GetTokenAsync();

            Assert.Equal("ACCESS_TOKEN", json);
        }

        [Fact]
        public async Task GetTokenShouldReturnInvalidAppId()
        {
            var services = new ServiceCollection();
            services.AddWeixinApi(o =>
            {
                o.AppId = "INVALID_APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();

            var api = serviceProvider.GetRequiredService<IWeixinAccessToken>();

            var ex = await Assert.ThrowsAsync<WeixinException>(() => api.GetTokenAsync());
            Assert.Equal(40013, ex.ErrorJson.errcode);
            Assert.Contains("invalid appid", ex.Message);
        }
    }
}
