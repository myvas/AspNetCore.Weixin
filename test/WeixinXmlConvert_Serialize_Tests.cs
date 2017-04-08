using AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace test
{
    public class WeixinXmlConvert_Serialize_Tests
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

            var o = new ResponseMessageText
            {
                ToUserName = "toUser",
                FromUserName = "fromUser",
                CreateTime = WeixinTimestampHelper.ToLocalTime(1490872329),
                Content = "你好"
            };

            var result = XmlConvert.SerializeObject(o);

            var deserializedExcepted = XmlConvert.DeserializeObject<ResponseMessageText>(excepted);
            var reserializedExcepted = XmlConvert.SerializeObject(deserializedExcepted);
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

            var o = new ResponseMessageNews
            {
                ToUserName = "toUser",
                FromUserName = "fromUser",
                CreateTime = WeixinTimestampHelper.ToLocalTime(1490872329),
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

            var result = XmlConvert.SerializeObject(o);

            var deserializedExcepted = XmlConvert.DeserializeObject<ResponseMessageNews>(excepted);
            var reserializedExcepted = XmlConvert.SerializeObject(deserializedExcepted);
            Assert.Equal(reserializedExcepted, result);
        }

    }
}
