using Microsoft.Extensions.Caching.Memory;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Manage their cached <see cref="WeixinJsapiTicketJson"> for the Weixin accounts specified by 'appId'
/// </summary>
public class WeixinJsapiTicketMemoryCacheProvider : WeixinExpirationMemoryCacheProvider<WeixinJsapiTicketJson>, IWeixinJsapiTicketCacheProvider
{
    public WeixinJsapiTicketMemoryCacheProvider(IMemoryCache cache) : base(cache)
    {
    }
}
