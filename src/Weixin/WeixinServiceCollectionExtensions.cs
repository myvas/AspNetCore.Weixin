using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Myvas.AspNetCore.Weixin.Abstractions.Site;
using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    public static class WeixinServiceCollectionExtensions
    {
        public static WeixinBuilder AddWeixin(this IServiceCollection services)
        {
            return services.AddWeixin(setupAction: null);
        }

        public static WeixinBuilder AddWeixin(this IServiceCollection services, Action<WeixinOptions> setupAction)
        {
            services.AddSite(o =>
                {
                    o.Path = "/wx";
                });

            // Hosting doesn't add IHttpContextAccessor by default
            services.AddHttpContextAccessor();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new WeixinBuilder(services);
        }
    }

    public static class WeixinBuilderExtensions
    {
        public static WeixinBuilder  AddAesEncoder(this WeixinBuilder builder)
        {
            builder.Services.AddScoped<IWeixinSiteEncoder, AesWeixinSiteEncoder>();
            return builder;
        }
    }
}
