using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// This class contains all events and messages from Weixin users.
/// </summary>
/// <remarks>
/// Whenever you received a Weixin event/message, an HTTP response must be sent back to the origin with status code 200;
// otherwise Tencent will send a message of "Official account services unavailable. Try again later."
// </remarks>
public interface IWeixinEventHandler
{
    Task<bool> ClickMenuEventReceived(WeixinReceivedContext<ClickMenuEventReceivedXml> context);
    Task<bool> ImageMessageReceived(WeixinReceivedContext<ImageMessageReceivedXml> context);
    Task<bool> LinkMessageReceived(WeixinReceivedContext<LinkMessageReceivedXml> context);
    Task<bool> LocationEventReceived(WeixinReceivedContext<LocationEventReceivedXml> context);
    Task<bool> LocationMessageReceived(WeixinReceivedContext<LocationMessageReceivedXml> context);
    Task<bool> QrscanEventReceived(WeixinReceivedContext<QrscanEventReceivedXml> context);
    Task<bool> ShortVideoMessageReceived(WeixinReceivedContext<ShortVideoMessageReceivedXml> context);
    Task<bool> SubscribeEventReceived(WeixinReceivedContext<SubscribeEventReceivedXml> context);
    Task<bool> TextMessageReceived(WeixinReceivedContext<TextMessageReceivedXml> context);
    Task<bool> UnsubscribeEventReceived(WeixinReceivedContext<UnsubscribeEventReceivedXml> context);
    Task<bool> VideoMessageReceived(WeixinReceivedContext<VideoMessageReceivedXml> context);
    Task<bool> ViewMenuEventReceived(WeixinReceivedContext<ViewMenuEventReceivedXml> context);
    Task<bool> VoiceMessageReceived(WeixinReceivedContext<VoiceMessageReceivedXml> context);
}
