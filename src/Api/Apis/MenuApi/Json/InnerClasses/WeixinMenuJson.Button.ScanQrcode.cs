using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 3.scancode_push：扫码并推送事件给开发者。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后显示扫描结果（如果是URL，将进入URL），且会将扫码的结果传给开发者，开发者可以下发消息。
        /// </remarks>
        public class ScanQrcode : Button
        {
            /// <summary>
            /// 3.scancode_push
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "scancode_push";

            /// <summary>
            /// 自定义键值。e.g. "rselfmenu_0_0"
            /// </summary>
            [JsonPropertyName("key")]
            public string Key { get; set; } = string.Empty;
        }
    }
}
