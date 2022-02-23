namespace Myvas.AspNetCore.Weixin.AccessTokenServer.EntityFrameworkCore;

/// <summary>
/// Interface to model notifications from the TokenCleanup feature.
/// </summary>
public interface IOperationalStoreNotification
{
    /// <summary>
    /// Notification for persisted tokens being removed.
    /// </summary>
    /// <param name="persistedTokens">The persisted tokens</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    Task PersistedTokensRemovedAsync(IEnumerable<Entities.PersistedToken> persistedTokens, CancellationToken cancellationToken = default);
}