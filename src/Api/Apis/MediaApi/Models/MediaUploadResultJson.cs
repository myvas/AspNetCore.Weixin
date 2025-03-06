using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 上传媒体文件返回结果
    /// </summary>
    public class MediaUploadResultJson : WeixinErrorJson
    {

        [JsonPropertyName("type")]
        public UploadMediaType type;

        [JsonPropertyName("media_id")]
        public string MediaId { get; set; }

        [JsonPropertyName("created_at")]
        public long CreatedAtUnixTime { get; set; }
    }
}
