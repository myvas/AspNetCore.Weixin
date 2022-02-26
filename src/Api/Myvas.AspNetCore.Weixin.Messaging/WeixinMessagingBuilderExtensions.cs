using Microsoft.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Interfaces;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;

namespace Microsoft.Extensions.DependencyInjection;

public static class WeixinMessagingBuilderExtensions
{
  public static IWeixinBuilder AddMessenger(
      this IWeixinBuilder builder)
  {
    if (builder == null)
    {
      throw new ArgumentNullException(nameof(builder));
    }

    builder.Services.AddWeixinResponseBuilder();

    return builder;
  }
}