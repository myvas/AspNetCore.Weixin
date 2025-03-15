using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 微信订阅者
/// </summary>
public class Subscriber
{
    /// <summary>
    /// The constructor.
    /// </summary>
    public Subscriber()
    {

    }

    /// <summary>
    /// Update this from the input. NOTE: Both of them must have the same OpenId, otherwise nothing will change.
    /// </summary>
    /// <param name="entity">The input.</param>
    /// <returns>this</returns>
    public virtual TSubscriber Update<TSubscriber>(TSubscriber entity) where TSubscriber : Subscriber
    {
        Debug.Assert(entity.OpenId == this.OpenId, "The OpenId of both sides must be equal!");
        if (string.IsNullOrWhiteSpace(entity.OpenId) || entity.OpenId != this.OpenId) return (TSubscriber)this;

        if (!string.IsNullOrWhiteSpace(entity.AppId) && entity.AppId != this.AppId) this.AppId = entity.AppId;
        if (!string.IsNullOrWhiteSpace(entity.UnionId) && entity.UnionId != this.UnionId) this.UnionId = entity.UnionId;
        if (!string.IsNullOrWhiteSpace(entity.Nickname) && entity.Nickname != this.Nickname) this.Nickname = entity.Nickname;
        if (!string.IsNullOrWhiteSpace(entity.Remark) && entity.Remark != this.Remark) this.Remark = entity.Remark;
        if (!string.IsNullOrWhiteSpace(entity.Language) && entity.Language != this.Language) this.Language = entity.Language;
        if (entity.Gender.HasValue && entity.Gender != this.Gender) this.Gender = entity.Gender;
        if (entity.SubscribedTime.HasValue && entity.SubscribedTime != this.SubscribedTime) this.SubscribedTime = entity.SubscribedTime;
        if (entity.Unsubscribed.HasValue && entity.Unsubscribed != this.Unsubscribed) this.Unsubscribed = entity.Unsubscribed;
        if (entity.UnsubscribedTime.HasValue && entity.UnsubscribedTime != this.UnsubscribedTime) this.UnsubscribedTime = entity.UnsubscribedTime;
        if (!string.IsNullOrWhiteSpace(entity.AvatorImageUrl) && entity.AvatorImageUrl != this.AvatorImageUrl) this.AvatorImageUrl = entity.AvatorImageUrl;
        if (!string.IsNullOrWhiteSpace(entity.Province) && entity.Province != this.Province) this.Province = entity.Province;
        if (!string.IsNullOrWhiteSpace(entity.City) && entity.City != this.City) this.City = entity.City;
        if (!string.IsNullOrWhiteSpace(entity.Country) && entity.Country != this.Country) this.Country = entity.Country;
        if (!string.IsNullOrWhiteSpace(entity.UserId) && entity.UserId != this.UserId) this.UserId = entity.UserId;
        if (!string.IsNullOrWhiteSpace(entity.MentorId) && entity.MentorId != this.MentorId) this.MentorId = entity.MentorId;

        return (TSubscriber)this;
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
    /// Gets or sets the primary key for this user. It's also the subscriber id, which has one different value for the subscriber in each Weixin site/app.
    /// </summary>
    /// <remarks>Unofficially, the length is 28.</remarks>
    [Key]
    [MaxLength(32)]
    [Required]
    public string OpenId { get; set; }

    /// <summary>
    /// [Optional] The AppId, which is configured in the <see cref="WeixinOptions"/>.
    /// </summary>
    public virtual string AppId { get; set; }


    /// <summary>
    /// The <see cref="UnionId"/>, which has its same value for each subscriber in the scope of all WeixinOpen-bound apps.
    /// </summary>
    /// <remarks>Unofficially, the length is 29.</remarks>
    [MaxLength(32)]
    public string UnionId { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public WeixinGender? Gender { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; }

    /// <summary>
    /// The remark noted by operators.
    /// </summary>
    public string Remark { get; set; }

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
    /// 订阅时间
    /// </summary>
    public DateTime? SubscribedTime { get; set; }

    /// <summary>
    /// If unsubscribed?
    /// </summary>
    public bool? Unsubscribed { get; set; }

    /// <summary>
    /// The unsubscribed time.
    /// </summary>
    public DateTime? UnsubscribedTime { get; set; }

    /// <summary>
    /// [Optional] The UserId that links to a user of host application or nobody.
    /// </summary>
    /// <remarks>nullable, or maybe a wrong value that mislinks to nobody.</remarks>
    public virtual string UserId { get; set; }

    /// <summary>
    /// [Optional] The mentor's UserId that links to a user of host application or nobody.
    /// </summary>
    /// <remarks>nullable, or maybe a wrong value that mislinks to nobody.</remarks>
    public virtual string MentorId { get; set; }

    /// <summary>
    /// This column works for concurrency, the end-user doesn't see it as when it was actually updated.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; }
}
