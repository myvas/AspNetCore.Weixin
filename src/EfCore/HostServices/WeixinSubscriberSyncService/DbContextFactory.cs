using System;
using Microsoft.EntityFrameworkCore;

namespace Myvas.AspNetCore.Weixin.EfCore;

/// <summary>
/// A factory to create db context.
/// </summary>
/// <typeparam name="TDbContext"></typeparam>
/// <remarks>Use in WeixinSubscriberSyncService for "DbContext is Not Thread-Safe".
/// We need to ensure that each operation or thread uses its own DbContext instance in background service.
// </remarks>
/// <seealso cref="IDbContextFactory{TDbContext}">Not available in netcoreapp3.1 and below.</seealso>
public class DbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    private readonly DbContextOptions _options;

    public DbContextFactory(DbContextOptions options)
    {
        _options = options;
    }

    public TDbContext CreateDbContext()
    {
        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), _options);
    }

    public TDbContext CreateDbContext(DbContextOptions options)
    {
        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), options);
    }
}