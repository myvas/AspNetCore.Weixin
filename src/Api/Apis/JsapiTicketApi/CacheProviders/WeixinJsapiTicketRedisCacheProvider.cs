using Microsoft.Extensions.Caching.Distributed;

namespace Myvas.AspNetCore.Weixin;

public class WeixinJsapiTicketRedisCacheProvider: WeixinRedisCacheProvider<WeixinJsapiTicketJson>, IWeixinJsapiTicketCacheProvider
{
    public WeixinJsapiTicketRedisCacheProvider(IDistributedCache cache) : base(cache)
    {
    }
}
