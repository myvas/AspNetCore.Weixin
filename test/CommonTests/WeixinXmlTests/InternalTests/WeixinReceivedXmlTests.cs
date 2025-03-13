using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests.InternalTests;

public class WeixinReceivedXmlTests
{
    [Fact]
    public void Serialize()
    {
        string s = @"<xml><ToUserName></ToUserName><FromUserName></FromUserName><CreateTime>0</CreateTime><MsgType></MsgType></xml>";
        var o = new ReceivedXml
        {
            ToUserName = "",
            FromUserName = "",
            CreateTime = 0,
            MsgType = ""
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        Assert.Equal(s, result);
    }

    [Fact]
    public void SerializeSimple()
    {
        string s = @"<xml><ToUserName>To</ToUserName><FromUserName>From</FromUserName><CreateTime>1490872329</CreateTime><MsgType>text</MsgType></xml>";
        var o = new ReceivedXml
        {
            ToUserName = "To",
            FromUserName = "From",
            CreateTime = 1490872329,
            MsgType = "text"
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        Assert.Equal(s, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = @"<xml><ToUserName></ToUserName><FromUserName></FromUserName><CreateTime>0</CreateTime><MsgType></MsgType></xml>";
        var o = new ReceivedXml
        {
            ToUserName = "",
            FromUserName = "",
            CreateTime = 0,
            MsgType = ""
        };

        var result = MyvasXmlConvert.DeserializeObject<ReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
    }

    [Fact]
    public void DeserializeSimple()
    {
        string s = @"<xml><ToUserName>To</ToUserName><FromUserName>From</FromUserName><CreateTime>1490872329</CreateTime><MsgType>text</MsgType></xml>";
        var o = new ReceivedXml
        {
            ToUserName = "To",
            FromUserName = "From",
            CreateTime = 1490872329,
            MsgType = "text"
        };

        var result = MyvasXmlConvert.DeserializeObject<ReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
    }
}