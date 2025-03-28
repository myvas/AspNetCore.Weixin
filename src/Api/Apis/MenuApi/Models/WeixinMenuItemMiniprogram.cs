using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 11.打开小程序
/// </summary>
public class WeixinMenuItemMiniprogram : WeixinMenuItem, IWeixinMenuItemHasUrl
{
    public WeixinMenuItemMiniprogram()
    {
        Type = WeixinMenuItemTypes.Miniprogram;
    }

    /// <summary>
    /// 小程序Url地址。eg. http://mp.weixin.qq.com
    /// </summary>
    /// <remarks>不支持小程序的老版本客户端将打开此Url</remarks>
    [JsonPropertyName("url")]
    public string Url { get; set; }

    /// <summary>
    /// 小程序AppId。eg. wx2**************a
    /// </summary>
    [JsonPropertyName("appid")]
    public string AppId { get; set; }

    /// <summary>
    /// 小程序页面相对路径。eg. pages/lunar/index
    /// </summary>
    [JsonPropertyName("pagepath")]
    public string PagePath { get; set; }
}
