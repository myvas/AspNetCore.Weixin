using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinVoiceMessageReceivedXml_WithRecognitionTests
{
    [Fact]
    public void Serialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-voice-recognition.xml.txt");
        var o = new VoiceMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936,
            MediaId = "media_id",
            Format = "Format",
            Recognition = "腾讯微信团队"
        };

        var result = WeixinXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = TestFile.ReadTestFile("msg/msg-voice-recognition.xml.txt");
        var o = new VoiceMessageReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936,
            MediaId = "media_id",
            Format = "Format",
            Recognition = "腾讯微信团队"
        };

        var result = WeixinXmlConvert.DeserializeObject<VoiceMessageReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.MsgId, result.MsgId);
        Assert.Equal(o.MediaId, result.MediaId);
        Assert.Equal(o.Format, result.Format);
        Assert.Equal(o.Recognition, result.Recognition);
    }
}