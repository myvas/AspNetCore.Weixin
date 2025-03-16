using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin;

public class WeixinAccessTokenMemoryCacheProvider : WeixinExpirationMemoryCacheProvider<WeixinAccessTokenJson>
{
    public WeixinAccessTokenMemoryCacheProvider(IMemoryCache cache) : base(cache)
    {
    }
}
