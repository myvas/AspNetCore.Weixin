using System.Text.Json;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.CommonTests.JsonSerializerTests;

public class JsonSerializerTests
{
    [Fact]
    public void WeixinAccessTokenJson_Serialize()
    {
        var s = @"{""access_token"":""89_RnWF3ynfikijP6gaqt_XlOtYCvV6188JYiMQcFvAEu94Ksih8Z3qML-Vio7ZDthRtZDaVqtbF8-W0LTD9Etenkso_nwmhmlEicOXnA7rQpu1ezENvNU6JhFHSpAVUBfAHAYXE"",""expires_in"":7200}";
        var obj = new WeixinAccessTokenJson
        {
            AccessToken = "89_RnWF3ynfikijP6gaqt_XlOtYCvV6188JYiMQcFvAEu94Ksih8Z3qML-Vio7ZDthRtZDaVqtbF8-W0LTD9Etenkso_nwmhmlEicOXnA7rQpu1ezENvNU6JhFHSpAVUBfAHAYXE",
            ExpiresIn = 7200
        };
        var options = new JsonSerializerOptions();
        options.AllowTrailingCommas = true;
#if NET5_0_OR_GREATER
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
#else
        options.IgnoreNullValues = true;
#endif
        var result = JsonSerializer.Serialize(obj, options);
        Assert.Equal(s, result);
    }

    [Fact]
    public void WeixinAccessTokenJson_Deserialize()
    {
        var json = @"{""access_token"":""89_RnWF3ynfikijP6gaqt_XlOtYCvV6188JYiMQcFvAEu94Ksih8Z3qML-Vio7ZDthRtZDaVqtbF8-W0LTD9Etenkso_nwmhmlEicOXnA7rQpu1ezENvNU6JhFHSpAVUBfAHAYXE"",""expires_in"":7200}";
        var result = JsonSerializer.Deserialize<WeixinAccessTokenJson>(json);

        Assert.True(result.Succeeded);
        Assert.Equal(7200, result.ExpiresIn);
        Assert.Equal("89_RnWF3ynfikijP6gaqt_XlOtYCvV6188JYiMQcFvAEu94Ksih8Z3qML-Vio7ZDthRtZDaVqtbF8-W0LTD9Etenkso_nwmhmlEicOXnA7rQpu1ezENvNU6JhFHSpAVUBfAHAYXE", result.AccessToken);
    }

    [Fact]
    public void WeixinAccessTokenJson_Deserialize2()
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
    public void UserGetJson_Deserialize()
    {
        string json = @"{
""total"":2,
""count"":2,
""data"":{""openid"":[""OPENID1"",""OPENID2""]},
""next_openid"":""NEXT_OPENID""
}";
        var result = JsonSerializer.Deserialize<WeixinUserGetJson>(json);
        Assert.Equal(2, result.total);
        Assert.Equal(2, result.count);
        Assert.Equal(2, result.data.openid.Count);
        Assert.Contains("OPENID1", result.data.openid);
        Assert.Contains("OPENID2", result.data.openid);
        Assert.Equal("NEXT_OPENID", result.next_openid);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public void UserInfoJson_Deserialize()
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
        var result = JsonSerializer.Deserialize<WeixinUserInfoJson>(json);
        Assert.Equal(1, result.Subscribed);
        Assert.Equal("o6_bmjrPTlm6_2sgVt7hMZOPfL2M", result.OpenId);
        Assert.Equal("zh_CN", result.Language);
        Assert.Equal(1382694957, result.SubscribedTime);
        Assert.Equal("o6_bmasdasdsad6_2sgVt7hMZOPfL", result.UnionId);
        Assert.Empty(result.Remark);
        Assert.Equal(0, result.GroupId);
        Assert.Equal(2, result.TagIdList.Count);
        Assert.Contains(128, result.TagIdList);
        Assert.Contains(2, result.TagIdList);
        Assert.Equal("ADD_SCENE_QR_CODE", result.SubscribeScene);
        Assert.Empty(result.QrsceneNote);
        Assert.True(result.Succeeded);
    }
}
