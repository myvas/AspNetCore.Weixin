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
    public class WeixinEvents
    {
        public Func<WeixinEventContext<ImageMessageReceivedXml>, Task<bool>> OnImageMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<ClickMenuEventReceivedXml>, Task<bool>> OnClickMenuEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<ViewMenuEventReceivedXml>, Task<bool>> OnViewMenuEventReceived { get; set; } = e => Task.FromResult(false);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public Func<WeixinEventContext<EnterEventReceivedXml>, Task<bool>> OnEnterEventReceived { get; set; } = e => Task.FromResult(false);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public Func<WeixinEventContext<UnsubscribeEventReceivedXml>, Task<bool>> OnUnsubscribeEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<SubscribeEventReceivedXml>, Task<bool>> OnSubscribeEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<QrscanEventReceivedXml>, Task<bool>> OnQrscanEventReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<TextMessageReceivedXml>, Task<bool>> OnTextMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<LinkMessageReceivedXml>, Task<bool>> OnLinkMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<VideoMessageReceivedXml>, Task<bool>> OnVideoMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<ShortVideoMessageReceivedXml>, Task<bool>> OnShortVideoMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<VoiceMessageReceivedXml>, Task<bool>> OnVoiceMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<LocationMessageReceivedXml>, Task<bool>> OnLocationMessageReceived { get; set; } = e => Task.FromResult(false);
        public Func<WeixinEventContext<LocationEventReceivedXml>, Task<bool>> OnLocationEventReceived { get; set; } = e => Task.FromResult(false);

        public virtual Task<bool> ImageMessageReceived(WeixinEventContext<ImageMessageReceivedXml> context) => OnImageMessageReceived(context);
        public virtual Task<bool> ClickMenuEventReceived(WeixinEventContext<ClickMenuEventReceivedXml> context) => OnClickMenuEventReceived(context);
        public virtual Task<bool> ViewMenuEventReceived(WeixinEventContext<ViewMenuEventReceivedXml> context) => OnViewMenuEventReceived(context);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public virtual Task<bool> EnterEventReceived(WeixinEventContext<EnterEventReceivedXml> context) => OnEnterEventReceived(context);
        [Obsolete("This event will be never fired, because it was removed by Tencent.")]
        public virtual Task<bool> UnsubscribeEventReceived(WeixinEventContext<UnsubscribeEventReceivedXml> context) => OnUnsubscribeEventReceived(context);
        public virtual Task<bool> SubscribeEventReceived(WeixinEventContext<SubscribeEventReceivedXml> context) => OnSubscribeEventReceived(context);
        public virtual Task<bool> QrscanEventReceived(WeixinEventContext<QrscanEventReceivedXml> context) => OnQrscanEventReceived(context);
        public virtual Task<bool> TextMessageReceived(WeixinEventContext<TextMessageReceivedXml> context) => OnTextMessageReceived(context);
        public virtual Task<bool> LinkMessageReceived(WeixinEventContext<LinkMessageReceivedXml> context) => OnLinkMessageReceived(context);
        public virtual Task<bool> VideoMessageReceived(WeixinEventContext<VideoMessageReceivedXml> context) => OnVideoMessageReceived(context);
        public virtual Task<bool> ShortVideoMessageReceived(WeixinEventContext<ShortVideoMessageReceivedXml> context) => OnShortVideoMessageReceived(context);
        public virtual Task<bool> VoiceMessageReceived(WeixinEventContext<VoiceMessageReceivedXml> context) => OnVoiceMessageReceived(context);
        public virtual Task<bool> LocationMessageReceived(WeixinEventContext<LocationMessageReceivedXml> context) => OnLocationMessageReceived(context);
        public virtual Task<bool> LocationEventReceived(WeixinEventContext<LocationEventReceivedXml> context) => OnLocationEventReceived(context);
    }
}
