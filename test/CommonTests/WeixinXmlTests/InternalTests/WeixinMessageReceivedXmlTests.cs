using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.WeixinXmlTests.InternalTests;

public class WeixinMessageReceivedXmlTests
{
    [Fact]
    public void Serialize()
    {
        string s = @"<xml><ToUserName></ToUserName><FromUserName></FromUserName><CreateTime>0</CreateTime><MsgType></MsgType><MsgId>0</MsgId></xml>";
        var o = new MessageReceivedXml
        {
            ToUserName = "",
            FromUserName = "",
            CreateTime = 0,
            MsgType = "",
            MsgId = 0
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        Assert.Equal(s, result);
    }

    [Fact]
    public void SerializeSimple()
    {
        string s = @"<xml><ToUserName>To</ToUserName><FromUserName>From</FromUserName><CreateTime>1490872329</CreateTime><MsgType>text</MsgType><MsgId>6403247895999455936</MsgId></xml>";
        var o = new MessageReceivedXml
        {
            ToUserName = "To",
            FromUserName = "From",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        Assert.Equal(s, result);
    }

    [Fact]
    public void Deserialize()
    {
        string s = @"<xml><ToUserName></ToUserName><FromUserName></FromUserName><CreateTime>0</CreateTime><MsgType></MsgType><MsgId>0</MsgId></xml>";
        var o = new MessageReceivedXml
        {
            ToUserName = "",
            FromUserName = "",
            CreateTime = 0,
            MsgType = "",
            MsgId = 0
        };

        var result = MyvasXmlConvert.DeserializeObject<MessageReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.MsgId, result.MsgId);
    }

    [Fact]
    public void DeserializeSimple()
    {
        string s = @"<xml><ToUserName>To</ToUserName><FromUserName>From</FromUserName><CreateTime>1490872329</CreateTime><MsgType>text</MsgType><MsgId>6403247895999455936</MsgId></xml>";
        var o = new MessageReceivedXml
        {
            ToUserName = "To",
            FromUserName = "From",
            CreateTime = 1490872329,
            MsgType = "text",
            MsgId = 6403247895999455936
        };

        var result = MyvasXmlConvert.DeserializeObject<MessageReceivedXml>(s);
        Assert.Equal(o.FromUserName, result.FromUserName);
        Assert.Equal(o.ToUserName, result.ToUserName);
        Assert.Equal(o.CreateTime, result.CreateTime);
        Assert.Equal(o.MsgType, result.MsgType);
        Assert.Equal(o.MsgId, result.MsgId);
    }
}