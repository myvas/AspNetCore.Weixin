using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    public class MaterialCountJson
    {
        [JsonPropertyName("video_count")]
        public int VideoCount { get; set; }
        [JsonPropertyName("voice_count")]
        public int VoiceCount { get; set; }
        [JsonPropertyName("image_count")]
        public int ImageCount { get; set; }
        [JsonPropertyName("news_count")]
        public int NewsCount { get; set; }
    }
}
