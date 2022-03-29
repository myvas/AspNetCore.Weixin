using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using Myvas.AspNetCore.Weixin.Models;
using Myvas.AspNetCore.Weixin.Services;
using Myvas.AspNetCore.Weixin.Storage.Extensions;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;

/// <summary>
/// Implementation of <see cref="IReceivedEntryStore{T}"/> thats uses EF Core.
/// </summary>
public class EventReceivedEntryStore<TContext> : IReceivedEntryStore<EventReceivedEntry>, IReceivedEventStore
    where TContext : DbContext
{
    /// <summary>
    /// Gets the database context for this store.
    /// </summary>
    protected virtual TContext Context { get; private set; }

    private DbSet<EventReceivedEntry> EventReceivedEntriesSet { get { return Context.Set<EventReceivedEntry>(); } }
    private DbSet<MessageReceivedEntry> MessageReceivedEntriesSet { get { return Context.Set<MessageReceivedEntry>(); } }

    /// <summary>
    /// A navigation property for the entities the store contains.
    /// </summary>
    public IQueryable<EventReceivedEntry> EventReceivedEntries { get { return EventReceivedEntriesSet; } }

    /// <summary>
    /// A navigation property for the entities the store contains.
    /// </summary>
    public IQueryable<MessageReceivedEntry> MessageReceivedEntries { get { return MessageReceivedEntriesSet; } }

    /// <summary>
    /// The CancellationToken service.
    /// </summary>
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    /// <summary>
    /// The logger.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventReceivedEntryStore{TContext}"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="cancellationTokenProvider"></param>
    public EventReceivedEntryStore(
        TContext context,
        ILogger<EventReceivedEntryStore<TContext>> logger,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        Context = context;
        Logger = logger;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <inheritdoc/>
    public async Task<EventReceivedEntry> GetAsync(string id)
    {
        var entity = await EventReceivedEntries//.AsQueryable().Cast<EventReceivedEntry>()
            .FirstOrDefaultAsync(x => x.Id == id, CancellationTokenProvider.CancellationToken);

        Logger.LogDebug("{id}{not} found in database", id, entity == null ? " not" : "");

        return entity;
    }

    /// <inheritdoc/>
    public async Task StoreAsync<TEntity>(TEntity item) where TEntity : EventReceivedEntry
    {
        var existing = await EventReceivedEntries.AsQueryable().Cast<TEntity>()
           .FirstOrDefaultAsync(x => x.Id == item.Id, CancellationTokenProvider.CancellationToken);
        if (existing == null)
        {
            Logger.LogDebug("{id} not found in database", item.Id);
            if (string.IsNullOrWhiteSpace(item.Id)) item.Id = Guid.NewGuid().ToString("N");
            Context.Add(item);
        }
        else
        {
            Logger.LogDebug("{id} found in database", item.Id);
            Context.Update(item);
        }

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogWarning("Exception updating {id} received event in database: {error}", item.Id, ex.Message);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EventReceivedEntry>> GetAllByFromUserNameAsync(string fromUserName)
    {
        var entities = await EventReceivedEntries//.AsQueryable().Cast<EventReceivedEntry>()
            .Where(x => x.FromUserName == fromUserName)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);

        Logger.LogDebug("{count} received subscribe events found for {@fromUserName}", entities.Length, fromUserName);

        return entities;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EventReceivedEntry>> GetAllByToUserNameAsync(string toUserName)
    {
        var entities = await EventReceivedEntries//.AsQueryable().Cast<EventReceivedEntry>()
            .Where(x => x.ToUserName == toUserName)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);

        Logger.LogDebug("{count} received subscribe events found for {@toUserName}", entities.Length, toUserName);

        return entities;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<EventReceivedEntry>> GetAllByReceivedTimeAsync(DateTime? startTime, DateTime? endTime)
    {
        var entities = await EventReceivedEntries//.AsQueryable().Cast<EventReceivedEntry>()
            .Where(x => x.GetCreateTime() > (startTime.HasValue ? startTime.Value : DateTime.MinValue)
                && x.GetCreateTime() < (endTime.HasValue ? endTime.Value : DateTime.MaxValue))
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);

        Logger.LogDebug("{count} received subscribe events found for {@startTime}-{@endTime}", entities.Length,
            startTime, endTime);

        return entities;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
    }
}
