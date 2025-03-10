using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin;

public class WeixinSitePostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions>
    where TOptions : WeixinSiteOptions, new()
{
    public void PostConfigure(string name, TOptions options)
    {
        if (options.Path == null || !options.Path.HasValue)
            options.Path = WeixinSiteOptionsDefaults.Path;
    }
}
