using System.ComponentModel.DataAnnotations;

namespace Myvas.AspNetCore.Weixin.Models;

/// <summary>
/// 微信订阅者
/// </summary>
public class WeixinSubscriber
{
    /// <summary>
    /// 
    /// </summary>
    public WeixinSubscriber()
    {
        Id = Guid.NewGuid().ToString("N");
        SecurityStamp = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="openId"></param>
    public WeixinSubscriber(string openId) : this()
    {
        OpenId = openId;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="openId"></param>
    /// <param name="unionId"></param>
    public WeixinSubscriber(string openId, string unionId) : this()
    {
        OpenId = openId;
        UnionId = unionId;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => Nickname;

    /// <summary>
    /// Gets or sets the primary key for this user.
    /// </summary>
    [Key]
    public string Id { get; set; }

    /// <summary>
    /// The unique id of WeixinUser in all related Weixin site/apps.
    /// </summary>
    /// <remarks>Unofficially, the length is 29.</remarks>
    [MaxLength(32)]
    public string UnionId { get; set; }

    /// <summary>
    /// The unique id of WeixinUser in each Weixin site/app.
    /// </summary>
    /// <remarks>Unofficially, the length is 28.</remarks>
    [MaxLength(32)]
    [Required]
    public string OpenId { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public WeixinGender? Gender { get; set; }

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
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    public virtual string SecurityStamp { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}
