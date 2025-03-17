namespace Myvas.AspNetCore.Weixin;

public interface IWeixinResponseMessage : IWeixinMessage
{
    ResponseMsgType MsgType { get; }
    string ToXml();
}
