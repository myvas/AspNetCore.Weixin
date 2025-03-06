using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinSitePostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions>
        where TOptions : WeixinSiteOptions, new()
    {
        public void PostConfigure(string name, TOptions options)
        {
            if (options.Backchannel == null)
            {
                options.Backchannel = new HttpClient(new HttpClientHandler());
            }
        }
    }
}
