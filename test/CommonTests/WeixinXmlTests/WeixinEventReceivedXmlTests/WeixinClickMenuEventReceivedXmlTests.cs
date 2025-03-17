using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinClickMenuEventReceivedXmlTests
{
    [Fact]
    public void Serialize()
    {
        string s = TestFile.ReadTestFile("event/event-click-menu.xml.txt");
        var o = new ClickMenuEventReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "event",
            Event = "CLICK",
            EventKey = "EVENTKEY"
        };

        var result = WeixinXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = TestFile.ReadTestFile("event/event-click-menu.xml.txt");
        var o = new ClickMenuEventReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "event",
            Event = "CLICK",
            EventKey = "EVENTKEY"
        };

        var result = WeixinXmlConvert.DeserializeObject<ClickMenuEventReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.Event, result.Event);
        Assert.Equal(o.EventKey, result.EventKey);
        Assert.Equal(o.EventKey, result.EventKeyAsMenuItemKey());
    }
}