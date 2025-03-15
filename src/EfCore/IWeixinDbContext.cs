using Microsoft.EntityFrameworkCore;
using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinDbContext<TWeixinSubscriber, TKey>
    where TWeixinSubscriber : WeixinSubscriber<TKey>
    where TKey : IEquatable<TKey>
{
    DbSet<TWeixinSubscriber> WeixinSubscribers { get; set; }

    DbSet<WeixinReceivedEvent> WeixinReceivedEvents { get; set; }
    DbSet<WeixinReceivedMessage> WeixinReceivedMessages { get; set; }
    DbSet<WeixinResponseMessage> WeixinResponseMessages { get; set; }
    DbSet<WeixinSendMessage> WeixinSendMessages { get; set; }
}