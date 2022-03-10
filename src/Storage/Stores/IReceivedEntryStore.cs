using Myvas.AspNetCore.Weixin.Models;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Provides an abstraction for a storage and management of <see cref="ReceivedEntry"/>.
/// </summary>
public interface IReceivedEntryStore<T> : IDisposable
        where T : ReceivedEntry
{
    /// <summary>
    /// Stores the <see cref="ReceivedEntry"/>.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    Task StoreAsync<TEntity>(TEntity item) where TEntity : T;

    /// <summary>
    /// Gets the <see cref="ReceivedEntry"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    Task<T> GetAsync(string key);

    /// <summary>
    /// Gets all <see cref="ReceivedEntry"/> based on the filter.
    /// </summary>
    /// <param name="fromUserName">The filter on <see cref="ReceivedEntry.FromUserName"/>.</param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllByFromUserNameAsync(string fromUserName);

    /// <summary>
    /// Get all <see cref="ReceivedEntry"/> based on the filter.
    /// </summary>
    /// <param name="startTime">The filter on <see cref="ReceivedEntry.CreateUnixTime"/></param>
    /// <param name="endTime">The filter on <see cref="ReceivedEntry.CreateUnixTime"/></param>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllByReceivedTimeAsync(DateTime? startTime, DateTime? endTime);
}