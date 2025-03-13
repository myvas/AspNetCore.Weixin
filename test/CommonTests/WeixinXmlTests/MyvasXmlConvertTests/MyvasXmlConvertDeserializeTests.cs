using Xunit;
using System;
using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin.CommonTests.MyvasXmlConvertTests;

public class MyvasXmlConvertDeserializeTests
{
    [Fact]
    public void ReceivedXmlDeserializeShouldSuccess()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[中文字符]]></Content>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<ReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
    }

    [Fact]
    public void MessageReceivedXmlDeserializeShouldSuccess()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[中文字符]]></Content>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<MessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal(6403247895999455936, result.MsgId);
    }

    [Fact]
    public void TextMessageReceivedEventArgs()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[中文字符]]></Content>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<TextMessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal("中文字符", result.Content);
        Assert.Equal(6403247895999455936, result.MsgId);
    }

    [Fact]
    public void ImageMessageReceivedEventArgs()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<PicUrl><![CDATA[this is a url]]></PicUrl>
<MediaId><![CDATA[media_id]]></MediaId>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<ImageMessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal("this is a url", result.PicUrl);
        Assert.Equal("media_id", result.MediaId);
        Assert.Equal(6403247895999455936, result.MsgId);
    }


    [Fact]
    public void VoiceMessageReceivedEventArgs()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<MediaId><![CDATA[media_id]]></MediaId>
<Format><![CDATA[Format]]></Format>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<VoiceMessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal("media_id", result.MediaId);
        Assert.Equal("Format", result.Format);
        Assert.Equal(6403247895999455936, result.MsgId);
    }

    [Fact]
    public void VoiceMessageReceivedEventArgs_WithRecognition()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<MediaId><![CDATA[media_id]]></MediaId>
<Format><![CDATA[Format]]></Format>
<Recognition><![CDATA[腾讯微信团队]]></Recognition>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<VoiceMessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal("media_id", result.MediaId);
        Assert.Equal("Format", result.Format);
        Assert.Equal("腾讯微信团队", result.Recognition);
        Assert.Equal(6403247895999455936, result.MsgId);
    }

    [Fact]
    public void VideoMessageReceivedEventArgs()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<MediaId><![CDATA[media_id]]></MediaId>
<ThumbMediaId><![CDATA[thumb_media_id]]></ThumbMediaId>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<VideoMessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal("media_id", result.MediaId);
        Assert.Equal("thumb_media_id", result.ThumbMediaId);
        Assert.Equal(6403247895999455936, result.MsgId);
    }


    [Fact]
    public void ShortVideoMessageReceivedEventArgs()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<MediaId><![CDATA[media_id]]></MediaId>
<ThumbMediaId><![CDATA[thumb_media_id]]></ThumbMediaId>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<ShortVideoMessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal("media_id", result.MediaId);
        Assert.Equal("thumb_media_id", result.ThumbMediaId);
        Assert.Equal(6403247895999455936, result.MsgId);
    }

    [Fact]
    public void LocationMessageReceivedEventArgs()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Location_X>23.134521</Location_X>
<Location_Y>113.358803</Location_Y>
<Scale>20</Scale>
<Label><![CDATA[位置信息]]></Label>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<LocationMessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal((decimal)23.134521, result.Latitude);
        Assert.Equal((decimal)113.358803, result.Longitude);
        Assert.Equal(20, result.Scale);
        Assert.Equal("位置信息", result.Label);
        Assert.Equal(6403247895999455936, result.MsgId);
    }


    [Fact]
    public void LinkMessageReceivedEventArgs()
    {
        string s = @"<xml>
  <ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Title><![CDATA[我的官网]]></Title>
<Description><![CDATA[公众平台官网链接]]></Description>
<Url><![CDATA[http://url.com/q?something]]></Url>
<MsgId>6403247895999455936</MsgId>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<LinkMessageReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.text, result.MsgTypeObject());
        Assert.Equal("我的官网", result.Title);
        Assert.Equal("公众平台官网链接", result.Description);
        Assert.Equal("http://url.com/q?something", result.Url);
        Assert.Equal(6403247895999455936, result.MsgId);
    }

    [Fact]
    public void SubscribeEventReceivedEventArgs()
    {
        string s = @"<xml><ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[subscribe]]></Event>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<SubscribeEventReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.@event, result.MsgTypeObject());
        Assert.Equal(RequestEventType.subscribe, result.Event);
    }

    [Fact]
    public void SubscribeEventReceivedEventArgsWithScene()
    {
        string s = @"<xml><ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[subscribe]]></Event>
<EventKey><![CDATA[qrscene_123123]]></EventKey>
<Ticket><![CDATA[gQE28TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyUkhQZWgzelI4QVUxMDAwMGcwN3kAAgR6ittYAwQAAAAA]]></Ticket>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<SubscribeEventReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.@event, result.MsgTypeObject());
        Assert.Equal(RequestEventType.subscribe, result.Event);
        Assert.Equal("qrscene_123123", result.EventKey);
        Assert.Equal("123123", result.Scene);
        Assert.Equal("gQE28TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyUkhQZWgzelI4QVUxMDAwMGcwN3kAAgR6ittYAwQAAAAA", result.Ticket);
    }

    [Fact]
    public void QrscanEventReceivedEventArgs()
    {
        string s = @"<xml><ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[SCAN]]></Event>
<EventKey><![CDATA[123123]]></EventKey>
<Ticket><![CDATA[gQE28TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyUkhQZWgzelI4QVUxMDAwMGcwN3kAAgR6ittYAwQAAAAA]]></Ticket>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<QrscanEventReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.@event, result.MsgTypeObject());
        Assert.Equal(RequestEventType.SCAN, result.Event);
        Assert.Equal("123123", result.EventKey);
        Assert.Equal("123123", result.Scene);
        Assert.Equal("gQE28TwAAAAAAAAAAS5odHRwOi8vd2VpeGluLnFxLmNvbS9xLzAyUkhQZWgzelI4QVUxMDAwMGcwN3kAAgR6ittYAwQAAAAA", result.Ticket);
    }

    [Fact]
    public void LocationEventReceivedEventArgs()
    {
        string s = @"<xml><ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[LOCATION]]></Event>
<Latitude>23.137466</Latitude>
<Longitude>113.352425</Longitude>
<Precision>119.385040</Precision>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<LocationEventReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.@event, result.MsgTypeObject());
        Assert.Equal(RequestEventType.LOCATION, result.Event);
        Assert.Equal((decimal)23.137466, result.Latitude);
        Assert.Equal((decimal)113.352425, result.Longitude);
        Assert.Equal((decimal)119.385040, result.Precision);
    }

    [Fact]
    public void ClickMenuEventReceivedEventArgs()
    {
        string s = @"<xml><ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[CLICK]]></Event>
<EventKey><![CDATA[EVENTKEY]]></EventKey>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<ClickMenuEventReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.@event, result.MsgTypeObject());
        Assert.Equal(RequestEventType.CLICK, result.Event);
        Assert.Equal("EVENTKEY", result.EventKey);
        Assert.Equal("EVENTKEY", result.MenuItemKey);
    }

    [Fact]
    public void ViewMenuEventReceivedEventArgs()
    {
        string s = @"<xml><ToUserName><![CDATA[gh_712b448adf85]]></ToUserName>
<FromUserName><![CDATA[oI3UkuL9uZxTOuj--HHMSMTlO3ks]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[event]]></MsgType>
<Event><![CDATA[VIEW]]></Event>
<EventKey><![CDATA[www.qq.com]]></EventKey>
</xml>";

        var result = MyvasXmlConvert.DeserializeObject<ViewMenuEventReceivedXml>(s);
        Assert.Equal("gh_712b448adf85", result.ToUserName);
        Assert.Equal("oI3UkuL9uZxTOuj--HHMSMTlO3ks", result.FromUserName);
        Assert.Equal(1490872329, result.CreateTime);
        Assert.Equal(RequestMsgType.@event, result.MsgTypeObject());
        Assert.Equal(RequestEventType.VIEW, result.Event);
        Assert.Equal("www.qq.com", result.EventKey);
        Assert.Equal("www.qq.com", result.Url);
    }
}
