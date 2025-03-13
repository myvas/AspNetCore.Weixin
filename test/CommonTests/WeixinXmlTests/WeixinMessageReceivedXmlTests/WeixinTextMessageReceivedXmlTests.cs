using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinTextMessageReceivedXmlTests
{
    [Fact]
    public void WeixinTextMessageReceivedXml_Serialize()
    {
        var s = TestFile.ReadTestFile("msg/msg-text.xml.txt");
        var o = new TextMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936,
            Content = "中文字符"
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void WeixinTextMessageReceivedXml_Deserialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-text.xml.txt");
        var o = new TextMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936,
            Content = "中文字符"
        };

        var result = MyvasXmlConvert.DeserializeObject<TextMessageReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.MsgId, result.MsgId);
        Assert.Equal(o.Content, result.Content);
    }
}