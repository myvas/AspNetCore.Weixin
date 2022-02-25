using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Entities;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Extensions;

/// <summary>
/// Extensions methods to define the database schema for the configuration and operational data stores.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Configures the persisted token context.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    /// <param name="storeOptions">The store options.</param>
    public static void ConfigurePersistedTokenContext(this ModelBuilder modelBuilder, OperationalStoreOptions storeOptions)
    {
        modelBuilder.Entity<PersistedToken>(entity =>
        {
            entity.HasKey(x => x.AppId);
            entity.Property(x => x.AppId).HasMaxLength(200).ValueGeneratedNever();
            entity.Property(x => x.AccessToken).HasMaxLength(1000).IsRequired();
            entity.Property(x => x.ExpirationTime).IsRequired();
            entity.HasIndex(x => x.ExpirationTime);
        });

        modelBuilder.Entity<WeixinSubscriber>(entity =>
        {
            entity.ToTable("WeixinSubscriber");

            entity.HasKey(x => x.OpenId);
            entity.Property(x => x.OpenId).HasMaxLength(200).ValueGeneratedNever();
            entity.Property(x => x.Unsubscribed).HasMaxLength(1000).IsRequired();
            entity.HasIndex(x => x.OpenId);
        });
    }
}
