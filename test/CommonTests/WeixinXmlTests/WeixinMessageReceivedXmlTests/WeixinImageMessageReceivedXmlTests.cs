using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinImageMessageReceivedXmlTests
{
    [Fact]
    public void WeixinTextMessageReceivedXml_Serialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-image.xml.txt");
        var o = new ImageMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MediaId = "media_id",
            PicUrl = "this is a url",
            MsgId = 6403247895999455936
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        var s2=WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void WeixinMessageReceivedXml_Deserialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-image.xml.txt");
        var o = new ImageMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MediaId = "media_id",
            PicUrl = "this is a url",
            MsgId = 6403247895999455936
        };

        var result = MyvasXmlConvert.DeserializeObject<ImageMessageReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.MsgId, result.MsgId);
        Assert.Equal(o.PicUrl, result.PicUrl);
        Assert.Equal(o.MsgId, result.MsgId);
    }
}