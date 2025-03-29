namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 退订事件
/// </summary>
public class UnsubscribeEventReceivedEventArgs : WeixinEventArgs<UnsubscribeEventReceivedXml>
{
    public UnsubscribeEventReceivedEventArgs(WeixinContext context, UnsubscribeEventReceivedXml data) : base(context, data)
    {
    }
}
