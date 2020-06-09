using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinDbContext : WeixinDbContext<WeixinSubscriber, string>
    {
        public WeixinDbContext(DbContextOptions options) : base(options)
        {
        }

        protected WeixinDbContext() { }
    }

    public class WeixinDbContext<TWeixinSubscriber> : WeixinDbContext<TWeixinSubscriber, string>
        where TWeixinSubscriber : WeixinSubscriber
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
        public DbSet<TWeixinSubscriber> Subscribers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinReceivedMessage"/>.
        /// </summary>
        public DbSet<WeixinReceivedMessage> ReceivedMessages { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinReceivedEvent"/>.
        /// </summary>
        public DbSet<WeixinReceivedEvent> ReceivedEvents { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinSendMessage"/>.
        /// </summary>
        public DbSet<WeixinSendMessage> SendMessages { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinResponseMessage"/>.
        /// </summary>
        public DbSet<WeixinResponseMessage> ResponseMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var storeOptions = GetStoreOptions();

            builder.Entity<TWeixinSubscriber>(b =>
            {
                b.HasKey(x => x.Id);
                b.ToTable("WeixinSubscibers");
                b.Property(x => x.ConcurrencyStamp).IsConcurrencyToken();
                b.Property(x => x.Nickname).HasMaxLength(256);
            });
        }

        private WeixinStoreOptions GetStoreOptions()
            => this.GetService<IDbContextOptions>().Extensions.OfType<CoreOptionsExtension>()
            .FirstOrDefault()?.ApplicationServiceProvider
            ?.GetService<IOptions<WeixinStoreOptions>>()
            ?.Value;//?.Stores;
    }
}
