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
    /// Gets the <see cref="CreateTime"/> cast from the WeixinTimestamp.
    /// </summary>
    /// <returns>The <see cref="DateTime"/>.</returns>
    //[XmlIgnore]
    //[NotMapped]
    public DateTime? GetCreateTime()
    {
        try
        {
            return WeixinTimestampHelper.ToLocalTime(CreateTime);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Set the WeixinTimestamp from the <see cref="DateTime"/>.
    /// </summary>
    /// <param name="value">The WeixinTimestamp.</param>
    /// <returns></returns>
    public long SetCreateTime(DateTime? value)
    {
        if (value.HasValue)
            CreateTime = WeixinTimestampHelper.FromLocalTime(value.Value);
        else
            CreateTime = 0;

        return CreateTime;
    }

    /// <summary>
    /// 消息类型
    /// </summary>
    [XmlElement("MsgType")]
    [Required]
    public string MsgType { get; set; }

    /// <summary>
    /// Gets the <see cref="RequestMsgType"/> parse from the <see cref="MsgType"/>.
    /// </summary>
    /// <returns>The <see cref="RequestMsgType"/>.</returns>
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

    /// <summary>
    /// Sets the <see cref="MsgType"/> with the <see cref="RequestMsgType"/>.
    /// </summary>
    /// <returns></returns>
    public string SetMsgType(RequestMsgType value)
    {
        MsgType = value.ToString();
        return MsgType;
    }
}
