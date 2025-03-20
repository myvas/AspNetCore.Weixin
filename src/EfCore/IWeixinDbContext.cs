using Microsoft.EntityFrameworkCore;
using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinDbContext : IWeixinDbContext<WeixinSubscriberEntity>
{
}

public interface IWeixinDbContext<TWeixinSubscriberEntity> : IWeixinDbContext<TWeixinSubscriberEntity, string>
    where TWeixinSubscriberEntity : class, IWeixinSubscriber<string>, IEntity
{
}

public interface IWeixinDbContext<TWeixinSubscriberEntity, TKey>
    where TWeixinSubscriberEntity : class, IWeixinSubscriber<TKey>, IEntity
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Weixin subscribers.
    /// </summary>
    DbSet<TWeixinSubscriberEntity> WeixinSubscribers { get; set; }

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