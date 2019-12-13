using System.Threading.Tasks;
using Myvas.AspNetCore.Weixin;

namespace Myvas.AspNetCore.Weixin
{
	public interface IWeixinEventSink
	{
		Task<bool> OnClickMenuEventReceived(object sender, ClickMenuEventReceivedEventArgs e);
        Task<bool> OnEnterEventReceived(object sender, EnterEventReceivedEventArgs e);
        Task<bool> OnImageMessageReceived(object sender, ImageMessageReceivedEventArgs e);
        Task<bool> OnLinkMessageReceived(object sender, LinkMessageReceivedEventArgs e);
        Task<bool> OnLocationEventReceived(object sender, LocationEventReceivedEventArgs e);
        Task<bool> OnLocationMessageReceived(object sender, LocationMessageReceivedEventArgs e);
        Task<bool> OnQrscanEventReceived(object sender, QrscanEventReceivedEventArgs e);
        Task<bool> OnShortVideoMessageReceived(object sender, ShortVideoMessageReceivedEventArgs e);
        Task<bool> OnSubscribeEventReceived(object sender, SubscribeEventReceivedEventArgs e);
        Task<bool> OnTextMessageReceived(object sender, TextMessageReceivedEventArgs e);
        Task<bool> OnUnsubscribeEventReceived(object sender, UnsubscribeEventReceivedEventArgs e);
        Task<bool> OnVideoMessageReceived(object sender, VideoMessageReceivedEventArgs e);
        Task<bool> OnViewMenuEventReceived(object sender, ViewMenuEventReceivedEventArgs e);
        Task<bool> OnVoiceMessageReceived(object sender, VoiceMessageReceivedEventArgs e);
	}
}