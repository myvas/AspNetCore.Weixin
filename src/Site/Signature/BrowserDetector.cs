using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;

namespace Myvas.AspNetCore.Weixin;

public static class BrowserDetector
{
    public static (bool IsMicroMessenger, string Version) DetectMicroMessenger(this HttpContext context)
    {
        var key = "MicroMessenger";

        bool isMicroMessenger = false;
        string version = "";
        try
        {
            var userAgentHeader = context.Request.Headers?.FirstOrDefault(x => x.Key.ToLower() == "User-Agent".ToLower()).Value;
            if (userAgentHeader.HasValue)
            {
                var userAgent = userAgentHeader.ToString().AsSpan();
                var microMessengerIndex = userAgent.IndexOf((key + "/").AsSpan());
                if (microMessengerIndex > -1)
                {
                    isMicroMessenger = true;

                    var versionIndexStart = microMessengerIndex + key.Length + 1;
                    var versionSlice = userAgent.Slice(versionIndexStart);
                    var versionIndexEnd = versionSlice.IndexOf(' ');
                    if (versionIndexEnd > -1)
                    {
                        version = versionSlice.Slice(0, versionIndexEnd).ToString();
                    }
                    else
                    {
                        version = versionSlice.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return (isMicroMessenger, version);
    }
}
