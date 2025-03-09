using Microsoft.Extensions.Options;
using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinApiClient : ApiClient
{
    public WeixinOptions Options { get; }
    
    public WeixinApiClient(IOptions<WeixinOptions> optionsAccessor)
        : base(optionsAccessor?.Value?.Backchannel)
    {
        Options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
    }    
}
