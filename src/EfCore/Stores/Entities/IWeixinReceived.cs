namespace Myvas.AspNetCore.Weixin;

public interface IWeixinReceived
{
    string FromUserName { get; set; }
    string ToUserName { get; set; }
    long? CreateTime { get; set; }
    string MsgType { get; set; }
}