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
    public class AccessTokenServiceRealCallTests
    {
        [Fact]
        public async Task GetTokenShouldReturnInvalidAppId()
        {
            var services = new ServiceCollection();
            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
            });
            var serviceProvider = services.BuildServiceProvider();
            var api = serviceProvider.GetRequiredService<IWeixinAccessTokenApi>();

            var ex = await Assert.ThrowsAsync<WeixinException>(() => api.GetTokenAsync());
            Assert.Equal(40013, ex.ErrorJson.ErrorCode);
            Assert.StartsWith("invalid appid", ex.Message);
        }
    }
}
