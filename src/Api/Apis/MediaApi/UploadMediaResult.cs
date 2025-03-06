using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 上传媒体文件返回结果
    /// </summary>
    public class UploadMediaResult
    {
        [JsonPropertyName("type")]
        public MediaType UploadMediaType { get; set; }

        [JsonPropertyName("media_id")]
        public string MediaId { get; set; }

        [JsonPropertyName("created_at")]
        public long CreatedAtUnixTime { get; set; }
    }
}
