using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到消息或事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ReceivedXml
{
    /// <summary>
    /// 开发者微信号
    /// </summary>
    public string ToUserName { get; set; }

    /// <summary>
    /// 发送方帐号（一个OpenID）
    /// </summary>
    public string FromUserName { get; set; }

    /// <summary>
    /// 消息创建时间
    /// </summary>
    [XmlElement("CreateTime")]
    public long CreateTime { get; set; }

    //[XmlIgnore]
    //[NotMapped]
    public DateTime GetCreateTime()
    {
        return WeixinTimestampHelper.ToLocalTime(CreateTime);
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    [XmlElement("MsgType")]
    public string MsgType { get; set; }

    //[XmlIgnore]
    //[NotMapped]
    public RequestMsgType GetMsgType()
    {
        try
        {
            return (RequestMsgType)Enum.Parse(typeof(RequestMsgType), MsgType, true);
        }
        catch
        {
            return RequestMsgType.Unknown;
        }
    }
}
