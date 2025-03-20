using Microsoft.EntityFrameworkCore;
using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinDbContext : IWeixinDbContext<WeixinSubscriberEntity>
{
}

public interface IWeixinDbContext<TWeixinSubscriber> : IWeixinDbContext<TWeixinSubscriber, string>
    where TWeixinSubscriber : class, IWeixinSubscriber<string>
{
}

public interface IWeixinDbContext<TWeixinSubscriber, TKey>
    where TWeixinSubscriber : class, IWeixinSubscriber<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Weixin subscribers.
    /// </summary>
    DbSet<TWeixinSubscriber> WeixinSubscribers { get; set; }

    /// <summary>
    /// Weixin received (uplink) events
    /// </summary>
    DbSet<WeixinReceivedEventEntity> WeixinReceivedEvents { get; set; }

    /// <summary>
    /// Weixin received (uplink) messages
    /// </summary>
    DbSet<WeixinReceivedMessageEntity> WeixinReceivedMessages { get; set; }

    /// <summary>
    /// Weixin passive response (uplink) messages
    /// </summary>
    DbSet<WeixinResponseMessageEntity> WeixinResponseMessages { get; set; }

    /// <summary>
    /// Weixin active send (downlink) message
    /// </summary>
    DbSet<WeixinSendMessageEntity> WeixinSendMessages { get; set; }
}