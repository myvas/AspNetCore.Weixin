using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinResponseMessageEntity : Entity
{
    public virtual string FromUserName { get; set; }
    public virtual string ToUserName { get; set; }
    public virtual string CreateTime { get; set; }
    public virtual string MsgType { get; set; }

    public virtual string MsgId { get; set; }
    public virtual string Content { get; set; }
    public virtual string PicUrl { get; set; }
    public virtual string MediaId { get; set; }
    public virtual string Format { get; set; }
    public virtual string ThumbMediaId { get; set; }
    public virtual decimal Longitude { get; set; }
    public virtual decimal Latitude { get; set; }
    public virtual decimal Scale { get; set; }
    public virtual string Label { get; set; }
    public virtual string Title { get; set; }
    public virtual string Url { get; set; }
    public virtual string Description { get; set; }

    /// <summary>
    /// The Id of request message which associated with this response message
    /// </summary>
    public virtual string RequestId { get; set; }

    /// <summary>
    /// A random value that must change whenever a user is persisted to the store
    /// </summary>
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
}