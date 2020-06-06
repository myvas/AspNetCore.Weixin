namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinResponse : IWeixinMessage
    {
        ResponseMsgType MsgType { get; }
    }
}
