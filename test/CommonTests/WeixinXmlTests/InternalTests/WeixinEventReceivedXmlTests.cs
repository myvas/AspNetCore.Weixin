using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests.InternalTests;

public class WeixinEventReceivedXmlTests
{
    [Fact]
    public void Serialize()
    {
        string s = @"<xml><ToUserName></ToUserName><FromUserName></FromUserName><CreateTime>0</CreateTime><MsgType></MsgType><Event></Event></xml>";
        var o = new EventReceivedXml
        {
            ToUserName = "",
            FromUserName = "",
            CreateTime = 0,
            MsgType = "",
            Event = ""
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        Assert.Equal(s, result);
    }

    [Fact]
    public void SerializeSimple()
    {
        string s = @"<xml><ToUserName>To</ToUserName><FromUserName>From</FromUserName><CreateTime>1490872329</CreateTime><MsgType>text</MsgType><Event>LOCATION</Event></xml>";
        var o = new EventReceivedXml
        {
            ToUserName = "To",
            FromUserName = "From",
            CreateTime = 1490872329,
            MsgType = "text",
            Event = "LOCATION"
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        Assert.Equal(s, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = @"<xml><ToUserName></ToUserName><FromUserName></FromUserName><CreateTime>0</CreateTime><MsgType></MsgType><Event></Event></xml>";
        var o = new EventReceivedXml
        {
            ToUserName = "",
            FromUserName = "",
            CreateTime = 0,
            MsgType = "",
            Event = ""
        };

        var result = MyvasXmlConvert.DeserializeObject<EventReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.Event, result.Event);
    }

    [Fact]
    public void DeserializeSimple()
    {
        string s = @"<xml><ToUserName>To</ToUserName><FromUserName>From</FromUserName><CreateTime>1490872329</CreateTime><MsgType>text</MsgType><Event>LOCATION</Event></xml>";
        var o = new EventReceivedXml
        {
            ToUserName = "To",
            FromUserName = "From",
            CreateTime = 1490872329,
            MsgType = "text",
            Event = "LOCATION"
        };

        var result = MyvasXmlConvert.DeserializeObject<EventReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.Event, result.Event);
    }
}