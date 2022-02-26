using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinEventSink : IWeixinEventSink
    {
        protected readonly ILogger _logger;
        protected readonly IWeixinResponseBuilder WeixinResponseBuilder;

        public WeixinEventSink(IWeixinResponseBuilder responseBuilder, ILogger<IWeixinEventSink> logger)
        {
            WeixinResponseBuilder = responseBuilder ?? throw new ArgumentNullException(nameof(responseBuilder));
            _logger = logger;
        }

        public virtual Task<bool> OnTextMessageReceived(WeixinEventContext<TextMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnLinkMessageReceived(WeixinEventContext<LinkMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnVideoMessageReceived(WeixinEventContext<VideoMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnShortVideoMessageReceived(WeixinEventContext<ShortVideoMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnVoiceMessageReceived(WeixinEventContext<VoiceMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnImageMessageReceived(WeixinEventContext<ImageMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnLocationMessageReceived(WeixinEventContext<LocationMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnLocationEventReceived(WeixinEventContext<LocationEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnClickMenuEventReceived(WeixinEventContext<ClickMenuEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnViewMenuEventReceived(WeixinEventContext<ViewMenuEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnUnsubscribeEventReceived(WeixinEventContext<UnsubscribeEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnEnterEventReceived(WeixinEventContext<EnterEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnSubscribeEventReceived(WeixinEventContext<SubscribeEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnQrscanEventReceived(WeixinEventContext<QrscanEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }
    }
}
