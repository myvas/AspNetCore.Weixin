namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 执行的检测动作
/// </summary>
public static class WeixinCheckNetworkActions
{
    /// <summary>
    /// 做域名解析
    /// </summary>
    public const string Dns = "dns";
    /// <summary>
    /// 做ping检测
    /// </summary>
    public const string Ping = "ping";
    /// <summary>
    /// dns和ping都做
    /// </summary>
    public const string All = "all";
}
