using System;
using Xunit;

namespace Myvas.AspNetCore.Weixin.CommonTests.MyvasXmlConvertTests;

public class MyvasXmlConvertTests
{
    [Fact]
    public void MyvasXmlConvert_Serialize_AnonymousObject()
    {
        var s = @"<xml><ToUserName>To</ToUserName><FromUserName>From</FromUserName><CreateTime>0</CreateTime><MsgType>image</MsgType><Image><MediaId>mediaidxxx</MediaId></Image></xml>";
        var data = new
        {
            xml = new
            {
                ToUserName = "To",
                FromUserName = "From",
                CreateTime = 0,
                MsgType = "image",
                Image = new
                {
                    MediaId = "mediaidxxx"
                }
            }
        };
        var result = MyvasXmlConvert.SerializeObject(data);
        Assert.Equal(s, result);
    }

    [Fact]
    public void WeixinResponseImage_ToXml()
    {
        var s = @"<xml><ToUserName>toxxx</ToUserName><FromUserName>fromxxx</FromUserName><CreateTime>0</CreateTime><MsgType>image</MsgType><Image><MediaId>mediaidxxx</MediaId></Image></xml>";
        var data = new WeixinResponseImage
        {
            ToUserName = "toxxx",
            FromUserName = "fromxxx",
            CreateTimestamp = 0,
            MediaId = "mediaidxxx"
        };
        var result = data.ToXml();
        Assert.Equal(s, result);
    }

    [Fact]
    public void MyvasXmlConvert_SerializeObject_WeixinResponseText()
    {
        var s = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[你好]]></Content>
</xml>";

        var o = new WeixinResponseText
        {
            ToUserName = "toUser",
            FromUserName = "fromUser",
            CreateTime = WeixinTimestampHelper.ToLocalTime(1490872329),
            Content = "你好"
        };

        var result = MyvasXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s2, result);
    }

    [Fact]
    public void MustSerializable_ResponseNewsMessage()
    {
        var s = @"<xml>
<ToUserName><![CDATA[toUser]]></ToUserName>
<FromUserName><![CDATA[fromUser]]></FromUserName>
<CreateTime>1490872329</CreateTime>
<MsgType><![CDATA[news]]></MsgType>
<ArticleCount>2</ArticleCount>
<Articles>
<item>
<Title><![CDATA[title1]]></Title>
<Description><![CDATA[description1]]></Description>
<PicUrl><![CDATA[picurl]]></PicUrl>
<Url><![CDATA[url]]></Url>
</item>
<item>
<Title><![CDATA[title]]></Title>
<Description><![CDATA[description]]></Description>
<PicUrl><![CDATA[picurl]]></PicUrl>
<Url><![CDATA[url]]></Url>
</item>
</Articles>
</xml>";

        var o = new WeixinResponseNews
        {
            ToUserName = "toUser",
            FromUserName = "fromUser",
            CreateTime = WeixinTimestampHelper.ToLocalTime(1490872329),
        };
        o.Articles.Add(new WeixinResponseNewsArticle
        {
            Title = "title1",
            Description = "description1",
            PicUrl = "picurl",
            Url = "url"
        });
        o.Articles.Add(new WeixinResponseNewsArticle
        {
            Title = "title",
            Description = "description",
            PicUrl = "picurl",
            Url = "url"
        });

        var result = MyvasXmlConvert.SerializeObject(o);
        var s2 = WeixinXmlStringNormalizer.Normalize(s);
        Assert.Equal(s, result);
    }
}
