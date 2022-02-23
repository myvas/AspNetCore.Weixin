using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Entities;
using Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Extensions;
using Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Interfaces;
using Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;

/// <summary>
/// DbContext for the Weixin operational data.
/// </summary>
public class PersistedTokenDbContext : PersistedTokenDbContext<PersistedTokenDbContext>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersistedTokenDbContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <exception cref="ArgumentNullException">options</exception>
    public PersistedTokenDbContext(DbContextOptions<PersistedTokenDbContext> options)
        : base(options)
    {
    }
}

/// <summary>
/// DbContext for the Weixin operational data.
/// </summary>
/// <typeparam name="TDbContext">The DbContext implemented IPersistedTokenDbContext.</typeparam>
/// <seealso cref="DbContext"/>
/// <seealso cref="IPersistedTokenDbContext"/>
public class PersistedTokenDbContext<TDbContext> : DbContext, IPersistedTokenDbContext
    where TDbContext : DbContext, IPersistedTokenDbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PersistedTokenDbContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    public PersistedTokenDbContext(DbContextOptions options)
        :base(options)
    {
    }

    /// <inheritdoc/>
    public DbSet<PersistedToken> PersistedTokens { get; set; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var storeOptions = this.GetService<OperationalStoreOptions>();

        if (storeOptions is null)
        {
            throw new ArgumentNullException(nameof(storeOptions));
        }
        modelBuilder.ConfigurePersistedTokenContext(storeOptions);

        base.OnModelCreating(modelBuilder);
    }
}
