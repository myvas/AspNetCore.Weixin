using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class ResponseBuilderServiceCollectionExtensions
{
    public static IServiceCollection AddWeixinResponseBuilder(this IServiceCollection services)
    {
        return services.AddSingleton<IWeixinResponseBuilder, WeixinResponseBuilder>();
    }
}
