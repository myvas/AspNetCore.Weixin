using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// A button with type of "img".
        /// </summary>
        public class Image : Button
        {
            [JsonPropertyName("type")]
            public string ItemType { get; } = "img";

            /// <summary>
            /// media_id
            /// </summary>
            [JsonPropertyName("value")]
            public string Value { get; set; } = string.Empty;
        }
    }
}