﻿using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

/// <summary>
/// 3.扫码推事件。
/// <para>用户点击按钮后，微信客户端将调起扫一扫工具，
/// 完成扫码操作后显示扫描结果（如果是URL，将进入URL），
/// 且会将扫码的结果传给开发者，开发者可以下发消息。
/// </para>
/// </summary>
public class WeixinMenuItemScanPush : WeixinMenuEntityItem, IWeixinMenuEntityItemHasKey
{
    public WeixinMenuItemScanPush()
    {
        Type = WeixinMenuItemTypes.ScanPush;
    }

    /// <summary>
    /// 二维码解码后的内容
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; }
}
