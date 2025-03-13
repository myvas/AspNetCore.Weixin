using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinTextMessageReceivedXmlTests
{
    [Fact]
    public void WeixinTextMessageReceivedXml_Serialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-text.xml.txt");
        var s2 = MyvasXmlConvert.SerializeObject(MyvasXmlConvert.DeserializeObject<WeixinXml>(s));
        var o = new TextMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj",
            CreateTime = 1490872329,
            MsgType = "text",
            Content = "中文字符",
            MsgId = 6403247895999455936
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void WeixinMessageReceivedXml_Deserialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-text.xml.txt");
        var o = new TextMessageReceivedXml{
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj",
            CreateTime = 1490872329,
            MsgType = "text",
            Content = "中文字符",
            MsgId = 6403247895999455936
        };

        var result = MyvasXmlConvert.DeserializeObject<TextMessageReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.MsgId, result.MsgId);
    }
}