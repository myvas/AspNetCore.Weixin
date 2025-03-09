﻿using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 10.跳转图文消息URL
/// <para>用户点击view_limited类型按钮后，微信客户端将打开开发者在按钮中填写的永久素材id对应的图文消息URL，
/// 永久素材类型只支持图文消息。
/// 请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。​</para>
/// </summary>
public class WeixinMenuItemViewLimited : WeixinMenuItem
{
    public WeixinMenuItemViewLimited()
    {
        Type = WeixinMenuItemTypes.ViewLimited;
    }

    /// <summary>
    /// 永久素材Id。只能是图文消息。须预先上传到微信公众号服务器上。
    /// </summary>
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
}
