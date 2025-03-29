namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 订阅事件。或，用户扫描带参数（场景值）二维码（扫描前未关注）。
/// </summary>
public class SubscribeEventReceivedEventArgs : WeixinEventArgs<SubscribeEventReceivedXml>
{
    public SubscribeEventReceivedEventArgs(WeixinContext context, SubscribeEventReceivedXml data) : base(context, data)
    {
    }
}
