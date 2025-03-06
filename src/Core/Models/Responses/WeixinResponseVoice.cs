using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 需要预先上传多媒体文件到微信服务器，只支持认证服务号。
    /// </summary>
    [XmlRoot("xml", Namespace = "")]
    public class WeixinResponseVoice : WeixinResponse, IWeixinResponse
    {
        public WeixinResponseVoice()
        {
            MsgType = ResponseMsgType.voice;
        }

        public string MediaId { get; set; }

        public override string ToXml()
        {
            var data = new
            {
                xml = new
                {
                    ToUserName,
                    FromUserName,
                    CreateTime = CreateTimestamp,
                    MsgType = MsgTypeText,
                    Voice = new
                    {
                        MediaId
                    }
                }
            };
            return MyvasXmlConvert.SerializeObject(data);
        }
    }
}
