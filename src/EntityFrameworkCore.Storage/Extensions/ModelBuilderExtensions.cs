using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Entities;
using Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore.Extensions;

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
            entity.Property(x => x.Id).HasMaxLength(200).ValueGeneratedNever();

            entity.Property(x => x.AppId).HasMaxLength(50).IsRequired();
            entity.Property(x => x.CreationDate).IsRequired();
            entity.Property(x => x.Data).HasMaxLength(1000).IsRequired();

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => new { x.AppId, x.Type });
            entity.HasIndex(x => x.ExpirationDate);
            entity.HasIndex(x => x.ConsumedDate);
        });
    }
}
