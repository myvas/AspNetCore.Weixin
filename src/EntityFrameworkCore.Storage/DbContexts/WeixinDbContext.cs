using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Myvas.AspNetCore.Weixin.Entities;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Storage.Extensions;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;

#pragma warning disable 1591

/// <summary>
/// DbContext for the Weixin operational data.
/// </summary>
public class WeixinDbContext : WeixinDbContext<WeixinDbContext>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeixinDbContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <exception cref="ArgumentNullException">options</exception>
    public WeixinDbContext(DbContextOptions<WeixinDbContext> options)
        : base(options)
    {
    }
}

/// <summary>
/// DbContext for the Weixin operational data.
/// </summary>
/// <typeparam name="TDbContext">The DbContext implemented IPersistedTokenDbContext.</typeparam>
/// <seealso cref="DbContext"/>
/// <seealso cref="IWeixinDbContext"/>
public class WeixinDbContext<TDbContext> : DbContext, IWeixinDbContext
    where TDbContext : DbContext, IWeixinDbContext
{
    private readonly WeixinStoreOptions _storeOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="WeixinDbContext"/> class.
    /// </summary>
    /// <param name="options"></param>
    public WeixinDbContext(DbContextOptions options)
        : base(options)
    {
        _storeOptions = this.GetService<WeixinStoreOptions>();
    }

    #region shared-type entity types: Support for Shared-type entity types was introduced in EF Core 5.0+.
    #endregion

    public DbSet<AuditEntry> AuditEntires { get; set; }

    /// <inheritdoc/>
    public DbSet<PersistedToken> PersistedTokens { get; set; }

    /// <inheritdoc/>
    public DbSet<WeixinSubscriber> WeixinSubscribers { get; set; }
    #region Table-per-hierarchy for received messages: text, image, voice, video, shortvideo, location, link.
    /// <summary>
    /// Table-per-hierarchy
    /// </summary>
    public DbSet<MessageReceivedEntity> ReceivedMessages { get; set; }
    //public DbSet<TextMessageReceivedEntity> ReceivedTextMessages { get; set; }
    //public DbSet<ImageMessageReceivedEntity> ReceivedImageMessages { get; set; }
    //public DbSet<VoiceMessageReceivedEntity> ReceivedVoiceMessages { get; set; }
    //public DbSet<VideoMessageReceivedEntity> ReceivedVideoMessages { get; set; }
    //public DbSet<ShortVideoMessageReceivedEntity> ReceivedShortVideoMessages { get; set; }
    //public DbSet<LocationMessageReceivedEntity> ReceivedLocationMessages { get; set; }
    //public DbSet<LinkMessageReceivedEntity> ReceivedLinkMessages { get; set; }
    #endregion

    #region Table-per-type for received events
    public DbSet<SubscribeEventReceivedEntity> ReceivedSubscribeEvents { get; set; }
    public DbSet<EnterEventReceivedEntity> ReceivedEnterEvents { get; set; }
    public DbSet<ClickMenuEventReceivedEntity> ReceivedClickMenuEvents { get; set; }
    public DbSet<ViewMenuEventReceivedEntity> ReceivedViewMenuEvents { get; set; }
    public DbSet<QrscanEventReceivedEntity> ReceivedQrscanEvents { get; set; }
    public DbSet<LocationEventReceivedEntity> ReceivedLocationEvents { get; set; }
    public DbSet<UnsubscribeEventReceivedEntity> ReceivedUnsubscribeEvents { get; set; }
    #endregion

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var storeOptions = this.GetService<WeixinStoreOptions>();

        if (storeOptions is null)
        {
            throw new ArgumentNullException(nameof(storeOptions));
        }
        modelBuilder.ConfigureWeixinDbContext(storeOptions);

        base.OnModelCreating(modelBuilder);
    }

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
                        break;
                    case EntityState.Deleted:
                        auditEntry.OldValue = property.OriginalValue?.ToString();
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.OldValue = entry.GetDatabaseValues().GetValue<object>(propertyName)?.ToString();
                            auditEntry.NewValue = property.CurrentValue?.ToString();
                        }
                        break;
                }

                auditEntries.Add(auditEntry);
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
