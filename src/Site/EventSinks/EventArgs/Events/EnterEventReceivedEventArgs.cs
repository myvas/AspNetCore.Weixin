namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 用户进入公众号
/// </summary>
public class EnterEventReceivedEventArgs : WeixinEventArgs<EnterEventReceivedXml>
{
    public EnterEventReceivedEventArgs(WeixinContext context, EnterEventReceivedXml data) : base(context, data)
    {
    }
}
