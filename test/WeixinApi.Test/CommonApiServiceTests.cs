using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Myvas.AspNetCore.Weixin;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Myvas.AspNetCore.Weixin.Api.Test
{
    public class CommonApiServiceTests
    {
        private readonly TestServer _server;
        public CommonApiServiceTests()
        {
            _server = FakeServerBuilder.CreateTencentServer();
        }

        [Fact]
        public async Task GetCallbackIpsShouldSuccess()
        {
            var services = new ServiceCollection();
            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();
            var api = serviceProvider.GetRequiredService<IWeixinCommonApi>();
            var json = await api.GetCallbackIpsAsync();

            Assert.True(json.Succeeded);
            Assert.Contains("127.0.0.1", json.Ips);
            Assert.Contains("127.0.0.2", json.Ips);
            Assert.Contains("101.226.103.0/25", json.Ips);
        }

        [Fact]
        public async Task GetTencentServerIpsShouldSuccess()
        {
            var services = new ServiceCollection();
            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();
            var api = serviceProvider.GetRequiredService<IWeixinCommonApi>();
            var json = await api.GetTencentServerIpsAsync();

            Assert.True(json.Succeeded);
            Assert.Contains("127.0.0.1", json.Ips);
            Assert.Contains("127.0.0.2", json.Ips);
            Assert.Contains("101.226.103.0/25", json.Ips);
        }

        [Fact]
        public async Task CheckNetworkShouldSuccess()
        {
            var services = new ServiceCollection();
            services.AddWeixin(o =>
            {
                o.AppId = "APPID";
                o.AppSecret = "APPSECRET";
                o.Backchannel = _server.CreateClient();
            });
            var serviceProvider = services.BuildServiceProvider();
            var api = serviceProvider.GetRequiredService<IWeixinCommonApi>();
            var data = new CheckNetworkRequestJson
            {
                Action = CheckNetworkActions.All,
                CheckOperator = CheckNetworkOperators.Default
            };
            var json = await api.CheckNetworkAsync(data);

            Assert.True(json.Succeeded);
            Assert.NotNull(json.Dns);
            Assert.NotNull(json.Ping);
            var dnsIps = json.Dns.Select(x => x.ip).ToArray();
            var PingIps = json.Ping.Select(x => x.ip).ToArray();
            Assert.Contains("111.161.64.40", dnsIps);
            Assert.Contains("111.161.64.48", dnsIps);
        }
    }
}
