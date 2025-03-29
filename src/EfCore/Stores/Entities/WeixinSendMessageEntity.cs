using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinSendMessageHistoryEntity : WeixinSendMessageEntity
{
}

/// <summary>
/// A message to be send, which must match one of the pre-configured templates in mp.weixin.qq.com
/// <seealso cref="WeixinSendMessageHistoryEntity"/>
/// </summary>
public class WeixinSendMessageEntity : Entity, IWeixinSendMessageEntity
{
    public virtual string FromUserName { get; set; }
    public virtual string ToUserName { get; set; }
    public virtual long? CreateTime { get; set; }

    public virtual string Content { get; set; }

    /// <summary>
    /// send immediately if null
    /// </summary>
    public virtual DateTimeOffset? ScheduleTime { get; set; }

    /// <summary>
    /// not send yet if null
    /// </summary>
    public virtual int? LastRetCode { get; set; }
    public virtual string LastRetMsg { get; set; }
    public virtual int RetryTimes { get; set; }
    public virtual DateTimeOffset? LastTried { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
    long? IWeixinSendMessageEntity.ScheduleTime { get;set;}
    long? IWeixinSendMessageEntity.LastTried { get;set;}
}