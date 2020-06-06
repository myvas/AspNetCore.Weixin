using System;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinHandlerFactory : IWeixinHandlerFactory
    {
        public TWeixinHandler Create<TWeixinHandler>()
        {
            return (TWeixinHandler)Activator.CreateInstance<TWeixinHandler>();
        }
    }
}
