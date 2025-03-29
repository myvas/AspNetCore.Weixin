using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 7.pic_weixin：弹出微信相册发图器，发图给开发者。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信客户端将调起微信相册，完成选择操作后，将选择的相片发送给开发者的服务器，并推送事件给开发者，同时收起相册，随后可能会收到开发者下发的消息
        /// </remarks>
        public class ChooseFromWeixinAlbum : Button
        {
            /// <summary>
            /// 7.pic_weixin
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "pic_weixin";

            /// <summary>
            /// 自定义键值。e.g. "rselfmenu_1_2"
            /// </summary>
            [JsonPropertyName("key")]
            public string Key { get; set; } = string.Empty;
        }
    }
}
