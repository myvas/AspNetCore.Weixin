using System;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Manage their cached access token for the Weixin accounts specified by 'appId'
/// </summary>
public interface IWeixinJsapiTicketCacheProvider
{
    /// <summary>
    /// Get the cached access token
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    WeixinJsapiTicketJson Get(string appId);

    /// <summary>
    /// Remove the cached access token
    /// </summary>
    /// <param name="appId"></param>
    void Remove(string appId);

    /// <summary>
    /// Replace the cached access token by a new one from <see cref="WeixinJsapiTicketJson"/>
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="json"></param>
    void Replace(string appId, WeixinJsapiTicketJson json);

    /// <summary>
    /// Replace the cached access token by a new one, and set its expiration time
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="json"></param>
    void Replace(string appId, string jsapiTicket, TimeSpan absoluteExpirationRelativeToNow);
}