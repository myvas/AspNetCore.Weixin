using System;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinMessage
    {
        string ToUserName { get; set; }
        string FromUserName { get; set; }
        DateTime CreateTime { get; set; }
    }
}
