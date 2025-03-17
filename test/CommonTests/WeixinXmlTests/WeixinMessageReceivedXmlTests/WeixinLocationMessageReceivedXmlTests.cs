using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinLocationMessageReceivedXmlTests
{
    [Fact]
    public void Serialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-location.xml.txt");
        var o = new LocationMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936,
            Latitude = 23.134521m,
            Longitude = 113.358803m,
            Scale = 20,
            Label = "位置信息"
        };

        var result = WeixinXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-location.xml.txt");
        var o = new LocationMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936,
            Latitude = 23.134521m,
            Longitude = 113.358803m,
            Scale = 20,
            Label = "位置信息"
        };

        var result = WeixinXmlConvert.DeserializeObject<LocationMessageReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.MsgId, result.MsgId);
        Assert.Equal(o.Latitude, result.Latitude);
        Assert.Equal(o.Longitude, result.Longitude);
        Assert.Equal(o.Scale, result.Scale);
        Assert.Equal(o.Label, result.Label);
    }
}