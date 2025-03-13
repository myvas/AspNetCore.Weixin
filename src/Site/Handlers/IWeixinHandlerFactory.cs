using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinHandlerFactory
{
    TWeixinHandler Create<TWeixinHandler>(IServiceProvider serviceProvider);
}
