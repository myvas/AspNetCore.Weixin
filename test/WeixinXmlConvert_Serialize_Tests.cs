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

    }
}
