using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 4.scancode_waitmsg：扫码并推送事件给开发者，然后弹出“消息接收中”提示框等待开发者下发消息。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后，将扫码的结果传给开发者，同时收起扫一扫工具，然后弹出“消息接收中”提示框，随后可能会收到开发者下发的消息。
        /// </remarks>
        public class ScanQrcodeAndWait : Button
        {
            /// <summary>
            /// 4.scancode_waitmsg
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "scancode_waitmsg";

            /// <summary>
            /// 自定义键值。e.g. "rselfmenu_0_1"
            /// </summary>
            [JsonPropertyName("key")]
            public string Key { get; set; } = string.Empty;
        }
    }
}
