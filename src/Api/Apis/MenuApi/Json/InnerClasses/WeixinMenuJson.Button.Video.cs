using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// A button with type of "video".
        /// </summary>
        public class Video : Button
        {
            [JsonPropertyName("type")]
            public string ItemType { get; } = "video";

            /// <summary>
            /// http://url
            /// </summary>
            [JsonPropertyName("value")]
            public string Value { get; set; } = string.Empty;
        }
    }
}