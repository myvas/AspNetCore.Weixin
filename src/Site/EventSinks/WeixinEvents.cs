using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public class WeixinEvents
{
    public Func<object, WeixinEventArgs<ImageMessageReceivedXml>, Task<bool>> OnImageMessageReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<ClickMenuEventReceivedXml>, Task<bool>> OnClickMenuEventReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<ViewMenuEventReceivedXml>, Task<bool>> OnViewMenuEventReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<UnsubscribeEventReceivedXml>, Task<bool>> OnUnsubscribeEventReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<SubscribeEventReceivedXml>, Task<bool>> OnSubscribeEventReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<QrscanEventReceivedXml>, Task<bool>> OnQrscanEventReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<TextMessageReceivedXml>, Task<bool>> OnTextMessageReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<LinkMessageReceivedXml>, Task<bool>> OnLinkMessageReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<VideoMessageReceivedXml>, Task<bool>> OnVideoMessageReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<ShortVideoMessageReceivedXml>, Task<bool>> OnShortVideoMessageReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<VoiceMessageReceivedXml>, Task<bool>> OnVoiceMessageReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<LocationMessageReceivedXml>, Task<bool>> OnLocationMessageReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<LocationEventReceivedXml>, Task<bool>> OnLocationEventReceived { get; set; } = (s, e) => Task.FromResult(false);
    public Func<object, WeixinEventArgs<EnterEventReceivedXml>, Task<bool>> OnEnterEventReceived { get; set; } = (s, e) => Task.FromResult(false);
}
