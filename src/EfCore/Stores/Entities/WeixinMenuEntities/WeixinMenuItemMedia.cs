using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin.Entities;

/// <summary>
/// 9.下发消息（除文本消息）
/// <para>用户点击media_id类型按钮后，微信服务器会将开发者填写的永久素材id对应的素材下发给用户，
/// 永久素材类型可以是图片、音频、视频、图文消息。
/// 请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。</para>
/// </summary>
public class WeixinMenuItemMedia : WeixinMenuEntityItem
{
    public WeixinMenuItemMedia()
    {
        Type = WeixinMenuItemTypes.Media;
    }

    /// <summary>
    /// 永久素材Id。可以是图文消息、图片、视频、音频。须预先上传到微信公众号服务器上。
    /// </summary>
    [JsonPropertyName("media_id")]
    public string MediaId { get; set; }
}
