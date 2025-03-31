using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 9.media_id：下发永久素材（图片、音频、视频、图文消息）。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信服务器会将开发者填写的永久素材id对应的素材下发给用户，永久素材类型可以是图片、音频、视频 、图文消息。请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。
        /// </remarks>
        public class DownloadMedia : Button
        {
            /// <summary>
            /// 9.media_id
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "media_id";

            /// <summary>
            /// media_id 永久素材ID
            /// </summary>
            [JsonPropertyName("media_id")]
            public string MediaId { get; set; } = string.Empty;
        }
    }
}
