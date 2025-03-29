using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinEventSink
{
    #region Uplink/Messages
    Task<bool> OnTextMessageReceived(object sender, WeixinEventArgs<TextMessageReceivedXml> e);
    Task<bool> OnLinkMessageReceived(object sender, WeixinEventArgs<LinkMessageReceivedXml> e);
    Task<bool> OnVideoMessageReceived(object sender, WeixinEventArgs<VideoMessageReceivedXml> e);
    Task<bool> OnShortVideoMessageReceived(object sender, WeixinEventArgs<ShortVideoMessageReceivedXml> e);
    Task<bool> OnVoiceMessageReceived(object sender, WeixinEventArgs<VoiceMessageReceivedXml> e);
    Task<bool> OnImageMessageReceived(object sender, WeixinEventArgs<ImageMessageReceivedXml> e);
    Task<bool> OnLocationMessageReceived(object sender, WeixinEventArgs<LocationMessageReceivedXml> e);
    #endregion
    #region Uplink/Events
    Task<bool> OnLocationEventReceived(object sender, WeixinEventArgs<LocationEventReceivedXml> e);
    Task<bool> OnClickMenuEventReceived(object sender, WeixinEventArgs<ClickMenuEventReceivedXml> e);
    Task<bool> OnViewMenuEventReceived(object sender, WeixinEventArgs<ViewMenuEventReceivedXml> e);
    Task<bool> OnUnsubscribeEventReceived(object sender, WeixinEventArgs<UnsubscribeEventReceivedXml> e);
    Task<bool> OnQrscanEventReceived(object sender, WeixinEventArgs<QrscanEventReceivedXml> e);
    Task<bool> OnSubscribeEventReceived(object sender, WeixinEventArgs<SubscribeEventReceivedXml> e);
    Task<bool> OnEnterEventReceived(object sender, WeixinEventArgs<EnterEventReceivedXml> e);
    #endregion
}