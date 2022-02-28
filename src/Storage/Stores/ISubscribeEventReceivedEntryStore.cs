using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;

/// <summary>
/// Interface for the <see cref="SubscribeEventReceivedEntry"/> store.
/// </summary>
public interface ISubscribeEventReceivedEntryStore
{
    /// <summary>
    /// Stores the access token.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    Task StoreAsync(SubscribeEventReceivedEntry item);

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    Task<SubscribeEventReceivedEntry> GetAsync(string key);

    /// <summary>
    /// Gets all access tokens based on the filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <returns></returns>
    Task<IEnumerable<SubscribeEventReceivedEntry>> GetAllAsync(SubscribeEventReceivedEntryFilter filter);

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
    Task RemoveAllAsync(SubscribeEventReceivedEntryFilter filter);
}
