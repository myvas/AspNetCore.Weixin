using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 收到消息或事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ReceivedEntry
{
    /// <summary>
    /// 
    /// </summary>
    public ReceivedEntry()
    {
        Id = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// PK
    /// </summary>
    [XmlIgnore]
    [Key]
    public string Id { get; set; }

    /// <summary>
    /// 开发者微信号
    /// </summary>
    [MaxLength(32)]
    [Required]
    public string ToUserName { get; set; }

    /// <summary>
    /// 发送方帐号（一个OpenID）
    /// </summary>
    [MaxLength(32)]
    [Required]
    public string FromUserName { get; set; }

    /// <summary>
    /// 消息创建时间（整型）
    /// </summary>
    [XmlElement("CreateTime")]
    public long CreateTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [XmlIgnore]
    [NotMapped]
    public DateTime CreateTimeObject
    {
        get
        {
            return WeixinTimestampHelper.ToLocalTime(CreateTime);
        }
        set
        {
            CreateTime = WeixinTimestampHelper.FromLocalTime(value);
        }
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    [XmlElement("MsgType")]
    [Required]
    public string MsgType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [XmlIgnore]
    [NotMapped]
    public RequestMsgType MsgTypeEnum
    {
        get
        {
            return (RequestMsgType)Enum.Parse(typeof(RequestMsgType), MsgType, true);
        }
        set
        {
            MsgType = MsgTypeEnum.ToString();
        }
    }
}
