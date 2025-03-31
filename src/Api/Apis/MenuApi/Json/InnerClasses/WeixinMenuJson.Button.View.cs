using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 2.view：点击后跳转到指定网页
        /// </summary>
        /// <remarks>
        /// 用户点击view类型按钮后，微信客户端将会打开开发者在按钮中填写的网页URL，可与网页授权获取用户基本信息接口结合，获得用户基本信息。
        /// </remarks>
        public class View : Button
        {
            /// <summary>
            /// 2.view
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "view";

            /// <summary>
            /// 网址 eg. https://myvas.com/go/to/a/absolute/url
            /// </summary>
            [JsonPropertyName("url")]
            [MaxLength(1024, ErrorMessage = "网页链接不能超过1024个字节")]
            public string Url { get; set; } = string.Empty;
        }
    }
}