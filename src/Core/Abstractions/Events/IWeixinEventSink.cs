using System.Threading.Tasks;
using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin
{
	public interface IWeixinEventSink
	{
		Task<bool> OnClickMenuEventReceived(object sender, ClickMenuEventReceivedXml e);
        Task<bool> OnEnterEventReceived(object sender, EnterEventReceivedXml e);
        Task<bool> OnImageMessageReceived(object sender, ImageMessageReceivedXml e);
        Task<bool> OnLinkMessageReceived(object sender, LinkMessageReceivedXml e);
        Task<bool> OnLocationEventReceived(object sender, LocationEventReceivedXml e);
        Task<bool> OnLocationMessageReceived(object sender, LocationMessageReceivedXml e);
        Task<bool> OnQrscanEventReceived(object sender, QrscanEventReceivedXml e);
        Task<bool> OnShortVideoMessageReceived(object sender, ShortVideoMessageReceivedXml e);
        Task<bool> OnSubscribeEventReceived(object sender, SubscribeEventReceivedXml e);
        Task<bool> OnTextMessageReceived(object sender, TextMessageReceivedXml e);
        Task<bool> OnUnsubscribeEventReceived(object sender, UnsubscribeEventReceivedXml e);
        Task<bool> OnVideoMessageReceived(object sender, VideoMessageReceivedXml e);
        Task<bool> OnViewMenuEventReceived(object sender, ViewMenuEventReceivedXml e);
        Task<bool> OnVoiceMessageReceived(object sender, VoiceMessageReceivedXml e);
	}
}