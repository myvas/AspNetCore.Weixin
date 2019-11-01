using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Myvas.AspNetCore.Weixin
{
    public class MaterialCountJson
    {
        [JsonProperty("video_count")]
        public int VideoCount { get; set; }
        [JsonProperty("voice_count")]
        public int VoiceCount { get; set; }
        [JsonProperty("image_count")]
        public int ImageCount { get; set; }
        [JsonProperty("news_count")]
        public int NewsCount { get; set; }
    }
}
