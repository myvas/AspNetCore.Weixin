using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinSubscriber : IWeixinSubscriber<string> { }

public interface IWeixinSubscriber<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// This UserId is e foreign key, which points to your application user table.
    /// </summary>
    TKey UserId { get; set; }

    /// <summary>
    /// This MentorId is another foreign key for social relationship, which also points to your application user table.
    /// </summary>
    TKey MentorId { get; set; }

    /// <summary>
    /// This UnionId is unique for each Weixin terminal account, but it has a same value on your Weixin service accounts which related to a same Weixin Open account.
    /// </summary>
    /// <remarks>
    /// 开发者可通过OpenID来获取用户基本信息。特别需要注意的是，如果开发者拥有多个移动应用、网站应用和公众账号，可通过获取用户基本信息中的unionid来区分用户的唯一性，因为只要是同一个微信开放平台账号下的移动应用、网站应用和公众账号，用户的unionid是唯一的。换句话说，同一用户，对同一个微信开放平台下的不同应用，unionid是相同的。
    /// </remarks>
    /// <seealso href="https://developers.weixin.qq.com/doc/offiaccount/User_Management/Get_users_basic_information_UnionID.html#UinonId">See the Tencent document about UnionId</seealso>
    string UnionId { get; set; }

    /// <summary>
    /// This OpenId is unique for each Weixin terminal account on each Weixin service account.
    /// </summary>
    /// <remarks>
    /// 在关注者与公众号产生消息交互后，公众号可获得关注者的OpenID（加密后的微信号，每个用户对每个公众号的OpenID是唯一的。对于不同公众号，同一用户的openid不同）。
    // </remarks>
    /// <seealso href="https://developers.weixin.qq.com/doc/offiaccount/User_Management/Get_users_basic_information_UnionID.html#UinonId">See the Tencent document about OpenId</seealso>
    string OpenId { get; set; }

    /// <summary>
    /// Language code, such as "zh_CN", "zh_TW", "en".
    /// </summary>
    /// <seealso cref="WeixinLanguage"/>
    string Language { get; set; }

    /// <summary>
    /// The Unix time of action to subscribe.
    /// </summary>
    long? SubscribedTime { get; set; }

    /// <summary>
    /// Whether this subscriber is subscribed.
    /// </summary>
    bool Subscribed { get; set; }

    /// <summary>
    /// The Unix time of action to unsubscribe
    /// </summary>
    long? UnsubscribedTime { get; set; }

    /// <summary>
    /// The nickname of subscriber.
    /// </summary>
    string Nickname { get; set; }

    /// <summary>
    /// The remark, which is managed by your operation platform.
    /// </summary>
    /// <seealso cref="Nickname"/>
    string Remark { get; set; }

    /// <summary>
    /// The gender of subscriber.
    /// </summary>
    /// <seealso cref="WeixinGender"/>
    int? Gender { get; set; }

    /// <summary>
    /// The image url of subscriber's avator.
    /// </summary>
    string AvatorImageUrl { get; set; }

    /// <summary>
    /// The city name of subscriber declared.
    /// </summary>
    string City { get; set; }

    /// <summary>
    /// The province/state name of subscriber declared.
    /// </summary>
    string Province { get; set; }

    /// <summary>
    /// The country/zone name of subscriber declared.
    /// </summary>
    string Country { get; set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    string SecurityStamp { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    string ConcurrencyStamp { get; set; }
}
