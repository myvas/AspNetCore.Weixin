using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Myvas.AspNetCore.Weixin;

public class WeixinDbContext : WeixinDbContext<WeixinSubscriberEntity>
{
    public WeixinDbContext(DbContextOptions<WeixinDbContext> options) : base(options) { }

    protected WeixinDbContext() { }
}


public class WeixinDbContext<TWeixinSubscriber> : WeixinDbContext<TWeixinSubscriber, string>
 where TWeixinSubscriber : class, IWeixinSubscriber<string>, IEntity
{
    public WeixinDbContext(DbContextOptions<WeixinDbContext> options) : base(options) { }

    protected WeixinDbContext() { }
}

public class WeixinDbContext<TWeixinSubscriber, TKey> : DbContext, IWeixinDbContext<TWeixinSubscriber, TKey>
    where TWeixinSubscriber : class, IWeixinSubscriber<TKey>, IEntity
    where TKey : IEquatable<TKey>
{
    public WeixinDbContext(DbContextOptions options) : base(options) { }

    protected WeixinDbContext() { }

    /// <inheritdoc/>
    public DbSet<TWeixinSubscriber> WeixinSubscribers { get; set; }

    /// <inheritdoc/>
    public DbSet<WeixinReceivedMessageEntity> WeixinReceivedMessages { get; set; }

    /// <inheritdoc/>
    public DbSet<WeixinReceivedEventEntity> WeixinReceivedEvents { get; set; }

    /// <inheritdoc/>
    public DbSet<WeixinSendMessageEntity> WeixinSendMessages { get; set; }

    /// <inheritdoc/>
    public DbSet<WeixinResponseMessageEntity> WeixinResponseMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        var storeOptions = GetStoreOptions();

        builder.Entity<TWeixinSubscriber>(b =>
        {
            b.HasKey(x => x.Id);
            b.ToTable(nameof(WeixinSubscribers));
            b.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
            b.Property(x => x.Nickname).HasMaxLength(256);
        });
    }

    private WeixinSiteEfCoreOptions GetStoreOptions()
        => this.GetService<IDbContextOptions>().Extensions.OfType<CoreOptionsExtension>()
        .FirstOrDefault()?.ApplicationServiceProvider
        ?.GetService<IOptions<WeixinSiteEfCoreOptions>>()
        ?.Value;//?.Stores;
}
