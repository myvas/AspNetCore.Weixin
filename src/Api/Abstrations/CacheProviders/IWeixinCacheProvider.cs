using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinCacheProvider
{
    /// <summary>
    /// Get the T object from the cache
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    T Get<T>(string appId) where T : IWeixinExpirableValue, new();

    /// <summary>
    /// Remove the cached T object
    /// </summary>
    /// <param name="appId"></param>
    void Remove<T>(string appId);

    /// <summary>
    /// Replace the cached T object by a new one inherited from <see cref="WeixinJson"/>
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="json"></param>
    /// <returns>true: ensure the value stored, false: the store operation failed</returns>
    bool Replace<T>(string appId, T json) where T : IWeixinExpirableValue;
}

/// <summary>
/// Manage their cached <see cref="WeixinJson"> object for the Weixin accounts specified by 'appId'
/// </summary>
public interface IWeixinCacheProvider<T> where T : new()
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
    /// <returns>true: ensure the value stored, false: the store operation failed</returns>
    bool Replace(string appId, T json, TimeSpan expiresIn);
}