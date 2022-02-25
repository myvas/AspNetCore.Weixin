using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Interfaces;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Mappers;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using Myvas.AspNetCore.Weixin.Extensions;
using Myvas.AspNetCore.Weixin.Models;
using Myvas.AspNetCore.Weixin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Entities;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;

/// <summary>
/// Implementation of IPersistedTokenStore thats uses EF.
/// </summary>
public class PersistedTokenStore : IPersistedTokenStore
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
    public PersistedTokenStore(
        IPersistedTokenDbContext context,
        ILogger<PersistedTokenStore> logger,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        Context = context;
        Logger = logger;
        CancellationTokenProvider = cancellationTokenProvider;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Models.PersistedToken>> GetAllAsync(PersistedTokenFilter filter)
    {
        filter.Validate();

        var entities = await Filter(Context.PersistedTokens.AsQueryable(), filter)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);
        entities = Filter(entities.AsQueryable(), filter).ToArray();

        var models = entities.Select(x => x.ToModel());

        Logger.LogDebug("{persistedTokenCount} persisted tokens found for {@filter}", entities.Length, filter);

        return models;
    }

    private IQueryable<Entities.PersistedToken> Filter(IQueryable<Entities.PersistedToken> query, PersistedTokenFilter filter)
    {
        if (!String.IsNullOrWhiteSpace(filter.AppId))
        {
            query = query.Where(x => x.AppId == filter.AppId);
        }

        return query;
    }

    /// <inheritdoc/>
    public virtual async Task<Models.PersistedToken> GetAsync(string key)
    {
        var entity = (await Context.PersistedTokens
            .AsNoTracking()
            .Where(x => x.AppId == key)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.AppId == key);
        var model = entity?.ToModel();

        Logger.LogDebug("{persistedTokenId} found in database: {persistedTokenIdFound}", key, model != null);

        return model;
    }

    /// <inheritdoc/>
    public async Task RemoveAllAsync(PersistedTokenFilter filter)
    {
        filter.Validate();

        var entities = await Filter(Context.PersistedTokens.AsQueryable(), filter)
            .ToArrayAsync(CancellationTokenProvider.CancellationToken);
        entities = Filter(entities.AsQueryable(), filter).ToArray();

        Logger.LogDebug("removing {persistedTokenCount} persisted tokens from database for {@filter}", entities.Length, filter);

        Context.PersistedTokens.RemoveRange(entities);

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogInformation("removing {persistedTokenCount} persisted tokens from database for subject {@filter}: {error}", entities.Length, filter, ex.Message);
        }
    }

    /// <inheritdoc/>
    public virtual async Task RemoveAsync(string key)
    {
        var entity = (await Context.PersistedTokens.Where(x => x.AppId == key)
                .ToArrayAsync(CancellationTokenProvider.CancellationToken))
            .SingleOrDefault(x => x.AppId == key);
        if (entity != null)
        {
            Logger.LogDebug("removing {persistedTokenId} persisted token from database", key);

            Context.PersistedTokens.Remove(entity);

            try
            {
                await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogInformation("exception removing {persistedTokenId} persisted token from database: {error}", key, ex.Message);
            }
        }
        else
        {
            Logger.LogDebug("no {persistedTokenId} persisted token found in database", key);
        }
    }

    /// <inheritdoc/>
    public async Task StoreAsync(Models.PersistedToken token)
    {
        var existing = (await Context.PersistedTokens
           .Where(x => x.AppId == token.AppId)
           .ToArrayAsync(CancellationTokenProvider.CancellationToken))
           .SingleOrDefault(x => x.AppId == token.AppId);
        if (existing == null)
        {
            Logger.LogDebug("{persistedTokenId} not found in database", token.AppId);

            var persistedToken = token.ToEntity();
            Context.PersistedTokens.Add(persistedToken);
        }
        else
        {
            Logger.LogDebug("{persistedTokenId} found in database", token.AppId);

            token.UpdateEntity(existing);
        }

        try
        {
            await Context.SaveChangesAsync(CancellationTokenProvider.CancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            Logger.LogWarning("Exception updating {persistedTokenId} persisted token in database: {error}", token.AppId, ex.Message);
        }
    }
}
