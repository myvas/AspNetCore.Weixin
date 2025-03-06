using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Weixin.Tests.Functional
{
    public class WeixinXmlConvertSerializeTests
    {
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
                CreateTime = new UnixDateTime(1490872329),
                Content = "你好"
            };

            var result = WeixinXmlConvert.SerializeObject(o);

            var deserializedExcepted = WeixinXmlConvert.DeserializeObject<WeixinResponseText>(excepted);
            var reserializedExcepted = WeixinXmlConvert.SerializeObject(deserializedExcepted);
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
                CreateTime = new UnixDateTime(1490872329),
            };
            o.Articles.Add(new Article
            {
                Title = "title1",
                Description = "description1",
                PicUrl = "picurl",
                Url = "url"
            });
            o.Articles.Add(new Article
            {
                Title = "title",
                Description = "description",
                PicUrl = "picurl",
                Url = "url"
            });

            var result = WeixinXmlConvert.SerializeObject(o);

            var deserializedExcepted = WeixinXmlConvert.DeserializeObject<WeixinResponseNews>(excepted);
            var reserializedExcepted = WeixinXmlConvert.SerializeObject(deserializedExcepted);
            Assert.Equal(reserializedExcepted, result);
        }

    }
}
