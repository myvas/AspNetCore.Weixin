namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到链接消息
/// </summary>
public class LinkMessageReceivedEventArgs : WeixinEventArgs<LinkMessageReceivedXml>
{
    public LinkMessageReceivedEventArgs(WeixinContext context, LinkMessageReceivedXml data) : base(context, data)
    {
    }
}
