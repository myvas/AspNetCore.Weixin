using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinSendMessageEntity : IEntity
{
    string FromUserName { get; set; }
    string ToUserName { get; set; }
    long? CreateTime { get; set; }
    string Content { get; set; }
    long? ScheduleTime { get; set; }
    int? LastRetCode { get; set; }
    string LastRetMsg { get; set; }
    int RetryTimes { get; set; }
    long? LastTried { get; set; }
    string ConcurrencyStamp { get; set; }
}
