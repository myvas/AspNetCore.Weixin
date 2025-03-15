using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 上报地理位置事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class LocationEventReceivedEntry : EventReceivedEntry
{
    /// <summary>
    /// 纬度
    /// <example>23.134521</example>
    /// </summary>
    public decimal Latitude { get; set; }

    /// <summary>
    /// 经度
    /// <example>113.358803</example>
    /// </summary>
    public decimal Longitude { get; set; }

    /// <summary>
    /// 地理位置精度
    /// <example>40.000000</example>
    /// </summary>
    public decimal Precision { get; set; }
}
