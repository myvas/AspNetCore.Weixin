using Microsoft.EntityFrameworkCore;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinDbContext<TWeixinSubscriber, TKey>
        where TWeixinSubscriber : WeixinSubscriber<TKey>
        where TKey : IEquatable<TKey>
    {
        DbSet<WeixinReceivedEvent> ReceivedEvents { get; set; }
        DbSet<WeixinReceivedMessage> ReceivedMessages { get; set; }
        DbSet<WeixinResponseMessage> ResponseMessages { get; set; }
        DbSet<WeixinSendMessage> SendMessages { get; set; }
        DbSet<TWeixinSubscriber> Subscribers { get; set; }
    }
}