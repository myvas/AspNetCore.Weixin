using Microsoft.Extensions.Options;
using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinJssdkPostConfigureOptions : IPostConfigureOptions<WeixinJssdkOptions>
{
    private readonly WeixinOptions _options;

    public WeixinJssdkPostConfigureOptions(WeixinOptions options)
    {
        _options = options;
    }

    public void PostConfigure(string name, WeixinJssdkOptions options)
    {
        if(_options==null && options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if(options==null || string.IsNullOrWhiteSpace(options!.AppId))
        {
            options.AppId = _options.AppId;
        }
    }
}
