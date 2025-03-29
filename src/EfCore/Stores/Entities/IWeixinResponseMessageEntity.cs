namespace Myvas.AspNetCore.Weixin;

public interface IWeixinResponseMessageEntity : IEntity
{
    string FromUserName { get; set; }
    string ToUserName { get; set; }
    long? CreateTime { get; set; }
    string MsgType { get; set; }
    string MsgId { get; set; }
    string Content { get; set; }
    string PicUrl { get; set; }
    string MediaId { get; set; }
    string Format { get; set; }
    string ThumbMediaId { get; set; }
    decimal Longitude { get; set; }
    decimal Latitude { get; set; }
    decimal Scale { get; set; }
    string Label { get; set; }
    string Title { get; set; }
    string Url { get; set; }
    string Description { get; set; }
    string RequestId { get; set; }
    string ConcurrencyStamp { get; set; }
}
