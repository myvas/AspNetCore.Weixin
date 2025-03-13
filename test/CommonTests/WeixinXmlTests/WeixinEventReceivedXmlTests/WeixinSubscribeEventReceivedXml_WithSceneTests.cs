using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinSubscribeEventReceivedXml_WithSceneTests
{
    [Fact]
    public void Serialize()
    {
        string s = TestFile.ReadTestFile("event/event-subscribe-scene.xml.txt");
        var o = new SubscribeEventReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "event",
            Event = "subscribe",
            EventKey = "qrscene_123123",
            Ticket = "gQE28TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyUkhQZWgzelI4QVUxMDAwMGcwN3kAAgR6ittYAwQAAAAA"
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = TestFile.ReadTestFile("event/event-subscribe-scene.xml.txt");
        var o = new SubscribeEventReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "event",
            Event = "subscribe",
            EventKey = "qrscene_123123",
            Ticket = "gQE28TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyUkhQZWgzelI4QVUxMDAwMGcwN3kAAgR6ittYAwQAAAAA"
        };

        var result = MyvasXmlConvert.DeserializeObject<SubscribeEventReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.Event, result.Event);
        Assert.Equal(o.EventKey, result.EventKey);
        Assert.Equal(o.Ticket, result.Ticket);
    }
}