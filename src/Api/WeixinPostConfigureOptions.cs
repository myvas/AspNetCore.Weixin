using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace Myvas.AspNetCore.Weixin;

public class WeixinPostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions>
    where TOptions : WeixinOptions, new()
{
    public void PostConfigure(string name, TOptions options)
    {
        // Validate necessary values
        if (string.IsNullOrEmpty(options.AppId)) throw new ArgumentNullException(nameof(options.AppId));
        
        // Fill in optional values with defaults
        options.Backchannel ??= new HttpClient(new HttpClientHandler());
        options.WeixinApiServer ??= WeixinApiServers.Default;
    }
}
