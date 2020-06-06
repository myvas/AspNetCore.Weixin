using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    [XmlRoot("xml", Namespace = "")]
    public class WeixinResponseMusic : WeixinResponse, IWeixinResponse
    {
        public Music Music { get; set; }
        public string ThumbMediaId { get; set; }

        public WeixinResponseMusic()
        {
            MsgType = ResponseMsgType.music;
            Music = new Music();
        }
    }
}
