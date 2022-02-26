using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public abstract class WeixinEventSinkBase : IWeixinEventSink
    {
        public virtual async Task<bool> OnTextMessageReceived(WeixinResultContext<TextMessageReceivedXml> context)
        {
            //_logger.LogTrace($"收到一条微信文本消息。");
            //_logger.LogTrace(XmlConvert.SerializeObject(e));

            var result = new StringBuilder();
            result.AppendFormat("您刚才发送了文本信息：{0}", context.Xml.Content);

            await WeixinResponseBuilder.FlushTextMessage(context.Context, context.Xml, result.ToString());

            //_logger.LogDebug(XmlConvert.SerializeObject(responseMessage));

            return false;
        }

        public virtual Task<bool> OnLinkMessageReceived(WeixinResultContext<LinkMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnVideoMessageReceived(WeixinResultContext<VideoMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnShortVideoMessageReceived(WeixinResultContext<ShortVideoMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnVoiceMessageReceived(WeixinResultContext<VoiceMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnImageMessageReceived(WeixinResultContext<ImageMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnLocationMessageReceived(WeixinResultContext<LocationMessageReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnLocationEventReceived(WeixinResultContext<LocationEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnClickMenuEventReceived(WeixinResultContext<ClickMenuEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnViewMenuEventReceived(WeixinResultContext<ViewMenuEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnUnsubscribeEventReceived(WeixinResultContext<UnsubscribeEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnEnterEventReceived(WeixinResultContext<EnterEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnSubscribeEventReceived(WeixinResultContext<SubscribeEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnQrscanEventReceived(WeixinResultContext<QrscanEventReceivedXml> context)
        {
            return Task.FromResult(false);
        }
    }
}
