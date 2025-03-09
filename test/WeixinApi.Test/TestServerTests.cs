using System.Net;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin.Api.Test
{
    public class TestServerTests
    {
        private readonly TestServer _server;
        public TestServerTests()
        {
            _server = FakeServerBuilder.CreateTencentServer();
        }

        [Fact]
        public async Task TestServerShouldReturnAccessToken()
        {
            var uri = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET";

            var transaction = await _server.SendAsync(uri);
            var resp = transaction.Response;
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);

            //var json = await _server.CreateClient().GetFromJsonAsync<WeixinAccessTokenJson>(uri); // bug for inheritance class
            var s = await _server.CreateClient().GetStringAsync(uri);
            var json = JsonSerializer.Deserialize<WeixinAccessTokenJson>(s);
            Assert.True(json.Succeeded);
            Assert.Equal("ACCESS_TOKEN", json.AccessToken);
            Assert.Equal(7200, json.ExpiresIn);
        }

        [Fact]
        public async Task TestServerShouldReturnInvalidAppId()
        {
            var uri = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=INVALID_APPID&secret=APPSECRET";

            //var json2 = await server.CreateClient().GetFromJsonAsync<WeixinAccessTokenJson>(uri); // bug for inheritance class
            var s = await _server.CreateClient().GetStringAsync(uri);
            var json = JsonSerializer.Deserialize<WeixinAccessTokenJson>(s);
            Assert.False(json.Succeeded);
            Assert.Equal(40013, json.ErrorCode);
            Assert.Contains("invalid appid", json.ErrorMessage);
        }
    }
}
