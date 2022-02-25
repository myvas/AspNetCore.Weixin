using System;

namespace Myvas.AspNetCore.Weixin.Models;

#pragma warning disable 1591

public class WeixinSubscriber
{
    /// <summary>
    /// [Optional] The mentor's UserId that links to a user of host application or nobody.
    /// </summary>
    /// <remarks>nullable, or maybe a wrong value that mislinks to nobody.</remarks>
    public virtual string MentorId { get; set; }

    /// <summary>
    /// [Optional] The UserId that links to a user of host application or nobody.
    /// </summary>
    /// <remarks>nullable, or maybe a wrong value that mislinks to nobody.</remarks>
    public string UserId { get; set; }

    /// <summary>
    /// The unique id of WeixinUser in all related Weixin site/apps.
    /// </summary>
    public string UnionId { get; set; }

    /// <summary>
    /// The unique id of WeixinUser in each Weixin site/app.
    /// </summary>
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
    /// 订阅时间
    /// </summary>
    public DateTimeOffset? SubscribedTime { get; set; }

    /// <summary>
    /// If unsubscribed?
    /// </summary>
    public bool Unsubscribed { get; set; }

    /// <summary>
    /// The unsubscribed time.
    /// </summary>
    public DateTimeOffset? UnsubscribedTime { get; set; }
}
