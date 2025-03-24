using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics;

namespace Myvas.AspNetCore.Weixin;

public static class BrowserDetector
{
    public static (bool IsMicroMessenger, string Version) DetectMicroMessenger(this HttpContext context)
    {
        const string MicroMessengerKey = "MicroMessenger/";

        string version = "";
        try
        {
            if (context?.Request?.Headers != null)
            {
                var userAgentHeader = context.Request.Headers["User-Agent"];
                if (userAgentHeader.Any())
                {
                    var userAgentHeader0 = userAgentHeader[0];
                    if (!string.IsNullOrEmpty(userAgentHeader0))
                    {
                        var microMessengerIndex = userAgentHeader0.IndexOf(MicroMessengerKey, StringComparison.OrdinalIgnoreCase);
                        if (microMessengerIndex >= 0)
                        {
                            var versionStart = microMessengerIndex + MicroMessengerKey.Length;
                            var remainingString = userAgentHeader0.AsSpan(versionStart);
                            var spaceIndex = remainingString.IndexOf(' ');
                            version = spaceIndex > -1
                                ? remainingString.Slice(0, spaceIndex).ToString()
                                : remainingString.ToString();
                            return (true, version);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error detecting MicroMessenger: {ex}");
        }

        return (false, "");
    }
}
