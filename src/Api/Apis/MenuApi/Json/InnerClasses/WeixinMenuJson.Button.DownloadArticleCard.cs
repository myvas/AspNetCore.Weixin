using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 11.article_id：下发卡片式图文消息。
        /// </summary>
        /// <remarks>
        /// 用户点击按钮后，微信客户端将会以卡片形式下发开发者在按钮中填写的图文消息。
        /// </remarks>
        public class DownloadArticleCard : Button
        {
            /// <summary>
            /// 11.article_id
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "article_id";

            /// <summary>
            /// article_id 图文消息ID。注意：与永久素材ID(media_id)不同。
            /// </summary>
            [JsonPropertyName("article_id")]
            public string  ArticleId { get; set; } = string.Empty;
        }
    }
}
