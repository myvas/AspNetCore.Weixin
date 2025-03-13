using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests;

public class WeixinLocationEventReceivedXmlTests
{
    [Fact]
    public void Serialize()
    {
        string s = TestFile.ReadTestFile("event/event-location.xml.txt");
        var o = new LocationEventReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "event",
            Event = "LOCATION",
            Latitude = 23.137466m,
            Longitude = 113.352425m,
            Precision = 119.385040m
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = TestFile.ReadTestFile("event/event-location.xml.txt");
        var o = new LocationEventReceivedXml
        {
            ToUserName = "gh_712b448adf85",
            FromUserName = "oI3UkuL9uZxTOuj--HHMSMTlO3ks",
            CreateTime = 1490872329,
            MsgType = "event",
            Event = "LOCATION",
            Latitude = 23.137466m,
            Longitude = 113.352425m,
            Precision = 119.385040m
        };

        var result = MyvasXmlConvert.DeserializeObject<LocationEventReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.Event, result.Event);
        Assert.Equal(o.Latitude, result.Latitude);
        Assert.Equal(o.Longitude, result.Longitude);
        Assert.Equal(o.Precision, result.Precision);
    }
}