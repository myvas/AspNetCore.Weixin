using Microsoft.Extensions.Caching.Distributed;

namespace Myvas.AspNetCore.Weixin;

public class WeixinJsapiTicketRedisCacheProvider: WeixinExpirationRedisCacheProvider<WeixinJsapiTicketJson>, IWeixinJsapiTicketCacheProvider
{
    public WeixinJsapiTicketRedisCacheProvider(IDistributedCache cache) : base(cache)
    {
    }
}
