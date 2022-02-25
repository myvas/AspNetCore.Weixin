using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;

/// <summary>
/// Interface for the subscriber store.
/// </summary>
public interface IWeixinUserStore
{
    /// <summary>
    /// Stores the access token.
    /// </summary>
    /// <param name="o">The object.</param>
    /// <returns></returns>
    Task StoreAsync(WeixinSubscriber o);

    /// <summary>
    /// Gets the access token.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    Task<WeixinSubscriber> GetAsync(string key);

    /// <summary>
    /// Gets all access tokens based on the filter
    /// </summary>
    /// <param name="filter">The filter</param>
    /// <returns></returns>
    Task<IEnumerable<WeixinSubscriber>> GetAllAsync(WeixinSubscriberFilter filter);

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
    Task RemoveAllAsync(WeixinSubscriberFilter filter);
}

