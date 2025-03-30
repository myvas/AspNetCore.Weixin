using System.Net;
using System.Net.Http;

namespace Myvas.AspNetCore.Weixin;

public static class HttpRequestExceptionStatusCodeExtensions
{
    public static int? GetStatusCode(this HttpRequestException httpEx)
    {
        if (httpEx == null) return null;

#if NET5_0_OR_GREATER
        return (int)(httpEx.StatusCode ?? HttpStatusCode.InternalServerError);
#else
        return (int)(httpEx.Data.Contains("StatusCode")
            ? (HttpStatusCode?)httpEx.Data["StatusCode"]
            : HttpStatusCode.InternalServerError);
#endif
    }
}