﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
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
        public static bool GetAllCounts(JObject payload, out int voiceCount, out int videoCount, out int imageCount, out int newsCount)
        {
            if (payload == null)
            {
                throw new ArgumentNullException(nameof(payload));
            }
            voiceCount = payload.Value<int>("voice_count");
            videoCount = payload.Value<int>("video_count");
            imageCount = payload.Value<int>("image_count");
            newsCount = payload.Value<int>("news_count");
            return true;
        }
    }
}
