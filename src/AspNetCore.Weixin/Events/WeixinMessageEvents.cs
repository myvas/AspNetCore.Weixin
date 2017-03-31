using AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Weixin
{
    /// <summary>
    /// "Official account services unavailable. Try again later." 
    /// will be response while firing an event that returns "false".
    /// </summary>
    public class WeixinMessageEvents
    {
        public Func<WeixinReceivedContext<ImageMessageReceivedEventArgs>, Task<bool>> OnImageMessageReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<ClickMenuEventReceivedEventArgs>, Task<bool>> OnClickMenuEventReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<ViewMenuEventReceivedEventArgs>, Task<bool>> OnViewMenuEventReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<UnsubscribeEventReceivedEventArgs>, Task<bool>> OnUnsubscribeEventReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<EnterEventReceivedEventArgs>, Task<bool>> OnEnterEventReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<SubscribeEventReceivedEventArgs>, Task<bool>> OnSubscribeEventReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<QrscanEventReceivedEventArgs>, Task<bool>> OnQrscanEventReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<TextMessageReceivedEventArgs>, Task<bool>> OnTextMessageReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<LinkMessageReceivedEventArgs>, Task<bool>> OnLinkMessageReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<VideoMessageReceivedEventArgs>, Task<bool>> OnVideoMessageReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<ShortVideoMessageReceivedEventArgs>, Task<bool>> OnShortVideoMessageReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<VoiceMessageReceivedEventArgs>, Task<bool>> OnVoiceMessageReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<LocationMessageReceivedEventArgs>, Task<bool>> OnLocationMessageReceived { get; set; } = e => TaskCache.CompletedTaskFalse;
        public Func<WeixinReceivedContext<LocationEventReceivedEventArgs>,  Task<bool>> OnLocationEventReceived { get; set; } = e => TaskCache.CompletedTaskFalse;

        public virtual Task ImageMessageReceived(WeixinReceivedContext<ImageMessageReceivedEventArgs> context) => OnImageMessageReceived(context);
        public virtual Task ClickMenuEventReceived(WeixinReceivedContext<ClickMenuEventReceivedEventArgs> context) => OnClickMenuEventReceived(context);
        public virtual Task ViewMenuEventReceived(WeixinReceivedContext<ViewMenuEventReceivedEventArgs> context) => OnViewMenuEventReceived(context);
        public virtual Task UnsubscribeEventReceived(WeixinReceivedContext<UnsubscribeEventReceivedEventArgs> context) => OnUnsubscribeEventReceived(context);
        public virtual Task EnterEventReceived(WeixinReceivedContext<EnterEventReceivedEventArgs> context) => OnEnterEventReceived(context);
        public virtual Task SubscribeEventReceived(WeixinReceivedContext<SubscribeEventReceivedEventArgs> context) => OnSubscribeEventReceived(context);
        public virtual Task QrscanEventReceived(WeixinReceivedContext<QrscanEventReceivedEventArgs> context) => OnQrscanEventReceived(context);
        public virtual Task TextMessageReceived(WeixinReceivedContext<TextMessageReceivedEventArgs> context) => OnTextMessageReceived(context);
        public virtual Task LinkMessageReceived(WeixinReceivedContext<LinkMessageReceivedEventArgs> context) => OnLinkMessageReceived(context);
        public virtual Task VideoMessageReceived(WeixinReceivedContext<VideoMessageReceivedEventArgs> context) => OnVideoMessageReceived(context);
        public virtual Task ShortVideoMessageReceived(WeixinReceivedContext<ShortVideoMessageReceivedEventArgs> context) => OnShortVideoMessageReceived(context);
        public virtual Task VoiceMessageReceived(WeixinReceivedContext<VoiceMessageReceivedEventArgs> context) => OnVoiceMessageReceived(context);
        public virtual Task LocationMessageReceived(WeixinReceivedContext<LocationMessageReceivedEventArgs> context) => OnLocationMessageReceived(context);
        public virtual Task LocationEventReceived(WeixinReceivedContext<LocationEventReceivedEventArgs> context) => OnLocationEventReceived(context);
    }
}
