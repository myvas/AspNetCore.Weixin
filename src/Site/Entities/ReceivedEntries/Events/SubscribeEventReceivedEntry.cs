using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 订阅事件。或，用户扫描带参数（场景值）二维码（扫描前未关注）。
/// </summary>
/// <remarks>https://developers.weixin.qq.com/doc/offiaccount/Message_Management/Receiving_event_pushes.html</remarks>
[XmlRoot("xml", Namespace = "")]
public class SubscribeEventReceivedEntry : EventReceivedEntry
{
    #region 用户扫描带参数（场景值）二维码（扫描前未关注）
    /// <summary>
    /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
    /// </summary>
    public string EventKey { get; set; }

    /// <summary>
    /// 二维码的ticket，可用来换取二维码图片
    /// </summary>
    public string Ticket { get; set; }

    /// <summary>
    /// <see cref="EventKey"/> 中的固定前缀，用于分离二维码的参数值
    /// </summary>
    [XmlIgnore]
    [NotMapped]
    public const string EventKeyPrefix = "qrscene_";

    /// <summary>
    /// 从 <see cref="EventKey"/> 中分离出来的二维码的参数值
    /// </summary>
    //[XmlIgnore]
    //[NotMapped]
    public string GetScene()
    {
        if (EventKey?.StartsWith(EventKeyPrefix) == true)
            return EventKey?.Substring(EventKeyPrefix.Length);
        else
            return EventKey;
    }

    /// <summary>
    /// Sets the <see cref="EventKey"/>.
    /// </summary>
    /// <param name="value">The scene.</param>
    //[XmlIgnore]
    //[NotMapped]
    public string SetScene(string value)
    {
        if (value?.StartsWith(EventKeyPrefix) == true)
            EventKey = value;
        else
            EventKey = EventKeyPrefix + value;

        return EventKey;
    }
    #endregion
}
