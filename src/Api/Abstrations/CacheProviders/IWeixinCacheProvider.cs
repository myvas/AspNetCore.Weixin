namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Manage their cached <see cref="WeixinJson"> object for the Weixin accounts specified by 'appId'
/// </summary>
public interface IWeixinCacheProvider<T>
{
    /// <summary>
    /// Get the T object from the cache
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    T Get(string appId);

    /// <summary>
    /// Remove the cached T object
    /// </summary>
    /// <param name="appId"></param>
    void Remove(string appId);

    /// <summary>
    /// Replace the cached T object by a new one inherited from <see cref="WeixinJson"/>
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="json"></param>
    void Replace(string appId, T json);
}