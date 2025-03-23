using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinReceivedEventEntity : Entity, IWeixinReceivedEventEntity
{
    public virtual string FromUserName { get; set; }
    public virtual string ToUserName { get; set; }
    public virtual long? CreateTime { get; set; }
    public virtual string MsgType { get; set; }
    public virtual string Event { get; set; }
    public virtual string EventKey { get; set; }
    public virtual string Ticket { get; set; }
    public virtual decimal? Longitude { get; set; }
    public virtual decimal? Latitude { get; set; }
    public virtual decimal? Precision { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}