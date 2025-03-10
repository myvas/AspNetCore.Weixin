namespace Myvas.AspNetCore.Weixin;

public static class WeixinOptionsExtension
{
    public static string BuildWeixinApiUrl(this WeixinOptions options, string pathAndQuery)
        => new WeixinApiEndpoint(options.WeixinApiServer, pathAndQuery).ToString();

    public static string BuildWeixinFileApiUrl(this WeixinOptions options, string pathAndQuery)
        => new WeixinApiEndpoint(WeixinApiServers.File, pathAndQuery).ToString();

    public static string BuildWeixinPlatformUrl(this WeixinOptions options, string pathAndQuery)
        => new WeixinApiEndpoint(WeixinApiServers.MP, pathAndQuery).ToString();
}
