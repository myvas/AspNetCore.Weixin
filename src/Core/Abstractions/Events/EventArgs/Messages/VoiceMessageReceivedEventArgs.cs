namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到语音消息
/// </summary>
public class VoiceMessageReceivedEventArgs : WeixinEventArgs<VoiceMessageReceivedXml>
{
    public VoiceMessageReceivedEventArgs(WeixinContext context, VoiceMessageReceivedXml data) : base(context, data)
    {
    }
}
