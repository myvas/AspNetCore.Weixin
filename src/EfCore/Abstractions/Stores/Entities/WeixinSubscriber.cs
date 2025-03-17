using System;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinSubscriber : WeixinSubscriber<string>
    {

    }
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

        public override string ToString()
            => Nickname;

        public string UnionId { get; set; }

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

        public bool Unsubscribed { get; set; }

        public DateTimeOffset? UnsubscribedTime { get; set; }

        /// <summary>
        /// [Optional] Links to Third-party IdentityUser.Id
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// [Optional] Links to Third-party IdentityUser.Id
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
}