using Newtonsoft.Json;
using System.Text.Json;

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
    [JsonProperty("url")]
    public string Url { get; set; }

    /// <summary>
    /// 小程序AppId。eg. wx286b93c14bbf93aa
    /// </summary>
    [JsonProperty("appid")]
    public string AppId { get; set; }

    /// <summary>
    /// 小程序页面相对路径。eg. pages/lunar/index
    /// </summary>
    [JsonProperty("pagepath")]
    public string PagePath { get; set; }
}
