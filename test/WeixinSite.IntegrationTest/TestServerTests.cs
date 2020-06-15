using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Http;
using System.Net.Mime;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.TestHost;
using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.Site.Test
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

            //var json2 = await server.CreateClient().GetFromJsonAsync<WeixinAccessTokenJson>(uri); // bug for inheritance class
            var s = await _server.CreateClient().GetStringAsync(uri);
            var json = JsonConvert.DeserializeObject<WeixinAccessTokenJson>(s);
            Assert.True(json.Succeeded);
            Assert.Equal("ACCESS_TOKEN", json.access_token);
            Assert.Equal(7200, json.expires_in);
        }


        [Fact]
        public async Task TestServerShouldReturnInvalidAppId()
        {
            var uri = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=INVALID_APPID&secret=APPSECRET";
            //var json2 = await server.CreateClient().GetFromJsonAsync<WeixinAccessTokenJson>(uri); // bug for inheritance class
            var s = await _server.CreateClient().GetStringAsync(uri);
            var json = JsonConvert.DeserializeObject<WeixinAccessTokenJson>(s);
            Assert.False(json.Succeeded);
            Assert.Equal(40013, json.errcode);
            Assert.Contains("invalid appid", json.errmsg);
        }
    }
}
