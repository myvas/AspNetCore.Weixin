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

        [Fact]
        public void GetAllOpenIdsAsync()
        {
            string s = @"{
""total"":2,
""count"":2,
""data"":{""openid"":[""OPENID1"",""OPENID2""]},
""next_openid"":""NEXT_OPENID""
}";
            var o = System.Text.Json.JsonSerializer.Deserialize<UserGetJson>(s);
            Assert.Equal(2, o.total);
            Assert.Equal(2, o.count);
            Assert.Equal(2, o.data.openid.Count);
            Assert.Contains("OPENID1", o.data.openid);
            Assert.Contains("OPENID2", o.data.openid);
            Assert.Equal("NEXT_OPENID", o.next_openid);
            Assert.True(o.Succeeded);
        }

        [Fact]
        public void GetUserInfoAsync()
        {
            string s = @"{
    ""subscribe"": 1, 
    ""openid"": ""o6_bmjrPTlm6_2sgVt7hMZOPfL2M"", 
    ""language"": ""zh_CN"", 
    ""subscribe_time"": 1382694957,
    ""unionid"": ""o6_bmasdasdsad6_2sgVt7hMZOPfL"",
    ""remark"": """",
    ""groupid"": 0,
    ""tagid_list"":[128,2],
    ""subscribe_scene"": ""ADD_SCENE_QR_CODE"",
    ""qr_scene"": 98765,
    ""qr_scene_str"": """"
}";
            var o = System.Text.Json.JsonSerializer.Deserialize<UserInfoJson>(s);
            Assert.Equal(1, o.subscribe);
            Assert.Equal("o6_bmjrPTlm6_2sgVt7hMZOPfL2M", o.openid);
            Assert.Equal("zh_CN", o.language);
            Assert.Equal(1382694957, o.subscribe_time);
            Assert.Equal("o6_bmasdasdsad6_2sgVt7hMZOPfL", o.unionid);
            Assert.Empty(o.remark);
            Assert.Equal(0, o.groupid);
            Assert.Equal(2, o.tagid_list.Count);
            Assert.Contains(128, o.tagid_list);
            Assert.Contains(2, o.tagid_list);
            Assert.Equal("ADD_SCENE_QR_CODE", o.subscribe_scene);
            Assert.Empty(o.qr_scene_str);
            Assert.True(o.Succeeded);
        }
    }
}
