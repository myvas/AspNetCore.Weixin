using System;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 响应回复消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public abstract class WeixinResponse : WeixinMessage, IWeixinResponse
{
    [XmlElement("MsgType", Namespace = "")]
    public string MsgTypeText { get; set; }

    [XmlIgnore]
    public ResponseMsgType MsgType
    {
        get => (ResponseMsgType)Enum.Parse(typeof(ResponseMsgType), MsgTypeText);
        set => MsgTypeText = value.ToString();
    }

    public virtual string ToXml()
    {
        return WeixinXmlConvert.SerializeObject(this);
    }
}
