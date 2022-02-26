using System.Threading.Tasks;
using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinEventSink
	{
		Task<bool> OnClickMenuEventReceived(WeixinEventContext<ClickMenuEventReceivedXml> context);
        Task<bool> OnEnterEventReceived(WeixinEventContext<EnterEventReceivedXml> context);
        Task<bool> OnImageMessageReceived(WeixinEventContext<ImageMessageReceivedXml> context);
        Task<bool> OnLinkMessageReceived(WeixinEventContext<LinkMessageReceivedXml> context);
        Task<bool> OnLocationEventReceived(WeixinEventContext<LocationEventReceivedXml> context);
        Task<bool> OnLocationMessageReceived(WeixinEventContext<LocationMessageReceivedXml> context);
        Task<bool> OnQrscanEventReceived(WeixinEventContext<QrscanEventReceivedXml> context);
        Task<bool> OnShortVideoMessageReceived(WeixinEventContext<ShortVideoMessageReceivedXml> context);
        Task<bool> OnSubscribeEventReceived(WeixinEventContext<SubscribeEventReceivedXml> context);
        Task<bool> OnTextMessageReceived(WeixinEventContext<TextMessageReceivedXml> context);
        Task<bool> OnUnsubscribeEventReceived(WeixinEventContext<UnsubscribeEventReceivedXml> context);
        Task<bool> OnVideoMessageReceived(WeixinEventContext<VideoMessageReceivedXml> context);
        Task<bool> OnViewMenuEventReceived(WeixinEventContext<ViewMenuEventReceivedXml> context);
        Task<bool> OnVoiceMessageReceived(WeixinEventContext<VoiceMessageReceivedXml> context);
	}
}