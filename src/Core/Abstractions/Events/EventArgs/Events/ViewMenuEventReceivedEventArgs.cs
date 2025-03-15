namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到点击菜单跳转链接事件
/// </summary>
public class ViewMenuEventReceivedEventArgs : WeixinEventArgs<ViewMenuEventReceivedXml>
{
    public ViewMenuEventReceivedEventArgs(WeixinContext context, ViewMenuEventReceivedXml data) : base(context, data)
    {
    }
}
