using Microsoft.EntityFrameworkCore;
using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinDbContext<TWeixinSubscriber, TKey>
    where TWeixinSubscriber : WeixinSubscriber<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Weixin subscribers
    /// </summary>
    DbSet<TWeixinSubscriber> WeixinSubscribers { get; set; }

    /// <summary>
    /// Weixin received (uplink) events
    /// </summary>
    DbSet<WeixinReceivedEvent> WeixinReceivedEvents { get; set; }

    /// <summary>
    /// Weixin received (uplink) messages
    /// </summary>
    DbSet<WeixinReceivedMessage> WeixinReceivedMessages { get; set; }

    /// <summary>
    /// Weixin passive response (uplink) messages
    /// </summary>
    DbSet<WeixinResponseMessage> WeixinResponseMessages { get; set; }

    /// <summary>
    /// Weixin active send (downlink) message
    /// </summary>
    DbSet<WeixinSendMessage> WeixinSendMessages { get; set; }
}