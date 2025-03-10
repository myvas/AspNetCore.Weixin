using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Myvas.AspNetCore.Weixin.CommonTests.JsonSerializerTests;

public class JsonSerializer_AccessTokenJsonTests
{
    [Fact]
    public void WeixinAccessTokenJson_DeserializeTest()
    {
        var json = @"{""access_token"":""89_RnWF3ynfikijP6gaqt_XlOtYCvV6188JYiMQcFvAEu94Ksih8Z3qML-Vio7ZDthRtZDaVqtbF8-W0LTD9Etenkso_nwmhmlEicOXnA7rQpu1ezENvNU6JhFHSpAVUBfAHAYXE"",""expires_in"":7200}";
        var options = new JsonSerializerOptions();
        options.AllowTrailingCommas = true;
#if NET5_0_OR_GREATER
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
#else
            options.IgnoreNullValues = true;
#endif
        var result = JsonSerializer.Deserialize<WeixinAccessTokenJson>(json, options);

        Assert.True(result.Succeeded);
        Assert.Equal(7200, result.ExpiresIn);
        Assert.Equal("89_RnWF3ynfikijP6gaqt_XlOtYCvV6188JYiMQcFvAEu94Ksih8Z3qML-Vio7ZDthRtZDaVqtbF8-W0LTD9Etenkso_nwmhmlEicOXnA7rQpu1ezENvNU6JhFHSpAVUBfAHAYXE", result.AccessToken);
    }

    [Fact]
    public void WeixinAccessTokenJson_DeserializeTest2()
    {
        string json = @"{
""access_token"": ""54_1s-7myKXeZhBU163CbCf18snjgjaBooBBIS82GC9ZGVfC40XZdRIzgygRV1oJSQJ3jlCfHc1_doBlx3Umf7vW1h7rMQEA8Tnzu0kDrkoHVHHmWiOw5Qzjf2eVENo8KDWwtFO-FjViHr6Q25qAJXeAEAQZN"",
""expires_in"": 7200
}";
        var result = JsonSerializer.Deserialize<WeixinAccessTokenJson>(json);
        Assert.True(result.Succeeded);
        Assert.Equal(7200, result.ExpiresIn);
        Assert.Equal("54_1s-7myKXeZhBU163CbCf18snjgjaBooBBIS82GC9ZGVfC40XZdRIzgygRV1oJSQJ3jlCfHc1_doBlx3Umf7vW1h7rMQEA8Tnzu0kDrkoHVHHmWiOw5Qzjf2eVENo8KDWwtFO-FjViHr6Q25qAJXeAEAQZN", result.AccessToken);

    }

    [Fact]
    public void UserGetJson_DeserializeTest()
    {
        string json = @"{
""total"":2,
""count"":2,
""data"":{""openid"":[""OPENID1"",""OPENID2""]},
""next_openid"":""NEXT_OPENID""
}";
        var result = JsonSerializer.Deserialize<UserGetJson>(json);
        Assert.Equal(2, result.total);
        Assert.Equal(2, result.count);
        Assert.Equal(2, result.data.openid.Count);
        Assert.Contains("OPENID1", result.data.openid);
        Assert.Contains("OPENID2", result.data.openid);
        Assert.Equal("NEXT_OPENID", result.next_openid);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void UserInfoJson_DeserializeTest()
    {
        string json = @"{
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
        var result = JsonSerializer.Deserialize<UserInfoJson>(json);
        Assert.Equal(1, result.subscribe);
        Assert.Equal("o6_bmjrPTlm6_2sgVt7hMZOPfL2M", result.OpenId);
        Assert.Equal("zh_CN", result.Language);
        Assert.Equal(1382694957, result.SubscribeUnixTime);
        Assert.Equal("o6_bmasdasdsad6_2sgVt7hMZOPfL", result.UnionId);
        Assert.Empty(result.Remark);
        Assert.Equal(0, result.groupid);
        Assert.Equal(2, result.tagid_list.Count);
        Assert.Contains(128, result.tagid_list);
        Assert.Contains(2, result.tagid_list);
        Assert.Equal("ADD_SCENE_QR_CODE", result.subscribe_scene);
        Assert.Empty(result.qr_scene_str);
        Assert.True(result.Succeeded);
    }
}
