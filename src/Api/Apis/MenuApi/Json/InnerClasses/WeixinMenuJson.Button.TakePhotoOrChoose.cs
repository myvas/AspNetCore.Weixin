using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 6.pic_photo_or_album：弹出拍照或者相册后发图给开发者。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信客户端将弹出选择器供用户选择“拍照”或者“从手机相册选择”。用户选择后即走其他两种流程。
        /// </remarks>
        public class TakePhotoOrChoose : Button
        {
            /// <summary>
            /// 6.pic_photo_or_album
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "pic_photo_or_album";

            /// <summary>
            /// 自定义键值。e.g. "rselfmenu_1_1"
            /// </summary>
            [JsonPropertyName("key")]
            public string Key { get; set; } = string.Empty;
        }
    }
}
