using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

[XmlRoot("xml", Namespace = "")]
public class WeixinResponseImage : WeixinResponse, IWeixinResponse
{
    public WeixinResponseImage()
    {
        MsgType = ResponseMsgType.image;
    }

    public string MediaId { get; set; }


    public override string ToXml()
    {
        var data = new
        {
            ToUserName,
            FromUserName,
            CreateTime = CreateTimestamp,
            MsgType = MsgTypeText,
            Image = new
            {
                MediaId
            }
        };
        return WeixinXmlConvert.SerializeObject(data);
    }
}
