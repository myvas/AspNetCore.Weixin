using System;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Manage their cached <see cref="WeixinAccessTokenJson"/>  for the Weixin accounts specified by 'appId'
/// </summary>
public interface IWeixinAccessTokenCacheProvidre : IWeixinCacheProvider<WeixinAccessTokenJson>
{
}
