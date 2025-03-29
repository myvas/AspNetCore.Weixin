using System;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// The base class of Weixin request messages, including (uplink) and response (downlink) messages
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class WeixinMessage
{
    /// <summary>
    /// To whom this message will be approaching.
    /// </summary>
    public string ToUserName { get; set; }

    /// <summary>
    /// By who this message was created.
    /// </summary>
    public string FromUserName { get; set; }

/// <summary>
/// The unix time when this message was created.
/// </summary>
    [XmlElement("CreateTime", Namespace = "")]
    public long CreateTimestamp { get; set; }
    [XmlIgnore]
    public DateTime CreateTime
    {
        get => WeixinTimestampHelper.ToLocalTime(CreateTimestamp);
        set => CreateTimestamp = WeixinTimestampHelper.FromLocalTime(value);
    }

    public WeixinMessage()
    {
        CreateTime = DateTime.Now;
    }
}
