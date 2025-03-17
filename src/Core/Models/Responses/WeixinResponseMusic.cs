using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

[XmlRoot("xml", Namespace = "")]
public class WeixinResponseMusic : WeixinResponse, IWeixinResponse
{
    public WeixinResponseMusic()
    {
        MsgType = ResponseMsgType.music;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string MusicUrl { get; set; }
    [XmlElement("HQMusicUrl")]
    public string HQMusicUrl { get; set; }
    public string ThumbMediaId { get; set; }


    public override string ToXml()
    {
        var data = new
        {
            ToUserName,
            FromUserName,
            CreateTime = CreateTimestamp,
            MsgType = MsgTypeText,
            MUSIC = new
            {
                Title,
                Description,
                MusicUrl,
                HQMusicUrl,
                ThumbMediaId
            }
        };
        return WeixinXmlConvert.SerializeObject(data);
    }
}
