using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

/// <summary>
/// 5.弹出系统拍照发图
/// <para>用户点击按钮后，微信客户端将调起系统相机，
/// 完成拍照操作后，会将拍摄的相片发送给开发者，
/// 并推送事件给开发者，同时收起系统相机，随后可能会收到开发者下发的消息。</para>
/// </summary>
public class WeixinMenuItemPicFromCameraOnly : WeixinMenuEntityItem, IWeixinMenuEntityItemHasKey
{
    public WeixinMenuItemPicFromCameraOnly()
    {
        Type = WeixinMenuItemTypes.PicFromCameraOnly;
    }

    /// <summary>
    /// 自定义事件码
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; }
}
