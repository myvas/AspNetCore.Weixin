using System.Threading.Tasks;
using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinEventSink
	{
		Task<bool> OnClickMenuEventReceived(WeixinResultContext<ClickMenuEventReceivedXml> context);
        Task<bool> OnEnterEventReceived(WeixinResultContext<EnterEventReceivedXml> context);
        Task<bool> OnImageMessageReceived(WeixinResultContext<ImageMessageReceivedXml> context);
        Task<bool> OnLinkMessageReceived(WeixinResultContext<LinkMessageReceivedXml> context);
        Task<bool> OnLocationEventReceived(WeixinResultContext<LocationEventReceivedXml> context);
        Task<bool> OnLocationMessageReceived(WeixinResultContext<LocationMessageReceivedXml> context);
        Task<bool> OnQrscanEventReceived(WeixinResultContext<QrscanEventReceivedXml> context);
        Task<bool> OnShortVideoMessageReceived(WeixinResultContext<ShortVideoMessageReceivedXml> context);
        Task<bool> OnSubscribeEventReceived(WeixinResultContext<SubscribeEventReceivedXml> context);
        Task<bool> OnTextMessageReceived(WeixinResultContext<TextMessageReceivedXml> context);
        Task<bool> OnUnsubscribeEventReceived(WeixinResultContext<UnsubscribeEventReceivedXml> context);
        Task<bool> OnVideoMessageReceived(WeixinResultContext<VideoMessageReceivedXml> context);
        Task<bool> OnViewMenuEventReceived(WeixinResultContext<ViewMenuEventReceivedXml> context);
        Task<bool> OnVoiceMessageReceived(WeixinResultContext<VoiceMessageReceivedXml> context);
	}
}