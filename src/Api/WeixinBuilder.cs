using Microsoft.Extensions.DependencyInjection;

namespace Myvas.AspNetCore.Weixin;

public class WeixinBuilder
{
    public WeixinBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; private set; }    
}
