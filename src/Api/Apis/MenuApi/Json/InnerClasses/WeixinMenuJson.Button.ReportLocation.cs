using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 8.location_select：弹出地理位置选择器。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信客户端将调起地理位置选择工具，完成选择操作后，将选择的地理位置发送给开发者的服务器，同时收起位置选择工具，随后可能会收到开发者下发的消息。
        /// </remarks>
        public class ReportLocation : Button
        {
            /// <summary>
            /// 8.location_select
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "location_select";

            /// <summary>
            /// 自定义键值。e.g. "rselfmenu_2_0"
            /// </summary>
            [JsonPropertyName("key")]
            public string Key { get; set; } = string.Empty;
        }
    }
}
