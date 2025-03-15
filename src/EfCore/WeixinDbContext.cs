using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Myvas.AspNetCore.Weixin;

public class WeixinDbContext : WeixinDbContext<WeixinSubscriber, string>
{
    public WeixinDbContext(DbContextOptions options) : base(options)
    {
    }

    protected WeixinDbContext() { }
}

public class WeixinDbContext<TWeixinSubscriber, TKey> : DbContext, IWeixinDbContext<TWeixinSubscriber, TKey>
    where TWeixinSubscriber : WeixinSubscriber<TKey>
    where TKey : IEquatable<TKey>
{
    public WeixinDbContext(DbContextOptions options) : base(options)
    {
    }

    protected WeixinDbContext() { }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinSubscriber"/>.
    /// </summary>
    public DbSet<TWeixinSubscriber> WeixinSubscribers { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinReceivedMessage"/>.
    /// </summary>
    public DbSet<WeixinReceivedMessage> WeixinReceivedMessages { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinReceivedEvent"/>.
    /// </summary>
    public DbSet<WeixinReceivedEvent> WeixinReceivedEvents { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinSendMessage"/>.
    /// </summary>
    public DbSet<WeixinSendMessage> WeixinSendMessages { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinResponseMessage"/>.
    /// </summary>
    public DbSet<WeixinResponseMessage> WeixinResponseMessages { get; set; }

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
