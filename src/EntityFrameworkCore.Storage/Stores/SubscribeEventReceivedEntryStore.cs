using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;
using Myvas.AspNetCore.Weixin.Models;
using Myvas.AspNetCore.Weixin.Services;
using Myvas.AspNetCore.Weixin.Storage.Extensions;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;

/// <summary>
/// Implementation of <see cref="ISubscribeEventReceivedEntryStore"/> thats uses EF.
/// </summary>
public class SubscribeEventReceivedEntryStore : ISubscribeEventReceivedEntryStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    protected readonly IWeixinDbContext Context;

    /// <summary>
    /// The CancellationToken service.
    /// </summary>
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    /// <summary>
    /// The logger.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubscribeEventReceivedEntryStore"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="cancellationTokenProvider"></param>
    public SubscribeEventReceivedEntryStore(
        IWeixinDbContext context,
        ILogger<SubscribeEventReceivedEntryStore> logger,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        Context = context;
        Logger = logger;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<SubscribeEventReceivedEntry>> GetAllAsync(SubscribeEventReceivedEntryFilter filter)
    {
        filter.Validate();

        var entities = await Filter(Context.SubscribeReceivedEventEntries.AsQueryable(), filter)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);
        entities = Filter(entities.AsQueryable(), filter).ToArray();

        var models = entities;//.Select(x => x.ToModel());

        Logger.LogDebug("{count} received subscribe events found for {@filter}", entities.Length, filter);

        return models;
    }

    private IQueryable<SubscribeEventReceivedEntry> Filter(IQueryable<SubscribeEventReceivedEntry> query, SubscribeEventReceivedEntryFilter filter)
    {
        if (!String.IsNullOrWhiteSpace(filter.OpenId))
        {
            query = query.Where(x => x.FromUserName == filter.OpenId);
        }

        return query;
    }

    /// <inheritdoc/>
    public virtual async Task<SubscribeEventReceivedEntry> GetAsync(string key)
    {
        var entity = (await Context.SubscribeReceivedEventEntries
            .AsNoTracking()
            .Where(x => x.FromUserName == key)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.FromUserName == key);
        var model = entity;//?.ToModel();

        Logger.LogDebug("{id} found in database: {found}", key, model != null);

        return model;
    }

    /// <inheritdoc/>
    public async Task RemoveAllAsync(SubscribeEventReceivedEntryFilter filter)
    {
        filter.Validate();

        var entities = await Filter(Context.SubscribeReceivedEventEntries.AsQueryable(), filter)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);
        entities = Filter(entities.AsQueryable(), filter).ToArray();

        Logger.LogDebug("removing {count} received subscribe events from database for {@filter}", entities.Length, filter);

        Context.SubscribeReceivedEventEntries.RemoveRange(entities);

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogInformation("removing {count} received subscribe events from database for subject {@filter}: {error}", entities.Length, filter, ex.Message);
        }
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(string key)
    {
        var entity = (await Context.SubscribeReceivedEventEntries.Where(x => x.FromUserName == key)
                .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.FromUserName == key);
        if (entity != null)
        {
            Logger.LogDebug("removing {id} received subscribe event from database", key);

            Context.SubscribeReceivedEventEntries.Remove(entity);

            try
            {
                await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogInformation("exception removing {id} received subscribe event from database: {error}", key, ex.Message);
            }
        }
        else
        {
            Logger.LogDebug("no {id} received subscribe event found in database", key);
        }
    }

    /// <inheritdoc/>
    public async Task StoreAsync(SubscribeEventReceivedEntry item)
    {
        var existing = (await Context.SubscribeReceivedEventEntries
           .Where(x => x.FromUserName == item.FromUserName)
           .ToArrayAsync(CancellationTokenProvider.CancellationToken))
           .SingleOrDefault(x => x.FromUserName == item.FromUserName);
        if (existing == null)
        {
            Logger.LogDebug("{id} not found in database", item.FromUserName);

            var entity = item;//.ToEntity();
            Context.SubscribeReceivedEventEntries.Add(entity);
        }
        else
        {
            Logger.LogDebug("{id} found in database", item.FromUserName);

            //token.UpdateEntity(existing);
            Context.SubscribeReceivedEventEntries.Update(existing);
        }

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogWarning("Exception updating {id} received subscribe event in database: {error}", item.FromUserName, ex.Message);
        }
    }
}
