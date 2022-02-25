using Myvas.AspNetCore.Weixin;
using Xunit;

namespace test
{
    public class SystemTextJsonSerializer_Deserialize_Tests
    {
        [Fact]
        public void GetAccessTokenAsync()
        {
            string s = @"{
""access_token"": ""54_1s-7myKXeZhBU163CbCf18snjgjaBooBBIS82GC9ZGVfC40XZdRIzgygRV1oJSQJ3jlCfHc1_doBlx3Umf7vW1h7rMQEA8Tnzu0kDrkoHVHHmWiOw5Qzjf2eVENo8KDWwtFO-FjViHr6Q25qAJXeAEAQZN"",
""expires_in"": 7200
}";
            var o = System.Text.Json.JsonSerializer.Deserialize<WeixinAccessTokenJson>(s);
            Assert.Equal("54_1s-7myKXeZhBU163CbCf18snjgjaBooBBIS82GC9ZGVfC40XZdRIzgygRV1oJSQJ3jlCfHc1_doBlx3Umf7vW1h7rMQEA8Tnzu0kDrkoHVHHmWiOw5Qzjf2eVENo8KDWwtFO-FjViHr6Q25qAJXeAEAQZN", o.AccessToken);
            Assert.Equal(7200, o.ExpiresIn);
            Assert.True(o.Succeeded);
        }
    }
}
