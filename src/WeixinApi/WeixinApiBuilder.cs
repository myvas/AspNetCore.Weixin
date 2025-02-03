using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinApiBuilder
    {
        public WeixinApiBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; private set; }
    }
}
