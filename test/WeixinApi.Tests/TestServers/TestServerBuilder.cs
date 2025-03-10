using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Myvas.AspNetCore.Weixin.Api.Tests.TestServers;

public static class TestServerBuilder
{
    public static TestServer CreateServer(Action<IApplicationBuilder> configure, Action<IServiceCollection> configureServices, Func<HttpContext, Task<bool>> handler)
    {
        var builder = new WebHostBuilder()
            .Configure(app =>
            {
                configure?.Invoke(app);
                app.Use(async (context, next) =>
                {
                    if (handler == null || !await handler(context))
                    {
                        await next();
                    }
                });
            })
            .ConfigureServices(services => configureServices?.Invoke(services));
        return new TestServer(builder);
    }
}
