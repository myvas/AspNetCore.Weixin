using Myvas.AspNetCore.Weixin;
using System;
using Xunit;

namespace Weixin.Tests.Functional
{
    public class MyvasXmlConvertSerializeTests
    {
        [Fact]
        public void AnonymousObjectSerializeShouldSuccess()
        {
            var data = new {
                xml = new
                {
                    ToUserName = "toxxx",
                    FromUserName = "fromxxx",
                    CreateTime = DateTime.Now.Ticks,
                    MsgType = "image",
                    Image = new
                    {
                        MediaId = "mediaidxxx"
                    }
                }
            };
            var result = Myvas.AspNetCore.Weixin.MyvasXmlConvert.SerializeObject(data);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("<xml>", result);
            Assert.Contains("</xml>", result);
            Assert.Contains("<ToUserName>", result);
            Assert.Contains("</ToUserName>", result);
            Assert.Contains("<Image>", result);
            Assert.Contains("</Image>", result);
            Assert.Contains("<MediaId>", result);
            Assert.Contains("</MediaId>", result);
            Assert.Contains("mediaidxxx", result);
        }

        [Fact]
        public void WeixinResponseImageSerializeShouldSuccess()
        {
            var data = new WeixinResponseImage
            {
                ToUserName = "toxxx",
                FromUserName = "fromxxx",
                CreateTimestamp = DateTime.Now.Ticks,
                MediaId = "mediaidxxx"
            };
            var result = data.ToXml();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Contains("<xml>", result);
            Assert.Contains("</xml>", result);
            Assert.Contains("<ToUserName>", result);
            Assert.Contains("</ToUserName>", result);
            Assert.Contains("<Image>", result);
            Assert.Contains("</Image>", result);
            Assert.Contains("<MediaId>", result);
            Assert.Contains("</MediaId>", result);
            Assert.Contains("mediaidxxx", result);
        }

        [Fact]
        public void MustSerializable_ResponseTextMessage()
        {
            string excepted = @"<xml>
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

            var deserializedExcepted = MyvasXmlConvert.DeserializeObject<WeixinResponseText>(excepted);
            var reserializedExcepted = MyvasXmlConvert.SerializeObject(deserializedExcepted);
            Assert.Equal(reserializedExcepted, result);
        }

        [Fact]
        public void MustSerializable_ResponseNewsMessage()
        {
            string excepted = @"<xml>
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

            var deserializedExcepted = MyvasXmlConvert.DeserializeObject<WeixinResponseNews>(excepted);
            var reserializedExcepted = MyvasXmlConvert.SerializeObject(deserializedExcepted);
            Assert.Equal(reserializedExcepted, result);
        }

    }
}
