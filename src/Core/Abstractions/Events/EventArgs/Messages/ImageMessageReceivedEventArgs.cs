namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到图片消息
/// </summary>
public class ImageMessageReceivedEventArgs : WeixinEventArgs<ImageMessageReceivedXml>
{
    public ImageMessageReceivedEventArgs(WeixinContext context, ImageMessageReceivedXml data) : base(context, data)
    {
    }
}
