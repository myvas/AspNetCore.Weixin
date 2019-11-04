using System;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin
{

    public static class MediaApiHelper
    {
        public static string ExtractFileExtFromDisposition(string disposition)
        {
            int pos1 = disposition.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
            int pos3 = disposition.LastIndexOf("\"", StringComparison.OrdinalIgnoreCase);
            string ext = disposition.Substring(pos1, pos3 - pos1);
            return ext;
        }



        /// <summary>
        ///  openid
        /// </summary>
        public static bool GetAllCounts(JsonDocument payload, out int voiceCount, out int videoCount, out int imageCount, out int newsCount)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            voiceCount = payload.RootElement.GetInt32("voice_count");
            videoCount = payload.RootElement.GetInt32("video_count");
            imageCount = payload.RootElement.GetInt32("image_count");
            newsCount = payload.RootElement.GetInt32("news_count");
            return true;
        }
    }
}
