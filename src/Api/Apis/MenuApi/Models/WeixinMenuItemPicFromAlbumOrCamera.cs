using Newtonsoft.Json;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 6.弹出拍照或者相册发图
/// <para>用户点击按钮后，微信客户端将弹出选择器供用户选择“拍照”或者“从手机相册选择”。
/// 用户选择后即走其他两种流程。</para>
/// </summary>
public class WeixinMenuItemPicFromAlbumOrCamera : WeixinMenuItem, IWeixinMenuItemHasKey
{
    public WeixinMenuItemPicFromAlbumOrCamera()
    {
        Type = WeixinMenuItemTypes.PicFromAlbumOrCamera;
    }

    /// <summary>
    /// 自定义事件码
    /// </summary>
    [JsonProperty("key")]
    public string Key { get; set; }
}
