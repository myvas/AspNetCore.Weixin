using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 7.弹出微信相册发图
/// <para>用户点击按钮后，微信客户端将调起微信相册，
/// 完成选择操作后，将选择的相片发送给开发者的服务器，
/// 并推送事件给开发者，同时收起相册，随后可能会收到开发者下发的消息。</para>
/// </summary>
public class WeixinMenuItemPicFromWeixinAlbum : WeixinMenuItem, IWeixinMenuItemHasKey
{
    public WeixinMenuItemPicFromWeixinAlbum()
    {
        Type = WeixinMenuItemTypes.PicFromWeixinAlbum;
    }

    /// <summary>
    /// 自定义事件码
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; set; }
}
