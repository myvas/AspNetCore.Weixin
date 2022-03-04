using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Myvas.AspNetCore.Weixin;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.DbContexts;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Hosts;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Options;
using Myvas.AspNetCore.Weixin.EntityFrameworkCore.Stores;
using Myvas.AspNetCore.Weixin.Models;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Contains extension methods to <see cref="IWeixinBuilder"/> for <see cref="IHostedService"/>.
/// </summary>
public static class WeixinEntityFrameworCoreBuilderExtensions
{
    /// <summary>
    /// Configures an <see cref="IHostedService"/>.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns></returns>
    public static IWeixinBuilder AddCleanupHost(
        this IWeixinBuilder builder)
    {
        builder.Services.AddSingleton<IHostedService, TokenCleanupHost>();

        return builder;
    }
}
