using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinSendMessage
{
    string FromUserName { get; set; }
    string ToUserName { get; set; }
    long CreateTime { get; set; }
    string Content { get; set; }
    DateTimeOffset? ScheduleTime { get; set; }
    int? LastRetCode { get; set; }
    string LastRetMsg { get; set; }
    int RetryTimes { get; set; }
    DateTimeOffset? LastTried { get; set; }
    string ConcurrencyStamp { get; set; }
}
