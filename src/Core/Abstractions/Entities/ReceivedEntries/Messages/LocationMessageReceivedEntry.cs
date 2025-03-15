using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 收到地理位置消息
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class LocationMessageReceivedEntry : MessageReceivedEntry
{
    /// <summary>
    /// 地理位置纬度,Location_X
    /// <example>23.134521</example>
    /// </summary>
    [XmlElement("Location_X", Namespace = "")]
    public decimal Latitude { get; set; }

    /// <summary>
    /// 地理位置经度，Location_Y
    /// <example>113.358803</example>
    /// </summary>
    [XmlElement("Location_Y", Namespace = "")]
    public decimal Longitude { get; set; }

    /// <summary>
    /// 地图缩放大小
    /// <example>20</example>
    /// <remarks>注意：与Google Maps不一致</remarks>
    /// </summary>
    public decimal Scale { get; set; }

    /// <summary>
    /// 地理位置信息，地址标注
    /// </summary>
    public string Label { get; set; }
}
