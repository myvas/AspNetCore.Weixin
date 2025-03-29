using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;
    public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// 12.打开永久图文消息。类似<see cref="ViewLimited"(view_limited)，但不使用 media_id 而使用 article_id.
        /// </summary>
        public class ViewArticleByArticleId : Button
        {
            /// <summary>
            /// 12.article_view_limited
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "article_view_limited";

            /// <summary>
            /// article_id, e.g. ARTICLE_ID2。注意：与永久素材ID(media_id)不同。
            /// </summary>
            [JsonPropertyName("article_id")]
            public string ArticleId { get; set; } = string.Empty;
        }
    }
}