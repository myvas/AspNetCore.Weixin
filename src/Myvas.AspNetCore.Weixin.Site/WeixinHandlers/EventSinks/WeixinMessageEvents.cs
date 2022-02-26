using Myvas.AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// "Official account services unavailable. Try again later." 
    /// will be response while firing an event that returns "false".
    /// </summary>
    public class WeixinMessageEvents
    {
        public Func<WeixinResultContext<ImageMessageReceivedXml>, Task<bool>> OnImageMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<ClickMenuEventReceivedXml>, Task<bool>> OnClickMenuEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<ViewMenuEventReceivedXml>, Task<bool>> OnViewMenuEventReceived { get; set; } = e => Task.FromResult(false);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public Func<WeixinResultContext<EnterEventReceivedXml>, Task<bool>> OnEnterEventReceived { get; set; } = e => Task.FromResult(false);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public Func<WeixinResultContext<UnsubscribeEventReceivedXml>, Task<bool>> OnUnsubscribeEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<SubscribeEventReceivedXml>, Task<bool>> OnSubscribeEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<QrscanEventReceivedXml>, Task<bool>> OnQrscanEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<TextMessageReceivedXml>, Task<bool>> OnTextMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<LinkMessageReceivedXml>, Task<bool>> OnLinkMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<VideoMessageReceivedXml>, Task<bool>> OnVideoMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<ShortVideoMessageReceivedXml>, Task<bool>> OnShortVideoMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<VoiceMessageReceivedXml>, Task<bool>> OnVoiceMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<LocationMessageReceivedXml>, Task<bool>> OnLocationMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinResultContext<LocationEventReceivedXml>, Task<bool>> OnLocationEventReceived { get; set; } = e => Task.FromResult(false);

        public virtual Task<bool> ImageMessageReceived(WeixinResultContext<ImageMessageReceivedXml> context) => OnImageMessageReceived(context);
        public virtual Task<bool> ClickMenuEventReceived(WeixinResultContext<ClickMenuEventReceivedXml> context) => OnClickMenuEventReceived(context);
        public virtual Task<bool> ViewMenuEventReceived(WeixinResultContext<ViewMenuEventReceivedXml> context) => OnViewMenuEventReceived(context);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public virtual Task<bool> EnterEventReceived(WeixinResultContext<EnterEventReceivedXml> context) => OnEnterEventReceived(context);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public virtual Task<bool> UnsubscribeEventReceived(WeixinResultContext<UnsubscribeEventReceivedXml> context) => OnUnsubscribeEventReceived(context);
        public virtual Task<bool> SubscribeEventReceived(WeixinResultContext<SubscribeEventReceivedXml> context) => OnSubscribeEventReceived(context);
        public virtual Task<bool> QrscanEventReceived(WeixinResultContext<QrscanEventReceivedXml> context) => OnQrscanEventReceived(context);
        public virtual Task<bool> TextMessageReceived(WeixinResultContext<TextMessageReceivedXml> context) => OnTextMessageReceived(context);
        public virtual Task<bool> LinkMessageReceived(WeixinResultContext<LinkMessageReceivedXml> context) => OnLinkMessageReceived(context);
        public virtual Task<bool> VideoMessageReceived(WeixinResultContext<VideoMessageReceivedXml> context) => OnVideoMessageReceived(context);
        public virtual Task<bool> ShortVideoMessageReceived(WeixinResultContext<ShortVideoMessageReceivedXml> context) => OnShortVideoMessageReceived(context);
        public virtual Task<bool> VoiceMessageReceived(WeixinResultContext<VoiceMessageReceivedXml> context) => OnVoiceMessageReceived(context);
        public virtual Task<bool> LocationMessageReceived(WeixinResultContext<LocationMessageReceivedXml> context) => OnLocationMessageReceived(context);
        public virtual Task<bool> LocationEventReceived(WeixinResultContext<LocationEventReceivedXml> context) => OnLocationEventReceived(context);
    }
}
