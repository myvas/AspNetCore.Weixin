using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods to add EF database support to IdentityServer.
/// </summary>
public static class EntityFrameworkCoreStorageServiceCollectionExtensions
{
    /// <summary>
    /// Adds operational DbContext to the DI system.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddOperationalDbContext(this IServiceCollection services,
        Action<WeixinStoreOptions> storeOptionsAction = null)
    {
        return services.AddOperationalDbContext<WeixinDbContext>(storeOptionsAction);
    }

    /// <summary>
    /// Adds operational DbContext to the DI system.
    /// </summary>
    /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddOperationalDbContext<TContext>(this IServiceCollection services,
        Action<WeixinStoreOptions> storeOptionsAction = null)
        where TContext : DbContext, IWeixinDbContext
    {
        var storeOptions = new WeixinStoreOptions();
        services.AddSingleton(storeOptions);
        storeOptionsAction?.Invoke(storeOptions);

        if (storeOptions.ResolveDbContextOptions != null)
        {
            if (storeOptions.EnablePooling)
            {
                if (storeOptions.PoolSize.HasValue)
                {
                    services.AddDbContextPool<TContext>(storeOptions.ResolveDbContextOptions,
                        storeOptions.PoolSize.Value);
                }
                else
                {
                    services.AddDbContextPool<TContext>(storeOptions.ResolveDbContextOptions);
                }
            }
            else
            {
                services.AddDbContext<TContext>(storeOptions.ResolveDbContextOptions);
            }
        }
        else
        {
            if (storeOptions.EnablePooling)
            {
                if (storeOptions.PoolSize.HasValue)
                {
                    services.AddDbContextPool<TContext>(
                        dbCtxBuilder => { storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder); },
                        storeOptions.PoolSize.Value);
                }
                else
                {
                    services.AddDbContextPool<TContext>(
                        dbCtxBuilder => { storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder); });
                }
            }
            else
            {
                services.AddDbContext<TContext>(dbCtxBuilder =>
                {
                    storeOptions.ConfigureDbContext?.Invoke(dbCtxBuilder);
                });
            }
        }

        services.AddScoped<IWeixinDbContext, TContext>();
        services.AddTransient<TokenCleanupService>();

        return services;
    }

    /// <summary>
    /// Adds an implementation of the IOperationalStoreNotification to the DI system.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOperationalStoreNotification<T>(this IServiceCollection services)
        where T : class, ISubscriptionNotification
    {
        services.AddTransient<ISubscriptionNotification, T>();
        return services;
    }
}