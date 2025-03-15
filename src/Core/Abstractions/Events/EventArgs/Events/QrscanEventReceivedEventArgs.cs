namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 用户扫描带参数（场景值）二维码（扫描前已关注）
/// </summary>
public class QrscanEventReceivedEventArgs : WeixinEventArgs<QrscanEventReceivedXml>
{
    public QrscanEventReceivedEventArgs(WeixinContext context, QrscanEventReceivedXml data) : base(context, data)
    {
    }
}
