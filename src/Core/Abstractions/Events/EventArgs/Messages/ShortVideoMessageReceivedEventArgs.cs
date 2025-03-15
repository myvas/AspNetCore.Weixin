namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 小视频消息
/// </summary>
public class ShortVideoMessageReceivedEventArgs : WeixinEventArgs<ShortVideoMessageReceivedXml>
{
    public ShortVideoMessageReceivedEventArgs(WeixinContext context, ShortVideoMessageReceivedXml data) : base(context, data)
    {
    }
}
