using System;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin;

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
        voiceCount = payload.RootElement.GetProperty("voice_count").GetInt32();
        videoCount = payload.RootElement.GetProperty("video_count").GetInt32();
        imageCount = payload.RootElement.GetProperty("image_count").GetInt32();
        newsCount = payload.RootElement.GetProperty("news_count").GetInt32();
        return true;
    }
}
