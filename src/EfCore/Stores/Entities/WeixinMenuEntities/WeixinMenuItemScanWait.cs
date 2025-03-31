using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

/// <summary>
/// 4.扫码推事件且弹出“消息接收中”提示框
/// <para>用户点击按钮后，微信客户端将调起扫一扫工具，
/// 完成扫码操作后，将扫码的结果传给开发者，同时收起扫一扫工具，
/// 然后弹出“消息接收中”提示框，随后可能会收到开发者下发的消息。</para>
/// </summary>
public class WeixinMenuItemScanWait : WeixinMenuEntityItem, IWeixinMenuEntityItemHasKey
{
    public WeixinMenuItemScanWait()
    {
        Type = WeixinMenuItemTypes.ScanWait;
    }

    /// <summary>
    /// 自定义事件码
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; }
}
