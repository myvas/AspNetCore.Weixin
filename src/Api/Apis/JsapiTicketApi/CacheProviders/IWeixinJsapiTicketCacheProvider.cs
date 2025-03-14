using System;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Manage their cached <see cref="WeixinJsapiTicketJson"> for the Weixin accounts specified by 'appId'
/// </summary>
public interface IWeixinJsapiTicketCacheProvider : IWeixinCacheProvider<WeixinJsapiTicketJson>
{
}