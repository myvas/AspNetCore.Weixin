using Microsoft.Extensions.DependencyInjection;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinHandlerFactory : IWeixinHandlerFactory
    {
        public TWeixinHandler Create<TWeixinHandler>(IServiceProvider serviceProvider)
        {
            return ActivatorUtilities.CreateInstance<TWeixinHandler>(serviceProvider);
        }

        public TWeixinHandler Create<TWeixinHandler>()
        {
            return Activator.CreateInstance<TWeixinHandler>();
        }
    }
}
