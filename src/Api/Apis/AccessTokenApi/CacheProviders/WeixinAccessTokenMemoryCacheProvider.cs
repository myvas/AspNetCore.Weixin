using Microsoft.Extensions.Caching.Memory;
using System;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin;

public class WeixinAccessTokenMemoryCacheProvider : WeixinMemoryCacheProvider<WeixinAccessTokenJson>
{
    public WeixinAccessTokenMemoryCacheProvider(IMemoryCache cache) : base(cache)
    {
    }
}
