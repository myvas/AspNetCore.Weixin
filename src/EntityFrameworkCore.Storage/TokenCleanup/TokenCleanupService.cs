using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.EntityFrameworkCore;

/// <summary>
/// Helper to cleanup stale persisted tokens.
/// </summary>
public class TokenCleanupService
{
    private readonly WeixinStoreOptions _options;
    private readonly IReceivedEntryStore<EventReceivedEntry> _tokenDbContext;
    private readonly ISubscriptionNotification _operationalStoreNotification;
    private readonly ILogger<TokenCleanupService> _logger;

    /// <summary>
    /// Constructor for TokenCleanupService.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="tokenDbContext"></param>
    /// <param name="logger"></param>
    /// <param name="operationalStoreNotification"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public TokenCleanupService(
        WeixinStoreOptions options,
        IReceivedEntryStore<EventReceivedEntry> tokenDbContext,
        ILogger<TokenCleanupService> logger,
        ISubscriptionNotification operationalStoreNotification = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        if (_options.TokenCleanupBatchSize < 1) throw new ArgumentException("Token cleanup batch size interval must be at least 1");

        _tokenDbContext = tokenDbContext ?? throw new ArgumentNullException(nameof(tokenDbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _operationalStoreNotification = operationalStoreNotification;
    }

    /// <summary>
    /// Method to clear expired persisted tokens.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task RemoveExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogTrace("Querying for expired tokens to remove");

            await RemoveExpiredTokensAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception removing expired tokens: {exception}", ex.Message);
        }
    }

    /// <summary>
    /// Method to clear the stale persisted tokens.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public virtual async Task RemoveTokensAsync(CancellationToken cancellationToken = default)
    {
        await RemoveExpiredPersistedTokensAsync(cancellationToken);
        if (_options.RemoveConsumedTokens)
        {
            await RemoveConsumedPersistedTokensAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Removes the expired persisted tokens.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task RemoveExpiredPersistedTokensAsync(CancellationToken cancellationToken = default)
    {
        var found = Int32.MaxValue;

        while (found >= _options.TokenCleanupBatchSize)
        {
            //var expiredTokens = await _tokenDbContext.EventReceivedEntries
            //    .Where(x => x.GetCreateTime() < DateTime.UtcNow)
            //    .OrderBy(x => x.GetCreateTime())
            //    .Take(_options.TokenCleanupBatchSize)
            //    .ToArrayAsync(cancellationToken);
            var expiredTokens = await _tokenDbContext.GetAllByReceivedTimeAsync(null, DateTime.UtcNow.AddYears(-3));

            found = expiredTokens.Count();
            _logger.LogInformation("Removing {tokenCount} expired tokens", found);

            if (found > 0)
            {
                //_tokenDbContext.EventReceivedEntries.RemoveRange(expiredTokens);

                await SaveChangesAsync();

                if (_operationalStoreNotification != null)
                {
                    //TODO: ...
                    //await _operationalStoreNotification.PersistedTokensRemovedAsync(expiredTokens);
                }
            }
        }
    }

    /// <summary>
    /// Removes the consumed persisted tokens.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected virtual async Task RemoveConsumedPersistedTokensAsync(CancellationToken cancellationToken = default)
    {
        var found = Int32.MaxValue;

        while (found >= _options.TokenCleanupBatchSize)
        {
            //var expiredTokens = await _tokenDbContext.EventReceivedEntries
            //    //.Where(x => x.ConsumedDate < DateTime.UtcNow)
            //    //.OrderBy(x => x.ConsumedDate)
            //    .Take(_options.TokenCleanupBatchSize)
            //    .ToArrayAsync(cancellationToken);
            var expiredTokens = await _tokenDbContext.GetAllByReceivedTimeAsync(null, DateTime.UtcNow.AddYears(-3));

            found = expiredTokens.Count();
            _logger.LogInformation("Removing {tokenCount} consumed tokens", found);

            if (found > 0)
            {
                //_tokenDbContext.EventReceivedEntries.RemoveRange(expiredTokens);
                await SaveChangesAsync();

                if (_operationalStoreNotification != null)
                {
                    //TODO: ...
                    //await _operationalStoreNotification.PersistedTokensRemovedAsync(expiredTokens);
                }
            }
        }
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var count = 3;

        while (count > 0)
        {
            try
            {
                //await _tokenDbContext.SaveChangesAsync(cancellationToken);
                await _tokenDbContext.GetAsync(null);
                return;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                count--;
                // we get this if/when someone else already deleted the records
                // we want to essentially ignore this, and keep working
                _logger.LogDebug("Concurrency exception removing expired tokens: {exception}", ex.Message);

                foreach (var entry in ex.Entries)
                {
                    // mark this entry as not attached anymore so we don't try to re-delete
                    entry.State = EntityState.Detached;
                }
            }
        }

        _logger.LogDebug("Too many concurrency exceptions. Exiting...");
    }
}
