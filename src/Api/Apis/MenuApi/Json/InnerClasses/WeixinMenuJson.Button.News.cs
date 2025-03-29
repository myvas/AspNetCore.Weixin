using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// A button with type of "news".
        /// </summary>
        public class News : Button
        {
            [JsonPropertyName("type")]
            public string ItemType { get; set; } = "news";

            [JsonPropertyName("url")]
            public string Url { get; set; }

            [JsonPropertyName("value")]
            public string Value { get; set; }

            [JsonPropertyName("news_info")]
            public NewsInfoClass NewsInfo { get; set; } = new();

            public class NewsInfoClass
            {
                [JsonPropertyName("list")]
                public List<ItemClass> Items { get; set; } = [];

                public class ItemClass
                {
                    [JsonPropertyName("title")]
                    public string Title { get; set; }

                    [JsonPropertyName("author")]
                    public string Author { get; set; }

                    [JsonPropertyName("digest")]
                    public string Digest { get; set; }

                    [JsonPropertyName("show_cover")]
                    public int ShowCover { get; set; }

                    [JsonPropertyName("cover_url")]
                    public string CoverUrl { get; set; }

                    [JsonPropertyName("content_url")]
                    public string ContentUrl { get; set; }

                    [JsonPropertyName("source_url")]
                    public string SourceUrl { get; set; }
                }
            }
        }
    }
}