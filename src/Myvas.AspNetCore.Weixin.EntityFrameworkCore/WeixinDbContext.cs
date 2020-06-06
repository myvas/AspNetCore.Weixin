using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinDbContext<TWeixinSubscriber> : WeixinDbContext<TWeixinSubscriber, ReceivedTextMessage>
        where TWeixinSubscriber : WeixinSubscriber
    {
    }

    public class WeixinDbContext<TWeixinSubscriber, TReceivedTextMessage> : DbContext
    where TWeixinSubscriber : WeixinSubscriber
    where TReceivedTextMessage : ReceivedTextMessage
    {
        public WeixinDbContext(DbContextOptions<WeixinDbContext<TWeixinSubscriber, TReceivedTextMessage>> options) : base(options)
        {
        }

        protected WeixinDbContext() { }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="WeixinSubscriber"/>.
        /// </summary>
        public DbSet<TWeixinSubscriber> Subscribers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of <see cref="ReceivedTextMessage"/>.
        /// </summary>
        public DbSet<TReceivedTextMessage> ReceivedTextMessages { get; set; }

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

        private StoreOptions GetStoreOptions()
            => this.GetService<IDbContextOptions>().Extensions.OfType<CoreOptionsExtensions>()
            .FirstOrDefault()?.ApplicationServiceProvider
            ?.GetService<IOptions<WeixinStoreOptions>>()
            ?.Value?.Stores;
    }
}
