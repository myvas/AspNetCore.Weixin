using System;
using System.ComponentModel.DataAnnotations;

namespace Myvas.AspNetCore.Weixin;

public class WeixinSubscriberEntity : WeixinSubscriberEntity<string>, IWeixinSubscriberEntity
{

}


/// <summary>
/// The Weixin subscriber.
/// </summary>
public class WeixinSubscriberEntity<TKey> : Entity, IWeixinSubscriberEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public WeixinSubscriberEntity() : base()
    {
        SecurityStamp = Guid.NewGuid().ToString();
        ConcurrencyStamp = Guid.NewGuid().ToString();
    }

    public WeixinSubscriberEntity(string openId) : this()
    {
        OpenId = openId;
    }

    public WeixinSubscriberEntity(string openId, string unionId) : this()
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
    /// The foreign key associated with your application user.
    /// </summary>
    public virtual TKey UserId { get; set; }

    /// <summary>
    /// The foreign key associated with your application user as a mentor.
    /// </summary>
    public virtual TKey MentorId { get; set; }

    /// <summary>
    /// The UnionId keeps the same value for each subscriber among a series of WeixinOpen-bound apps.
    /// </summary>
    [MaxLength(32)]
    public virtual string UnionId { get; set; }

    /// <summary>
    /// The OpenId points to a unique Weixin subscriber.
    /// </summary>
    [MaxLength(32)]
    public virtual string OpenId { get; set; }

    /// <summary>
    /// 性别，null,0其他，1男，2女。
    /// </summary>
    public virtual int? Sex { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public virtual string Nickname { get; set; }

    /// <summary>
    /// 地级市区县
    /// </summary>
    public virtual string City { get; set; }

    /// <summary>
    /// 省州盟
    /// </summary>
    public virtual string Province { get; set; }

    /// <summary>
    /// 国家地区
    /// </summary>
    public virtual string Country { get; set; }

    /// <inheritdoc/>
    public virtual string Language { get; set; }

    /// <inheritdoc/>
    public virtual string HeadImgUrl { get; set; }

    /// <inheritdoc/>
    public virtual string Remark { get; set; }

    /// <inheritdoc/>
    public virtual long? SubscribeTime { get; set; }

    /// <inheritdoc/>
    public virtual bool Subscribed { get; set; }

    /// <inheritdoc/>
    public virtual long? UnsubscribeTime { get; set; }

    /// <inheritdoc/>
    public virtual string SecurityStamp { get; set; }

    /// <inheritdoc/>
    public virtual string ConcurrencyStamp { get; set; }
}