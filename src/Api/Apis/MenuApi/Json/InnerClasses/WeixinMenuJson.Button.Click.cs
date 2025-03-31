using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 1.click：点击后推送指定事件。
        /// </summary>
        /// <remarks>
        /// 用户点击click类型按钮后，微信服务器会通过消息接口推送消息类型为event的结构给开发者（参考消息接口指南），
        /// 并且带上按钮中开发者填写的key值，开发者可以通过自定义的key值与用户进行交互。
        /// </remarks>
        public class Click : Button
        {
            /// <summary>
            /// 1.click
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "click";

            /// <summary>
            /// 自定义事件 eg. V1001_LIKE
            /// </summary>
            [JsonPropertyName("key")]
            [MaxLength(128, ErrorMessage = "菜单KEY值不能超过128个字节")]
            public string Key { get; set; } = string.Empty;
        }
    }
}