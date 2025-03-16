using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace Myvas.AspNetCore.Weixin;

public class WeixinAccessTokenRedisCacheProvider : WeixinExpirationRedisCacheProvider<WeixinAccessTokenJson>
{
    public WeixinAccessTokenRedisCacheProvider(IDistributedCache cache) : base(cache)
    {
    }
}
