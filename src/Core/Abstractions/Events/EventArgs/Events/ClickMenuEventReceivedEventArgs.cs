namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到点击菜单拉取消息事件
/// </summary>
public class ClickMenuEventReceivedEventArgs : WeixinEventArgs<ClickMenuEventReceivedXml>
{
    public ClickMenuEventReceivedEventArgs(WeixinContext context, ClickMenuEventReceivedXml data) : base(context, data)
    {
    }
}
