using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

/// <summary>
/// 1.点击推自定义事件
/// <para>用户点击click类型按钮后，微信服务器会通过消息接口推送消息类型为event的结构给开发者（参考消息接口指南），
/// 并且带上按钮中开发者填写的key值，开发者可以通过自定义的key值与用户进行交互；</para>
/// </summary>
public class WeixinMenuEntityItemClick : WeixinMenuEntityItem, IWeixinMenuEntityItemHasKey
{
    public WeixinMenuEntityItemClick()
    {
        Type = WeixinMenuItemTypes.Click;
    }

    /// <summary>
    /// 自定义事件 eg. V1001_LIKE
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; }
}
