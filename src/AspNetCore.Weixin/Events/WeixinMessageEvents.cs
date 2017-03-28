using AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    public class WeixinMessageEvents
    {
        public Func<WeixinReceivedContext<ImageMessageReceivedEventArgs>, bool> OnImageMessageReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<ClickMenuEventReceivedEventArgs>, bool> OnClickMenuEventReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<ViewMenuEventReceivedEventArgs>, bool> OnViewMenuEventReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<UnsubscribeEventReceivedEventArgs>, bool> OnUnsubscribeEventReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<SubscribeEventReceivedEventArgs>, bool> OnSubscribeEventReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<QrscanEventReceivedEventArgs>, bool> OnQrscanEventReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<TextMessageReceivedEventArgs>, bool> OnTextMessageReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<LinkMessageReceivedEventArgs>, bool> OnLinkMessageReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<VideoMessageReceivedEventArgs>, bool> OnVideoMessageReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<VoiceMessageReceivedEventArgs>, bool> OnVoiceMessageReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<LocationMessageReceivedEventArgs>, bool> OnLocationMessageReceived { get; set; } = e => false;
        public Func<WeixinReceivedContext<LocationEventReceivedEventArgs>, bool> OnLocationEventReceived { get; set; } = e => false;
    }
}
