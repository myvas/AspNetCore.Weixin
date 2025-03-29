using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 5.pic_sysphoto：弹出系统相机拍照后发图给开发者。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信客户端将调起系统相机，完成拍照操作后，会将拍摄的相片发送给开发者，并推送事件给开发者，同时收起系统相机，随后可能会收到开发者下发的消息。
        /// </remarks>
        public class TakePhoto : Button
        {
            /// <summary>
            /// 5.pic_sysphoto
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "pic_sysphoto";

            /// <summary>
            /// 自定义键值。e.g. "rselfmenu_1_0"
            /// </summary>
            [JsonPropertyName("key")]
            public string Key { get; set; } = string.Empty;
        }
    }
}
