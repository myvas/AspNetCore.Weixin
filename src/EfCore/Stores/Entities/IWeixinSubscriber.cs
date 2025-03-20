using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinSubscriber : IWeixinSubscriber<string>
{

}

public interface IWeixinSubscriber<TKey> where TKey : IEquatable<TKey>
{
    string UnionId { get; set; }
    string OpenId { get; set; }
    int? Gender { get; set; }
    string Nickname { get; set; }
    string City { get; set; }
    string Province { get; set; }
    string Country { get; set; }
    string Language { get; set; }
    string AvatorImageUrl { get; set; }
    string Remark { get; set; }
    long? SubscribedTime { get; set; }
    bool Unsubscribed { get; set; }
    long? UnsubscribedTime { get; set; }
    TKey UserId { get; set; }
    TKey MentorId { get; set; }
    string SecurityStamp { get; set; }
    string ConcurrencyStamp { get; set; }
}
