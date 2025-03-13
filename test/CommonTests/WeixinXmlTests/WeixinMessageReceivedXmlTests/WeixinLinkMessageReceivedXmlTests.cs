using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinLinkMessageReceivedXmlTests
{
    [Fact]
    public void Serialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-link.xml.txt");
        var o = new LinkMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936,
            Title = "我的官网",
            Description = "公众平台官网链接",
            Url = "http://url.com/q?something",
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-link.xml.txt");
        var o = new LinkMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936,
            Title = "我的官网",
            Description = "公众平台官网链接",
            Url = "http://url.com/q?something",
        };

        var result = MyvasXmlConvert.DeserializeObject<LinkMessageReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.MsgId, result.MsgId);
        Assert.Equal(o.Title, result.Title);
        Assert.Equal(o.Description, result.Description);
        Assert.Equal(o.Url, result.Url);
    }
}