using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 10.view_limited：打开图文消息。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信客户端将打开开发者在按钮中填写的永久素材id对应的图文消息URL。永久素材类型只支持图文消息。请注意：永久素材id必须是在“素材管理/新增永久素材”接口上传后获得的合法id。
        /// </remarks>
        public class ViewArticleByMediaId : Button
        {
            /// <summary>
            /// 10.view_limited
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "view_limited";

            /// <summary>
            /// media_id （图文消息）永久素材ID
            /// </summary>
            [JsonPropertyName("media_id")]
            public string MediaId { get; set; } = string.Empty;
        }
    }
}
