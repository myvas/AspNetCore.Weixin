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
    public class AccessTokenServiceRealCallTests
    {
        [Fact]
        public async Task GetTokenShouldReturnInvalidAppId()
        {
            var services = new ServiceCollection();
            services.AddWeixinApi(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
            });
            var serviceProvider = services.BuildServiceProvider();
            var api = serviceProvider.GetRequiredService<IWeixinAccessToken>();

            var ex = await Assert.ThrowsAsync<WeixinException>(() => api.GetTokenAsync());
            Assert.Equal(40013, ex.ErrorJson.errcode);
            Assert.Contains("invalid appid", ex.Message);
        }
    }
}
