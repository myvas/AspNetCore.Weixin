using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;

/// <summary>
/// Interface for the access token store.
/// </summary>
public interface IPersistedTokenStore
{
    /// <summary>
    /// Stores the access token.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task StoreAsync(PersistedToken token);

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    Task<PersistedToken> GetAsync(string key);

    /// <summary>
    /// Gets all access tokens based on the filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <returns></returns>
    Task<IEnumerable<PersistedToken>> GetAllAsync(PersistedTokenFilter filter);

    /// <summary>
    /// Removes the token by key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    Task RemoveAsync(string key);

    /// <summary>
    /// Removes all tokens based on the filter.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    Task RemoveAllAsync(PersistedTokenFilter filter);
}

