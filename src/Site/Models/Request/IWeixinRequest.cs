namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinRequest : IWeixinMessage
    {
        RequestMsgType MsgType { get; }
        long MsgId { get; set; }
    }
}
