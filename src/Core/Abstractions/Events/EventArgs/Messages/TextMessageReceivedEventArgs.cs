namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到文本消息
/// </summary>
public class TextMessageReceivedEventArgs : WeixinEventArgs<TextMessageReceivedXml>
{
    public TextMessageReceivedEventArgs(WeixinContext context, TextMessageReceivedXml data) : base(context, data)
    {
    }
}
