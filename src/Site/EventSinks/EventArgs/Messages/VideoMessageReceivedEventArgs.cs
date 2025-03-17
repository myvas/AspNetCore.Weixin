namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 视频消息
/// </summary>
public class VideoMessageReceivedEventArgs : WeixinEventArgs<VideoMessageReceivedXml>
{
    public VideoMessageReceivedEventArgs(WeixinContext context, VideoMessageReceivedXml data) : base(context, data)
    {
    }
}
