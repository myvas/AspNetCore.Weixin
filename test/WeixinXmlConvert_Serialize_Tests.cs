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
<CreateTime>12345678</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[你好]]></Content>
</xml>";

            var o = new ResponseMessageText
            {
                ToUserName = "toUser",
                FromUserName = "fromUser",
                CreateTime = new DateTime(12345678),
                Content = "你好"
            };

            var result = WeixinXmlConvert.SerializeObject(o);
            //Assert.Equal(excepted, result);
        }

    }
}
