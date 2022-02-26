using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    [XmlRoot("xml", Namespace = "")]
    public class WeixinResponseImage : WeixinResponse, IWeixinResponse
    {
        public Image Image { get; set; }

        public WeixinResponseImage()
        {
            MsgType = ResponseMsgType.image;
            Image = new Image();
        }
    }
}
