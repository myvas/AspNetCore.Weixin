using System;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinAccessTokenCacheProvider
    {
        void RemoveCachedAccessToken(string appId);
        void ReplaceCachedAccessToken(string appId, WeixinAccessTokenJson json);
        void ReplaceCachedAccessToken(string appId, string accessToken, TimeSpan absoluteExpirationRelativeToNow);
        string TryGetCachedAccessToken(string appId);
    }
}