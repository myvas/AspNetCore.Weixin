using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Myvas.AspNetCore.Weixin;

public class WeixinPostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions>
    where TOptions : WeixinOptions, new()
{
    public void PostConfigure(string name, TOptions options)
    {
        options.Backchannel ??= new HttpClient(new HttpClientHandler());
        options.WeixinApiServer ??= WeixinApiServers.Default;
    }
}
