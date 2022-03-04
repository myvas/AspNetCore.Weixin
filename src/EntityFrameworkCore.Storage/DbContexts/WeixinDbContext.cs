using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Storage;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Storage.Extensions;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;

/// <summary>
/// DbContext for the Weixin operational data.
/// </summary>
/// <typeparam name="TSubscriber">The Subscriber implemented <see cref="Subscriber"/>.</typeparam>
/// <seealso cref="DbContext"/>
/// <seealso cref="IWeixinDbContext{TSubscriber}"/>
public class WeixinDbContext<TSubscriber> : DbContext, IWeixinDbContext<TSubscriber>
    where TSubscriber : Subscriber
{
    private readonly WeixinStoreOptions _storeOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeixinDbContext{TSubscriber}"/> class.
    /// </summary>
    /// <param name="options"></param>
    public WeixinDbContext(DbContextOptions options) : base(options)
    {
        _storeOptions = this.GetService<WeixinStoreOptions>();
    }

    #region shared-type entity types: Support for Shared-type entity types was introduced in EF Core 5.0+.
    #endregion

    /// <inheritdoc/>
    public DbSet<AuditEntry> AuditEntires { get; set; }

    /// <inheritdoc/>
    public DbSet<TSubscriber> Subscribers { get; set; }
    /// <inheritdoc/>
    public DbSet<MessageReceivedEntry> MessageReceivedEntries { get; set; }
    /// <inheritdoc/>
    public DbSet<EventReceivedEntry> EventReceivedEntries { get; set; }

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var storeOptions = this.GetService<WeixinStoreOptions>();

        if (storeOptions is null)
        {
            throw new ArgumentNullException(nameof(storeOptions));
        }
        modelBuilder.ConfigureWeixinDbContext<TSubscriber>(storeOptions);

        base.OnModelCreating(modelBuilder);
    }

    /// <inheritdoc/>
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SaveModifiedTimeAsync();

        var modifiedTimeEntries = OnBeforeSaveChangesAsync();
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        await OnAfterSaveChangesAsync(modifiedTimeEntries, cancellationToken);
        return result;
    }

    private void SaveModifiedTimeAsync()
    {
        var entities = ChangeTracker.Entries<IModifiedTime>()
            .Select(x => x.Entity);

        var now = DateTime.UtcNow;
        foreach (var entity in entities)
        {
            entity.ModifiedTime = now;
        }
    }

    private List<AuditEntry> OnBeforeSaveChangesAsync()
    {
        var auditEntries = new List<AuditEntry>();
        //Get the options to decide whether we should enable this feature
        if (!(_storeOptions?.EnableAuditable ?? false))
        {
            return auditEntries;
        }

        ChangeTracker.DetectChanges();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditEntry
                || entry.State == EntityState.Detached
                || entry.State == EntityState.Unchanged)
            {
                continue;
            }

            var auditEntry = new AuditEntry()
            {
                TableName = entry.Metadata.GetTableName()
            };

            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                {
                    continue;
                }

                var propertyName = property.Metadata.Name;
                auditEntry.KeyValue = propertyName;

                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Metadata.IsOwned())
                        {
                            auditEntry.OldValue = property.OriginalValue?.ToString();
                        }
                        auditEntry.NewValue = property.CurrentValue?.ToString();
                        auditEntries.Add(auditEntry);
                        break;
                    case EntityState.Deleted:
                        auditEntry.OldValue = property.OriginalValue?.ToString();
                        auditEntries.Add(auditEntry);
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.OldValue = entry.GetDatabaseValues().GetValue<object>(propertyName)?.ToString();
                            auditEntry.NewValue = property.CurrentValue?.ToString();
                        }
                        auditEntries.Add(auditEntry);
                        break;
                    default:
                        break;
                }
            }
        }
        return auditEntries.ToList();
    }

    private async Task<int> OnAfterSaveChangesAsync(List<AuditEntry> auditEntries, CancellationToken cancellationToken = default)
    {
        if (auditEntries == null || auditEntries.Count == 0)
        {
            return 0;
        }

        AuditEntires.AddRange(auditEntries);
        return await SaveChangesAsync(cancellationToken);
    }
}
