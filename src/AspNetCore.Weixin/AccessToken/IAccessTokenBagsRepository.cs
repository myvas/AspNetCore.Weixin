using System;
namespace AspNetCore.Weixin
{
    interface IAccessTokenBagsRepository
    {
        string GetToken(string appId);
        bool IsExpired(string appId);
        AccessTokenBag Load(string appId);
        void SetExpired(string appId);
        void Store(string appId, AccessTokenBag bag);
    }
}
