using System;
using System.ComponentModel.DataAnnotations;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// The Weixin subscriber with a string-typed primary key (named <see cref="Id"/>), who might have one or two foreign key(s) (<see cref="UserId"/> and <see cref="MentorId"/>) link to the master app's UserId with a primary key type of <see cref="TKey"/>.
/// </summary>
public class WeixinSubscriber : WeixinSubscriber<string>
{

}

/// <summary>
/// The Weixin subscriber with a string-typed primary key (named <see cref="Id"/>), who might have one or two foreign key(s) (<see cref="UserId"/> and <see cref="MentorId"/>) link to the master app's UserId with a primary key type of <see cref="TKey"/>.
/// </summary>
/// <typeparam name="TKey">The type of <see cref="UserId"/> and <see cref="MentorId"/>, which are both related to AppUsers. NOTE: It's not the key type of this entity!</typeparam>
public class WeixinSubscriber<TKey> : Entity
    where TKey : IEquatable<TKey>
{
    public WeixinSubscriber() : base()
    {
        SecurityStamp = Guid.NewGuid().ToString();
    }

    public WeixinSubscriber(string openId) : this()
    {
        OpenId = openId;
    }

    public WeixinSubscriber(string openId, string unionId) : this()
    {
        OpenId = openId;
        UnionId = unionId;
    }

    /// <summary>
    /// Gets the display name in this order: (1)<see cref="Remark"/>; (2)<see cref="Nickname"/>; (3)<see cref="OpenId"/>.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var displayName = Remark;
        if (string.IsNullOrWhiteSpace(displayName)) displayName = Nickname;
        if (string.IsNullOrWhiteSpace(displayName)) displayName = OpenId;
        return displayName;
    }

    /// <summary>
    /// The UnionId keeps the same value for each subscriber among a series of WeixinOpen-bound apps.
    /// </summary>
    [MaxLength(32)]
    public string UnionId { get; set; }

    /// <summary>
    /// The OpenId points to a unique Weixin subscriber.
    /// </summary>
    [MaxLength(32)]
    public string OpenId { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public WeixinGender Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// 地级市区县
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// 省州盟
    /// </summary>
    public string Province { get; set; }

    /// <summary>
    /// 国家地区
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// 语言
    /// </summary>
    public string Language { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string AvatorImageUrl { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>Defined by the operators.</remark>
    public string Remark { get; set; }

    /// <summary>
    /// 订阅时间
    /// </summary>
    public DateTimeOffset? SubscribedTime { get; set; }

    /// <summary>
    /// Whether the subscriber has unsubscribed from this Weixin subscripation list
    /// </summary>
    public bool Unsubscribed { get; set; }

    public DateTimeOffset? UnsubscribedTime { get; set; }

    /// <summary>
    /// [Optional] Links to the master app's UserId
    /// </summary>
    public virtual TKey UserId { get; set; }

    /// <summary>
    /// [Optional] Links to the master app's UserId
    /// </summary>
    public virtual TKey MentorId { get; set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    public virtual string SecurityStamp { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}