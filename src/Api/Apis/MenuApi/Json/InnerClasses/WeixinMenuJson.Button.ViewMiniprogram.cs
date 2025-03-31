using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 2+1.miniprogram：点击后打开指定小程序。
        /// </summary>
        /// <remarks>
        /// 用户点击miniprogram类型按钮后，微信客户端将会打开开发者在按钮中指定的小程序。
        /// </remarks>
        public class ViewMiniprogram : Button
        {
            /// <summary>
            /// 2.miniprogram
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "miniprogram";

            /// <summary>
            /// 网址。e.g. "http://mp.weixin.qq.com"
            /// </summary>
            [JsonPropertyName("url")]
            public string Url { get; set; } = string.Empty;

            /// <summary>
            /// 小程序APPID。 e.g. "wx223456789abcde"
            /// </summary>
            [JsonPropertyName("appid")]
            public string AppId { get; set; } = string.Empty;

            /// <summary>
            /// 小程序内页面路由。e.g. "pages/lunar/index"
            /// </summary>
            [JsonPropertyName("pagepath")]
            public string PagePath { get; set; } = string.Empty;
        }
    }
}