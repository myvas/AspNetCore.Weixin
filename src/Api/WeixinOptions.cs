using System.Net.Http;

namespace Myvas.AspNetCore.Weixin;

public class WeixinOptions
{
    /// <summary>
    /// Weixin:AppId
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// Weixin:AppSecret
    /// </summary>
    public string AppSecret { get; set; }

    /// <summary>
    /// Use a reusable <see cref="HttpClient"/> to communicate with the remote tencent server.
    /// </summary>
    /// <remarks>It is very useful for unit testing. If you don't care, please Keep it as it is.</remarks>
    public HttpClient Backchannel { get; set; }

    /// <summary>
    /// Choose as working server from dedicated servers in <see cref="WeixinApiServers"/>. The default value is <see cref="WeixinApiServers.Default"/>.
    /// </summary>
    public string WeixinApiServer { get; set; }
}
