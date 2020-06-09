namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinReceived
    {
        string FromUserName { get; set; }
        string ToUserName { get; set; }
        string CreateTime { get; set; }
        string MsgType { get; set; }
    }
}