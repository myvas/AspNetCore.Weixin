using System;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

public partial class WeixinMenuJson
{
    public partial class Button
    {
        /// <summary>
        /// A button with type of "voice".
        /// </summary>
        public class Voice : Button
        {
            /// <summary>
            /// voice
            /// </summary>
            [JsonPropertyName("type")]
            public string ItemType { get; } = "voice";

            /// <summary>
            /// media_id
            /// </summary>
            [JsonPropertyName("value")]
            public string Value { get; set; } = string.Empty;
        }
    }
}