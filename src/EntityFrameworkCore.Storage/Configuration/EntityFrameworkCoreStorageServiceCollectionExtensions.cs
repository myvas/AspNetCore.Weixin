using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Interfaces;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Action<OperationalStoreOptions> storeOptionsAction = null)
    {
        return services.AddOperationalDbContext<PersistedTokenDbContext>(storeOptionsAction);
    }

    /// <summary>
    /// Adds operational DbContext to the DI system.
    /// </summary>
    /// <typeparam name="TContext">The IPersistedGrantDbContext to use.</typeparam>
    /// <param name="services"></param>
    /// <param name="storeOptionsAction">The store options action.</param>
    /// <returns></returns>
    public static IServiceCollection AddOperationalDbContext<TContext>(this IServiceCollection services,
        Action<OperationalStoreOptions> storeOptionsAction = null)
        where TContext : DbContext, IPersistedTokenDbContext
    {
        var storeOptions = new OperationalStoreOptions();
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

        services.AddScoped<IPersistedTokenDbContext, TContext>();
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
        where T : class, IOperationalStoreNotification
    {
        services.AddTransient<IOperationalStoreNotification, T>();
        return services;
    }
}