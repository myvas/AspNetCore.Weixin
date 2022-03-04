using System.ComponentModel.DataAnnotations;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 微信订阅者
/// </summary>
public class Subscriber
{
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
