using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Interfaces;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using Myvas.AspNetCore.Weixin.Extensions;
using Myvas.AspNetCore.Weixin.Models;
using Myvas.AspNetCore.Weixin.Services;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Entities;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;

/// <summary>
/// Implementation of IPersistedTokenStore thats uses EF.
/// </summary>
public class WeixinUserStore : IWeixinUserStore
{
    /// <summary>
    /// The DbContext.
    /// </summary>
    protected readonly IPersistedTokenDbContext Context;

    /// <summary>
    /// The CancellationToken service.
    /// </summary>
    protected readonly ICancellationTokenProvider CancellationTokenProvider;

    /// <summary>
    /// The logger.
    /// </summary>
    protected readonly ILogger Logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PersistedTokenStore"/> class.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    /// <param name="cancellationTokenProvider"></param>
    public WeixinUserStore(
        IPersistedTokenDbContext context,
        ILogger<PersistedTokenStore> logger,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        Context = context;
        Logger = logger;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Models.WeixinSubscriber>> GetAllAsync(WeixinSubscriberFilter filter)
    {
        filter.Validate();

        var entities = await Filter(Context.Subscribers.AsQueryable(), filter)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);
        entities = Filter(entities.AsQueryable(), filter).ToArray();

        var models = entities.Select(x => x.ToModel());

        Logger.LogDebug("{count} users found for {@filter}", entities.Length, filter);

        return models;
    }

    private IQueryable<Entities.WeixinSubscriber> Filter(IQueryable<Entities.WeixinSubscriber> query, WeixinSubscriberFilter filter)
    {
        if (!String.IsNullOrWhiteSpace(filter.OpenId))
        {
            query = query.Where(x => x.OpenId == filter.OpenId);
        }

        return query;
    }

    /// <inheritdoc/>
    public virtual async Task<Models.WeixinSubscriber> GetAsync(string key)
    {
        var entity = (await Context.Subscribers
            .AsNoTracking()
            .Where(x => x.OpenId == key)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.OpenId == key);
        var model = entity?.ToModel();

        Logger.LogDebug("{key} found in database: {found}", key, model != null);

        return model;
    }

    /// <inheritdoc/>
    public async Task RemoveAllAsync(WeixinSubscriberFilter filter)
    {
        filter.Validate();

        var entities = await Filter(Context.Subscribers.AsQueryable(), filter)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);
        entities = Filter(entities.AsQueryable(), filter).ToArray();

        Logger.LogDebug("removing {count} persisted subscribers from database for {@filter}", entities.Length, filter);

        Context.Subscribers.RemoveRange(entities);

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogInformation("removing {count} persisted subscribers from database for subject {@filter}: {error}", entities.Length, filter, ex.Message);
        }
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(string key)
    {
        var entity = (await Context.Subscribers.Where(x => x.OpenId == key)
                .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.OpenId == key);
        if (entity != null)
        {
            Logger.LogDebug("removing {key} persisted subscriber from database", key);

            Context.Subscribers.Remove(entity);

            try
            {
                await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogInformation("exception removing {key} persisted subscriber from database: {error}", key, ex.Message);
            }
        }
        else
        {
            Logger.LogDebug("no {key} persisted subscriber found in database", key);
        }
    }

    /// <inheritdoc/>
    public async Task StoreAsync(Models.WeixinSubscriber model)
    {
        var existing = (await Context.Subscribers
           .Where(x => x.OpenId == model.OpenId)
           .ToArrayAsync(CancellationTokenProvider.CancellationToken))
           .SingleOrDefault(x => x.OpenId == model.OpenId);
        if (existing == null)
        {
            Logger.LogDebug("{key} not found in database", model.OpenId);

            var entity = model.ToEntity();
            Context.Subscribers.Add(entity);
        }
        else
        {
            Logger.LogDebug("{key} found in database", model.OpenId);

            model.UpdateEntity(existing);
        }

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogWarning("Exception updating {key} persisted subscriber in database: {error}", model.OpenId, ex.Message);
        }
    }
}
