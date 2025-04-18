﻿namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 标记大小
/// </summary>
public enum GoogleMapMarkerSize
{
    Default = mid,
    tiny = 0, mid = 1, small = 2
}

public class GoogleMapMarkers
{
    /// <summary>
    /// （可选）指定集合 {tiny, mid, small} 中的标记大小。如果未设置 size 参数，标记将以其默认（常规）大小显示。
    /// </summary>
    public GoogleMapMarkerSize Size { get; set; }
    /// <summary>
    /// （可选）指定 24 位颜色（例如 color=0xFFFFCC）或集合 {black, brown, green, purple, yellow, blue, gray, orange, red, white} 中预定义的一种颜色。
    /// </summary>
    public string Color { get; set; }
    /// <summary>
    /// （可选）指定集合 {A-Z, 0-9} 中的一个大写字母数字字符。
    /// </summary>
    public string Label { get; set; }
    /// <summary>
    /// 经度longitude
    /// </summary>
    public decimal Latitude { get; set; }
    /// <summary>
    /// 纬度latitude
    /// </summary>
    public decimal Longitude { get; set; }
}
