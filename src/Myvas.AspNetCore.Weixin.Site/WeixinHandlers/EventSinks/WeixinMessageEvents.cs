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
        public Func<WeixinReceivedContext<ImageMessageReceivedXml>, Task<bool>> OnImageMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<ClickMenuEventReceivedXml>, Task<bool>> OnClickMenuEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<ViewMenuEventReceivedXml>, Task<bool>> OnViewMenuEventReceived { get; set; } = e => Task.FromResult(false);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public Func<WeixinReceivedContext<EnterEventReceivedXml>, Task<bool>> OnEnterEventReceived { get; set; } = e => Task.FromResult(false);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public Func<WeixinReceivedContext<UnsubscribeEventReceivedXml>, Task<bool>> OnUnsubscribeEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<SubscribeEventReceivedXml>, Task<bool>> OnSubscribeEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<QrscanEventReceivedXml>, Task<bool>> OnQrscanEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<TextMessageReceivedXml>, Task<bool>> OnTextMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<LinkMessageReceivedXml>, Task<bool>> OnLinkMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<VideoMessageReceivedXml>, Task<bool>> OnVideoMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<ShortVideoMessageReceivedXml>, Task<bool>> OnShortVideoMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<VoiceMessageReceivedXml>, Task<bool>> OnVoiceMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<LocationMessageReceivedXml>, Task<bool>> OnLocationMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinReceivedContext<LocationEventReceivedXml>, Task<bool>> OnLocationEventReceived { get; set; } = e => Task.FromResult(false);

        public virtual Task<bool> ImageMessageReceived(WeixinReceivedContext<ImageMessageReceivedXml> context) => OnImageMessageReceived(context);
        public virtual Task<bool> ClickMenuEventReceived(WeixinReceivedContext<ClickMenuEventReceivedXml> context) => OnClickMenuEventReceived(context);
        public virtual Task<bool> ViewMenuEventReceived(WeixinReceivedContext<ViewMenuEventReceivedXml> context) => OnViewMenuEventReceived(context);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public virtual Task<bool> EnterEventReceived(WeixinReceivedContext<EnterEventReceivedXml> context) => OnEnterEventReceived(context);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public virtual Task<bool> UnsubscribeEventReceived(WeixinReceivedContext<UnsubscribeEventReceivedXml> context) => OnUnsubscribeEventReceived(context);
        public virtual Task<bool> SubscribeEventReceived(WeixinReceivedContext<SubscribeEventReceivedXml> context) => OnSubscribeEventReceived(context);
        public virtual Task<bool> QrscanEventReceived(WeixinReceivedContext<QrscanEventReceivedXml> context) => OnQrscanEventReceived(context);
        public virtual Task<bool> TextMessageReceived(WeixinReceivedContext<TextMessageReceivedXml> context) => OnTextMessageReceived(context);
        public virtual Task<bool> LinkMessageReceived(WeixinReceivedContext<LinkMessageReceivedXml> context) => OnLinkMessageReceived(context);
        public virtual Task<bool> VideoMessageReceived(WeixinReceivedContext<VideoMessageReceivedXml> context) => OnVideoMessageReceived(context);
        public virtual Task<bool> ShortVideoMessageReceived(WeixinReceivedContext<ShortVideoMessageReceivedXml> context) => OnShortVideoMessageReceived(context);
        public virtual Task<bool> VoiceMessageReceived(WeixinReceivedContext<VoiceMessageReceivedXml> context) => OnVoiceMessageReceived(context);
        public virtual Task<bool> LocationMessageReceived(WeixinReceivedContext<LocationMessageReceivedXml> context) => OnLocationMessageReceived(context);
        public virtual Task<bool> LocationEventReceived(WeixinReceivedContext<LocationEventReceivedXml> context) => OnLocationEventReceived(context);
    }
}
