using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin
{
    public abstract class WeixinEventSinkBase : IWeixinEventSink
	{
		public virtual Task<bool> OnTextMessageReceived(object sender, TextMessageReceivedEventArgs e)
		{
			//_logger.LogTrace($"收到一条微信文本消息。");
			//_logger.LogTrace(XmlConvert.SerializeObject(e));

			//var messageHandler = sender as WeixinMessageHandler;
			//var responseMessage = new ResponseMessageText();
			//{
			//	var result = new StringBuilder();
			//	result.AppendFormat("您刚才发送了文本信息：{0}", e.Content);

			//	responseMessage.FromUserName = e.ToUserName;
			//	responseMessage.ToUserName = e.FromUserName;
			//	responseMessage.Content = result.ToString();
			//}
			//await messageHandler.WriteAsync(responseMessage);

			//_logger.LogDebug(XmlConvert.SerializeObject(responseMessage));

			return Task.FromResult(false);
		}

		public virtual Task<bool> OnLinkMessageReceived(object sender, LinkMessageReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnVideoMessageReceived(object sender, VideoMessageReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnShortVideoMessageReceived(object sender, ShortVideoMessageReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnVoiceMessageReceived(object sender, VoiceMessageReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnImageMessageReceived(object sender, ImageMessageReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnLocationMessageReceived(object sender, LocationMessageReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnLocationEventReceived(object sender, LocationEventReceivedEventArgs e)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnClickMenuEventReceived(object sender, ClickMenuEventReceivedEventArgs e)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnViewMenuEventReceived(object sender, ViewMenuEventReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnUnsubscribeEventReceived(object sender, UnsubscribeEventReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnEnterEventReceived(object sender, EnterEventReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnSubscribeEventReceived(object sender, SubscribeEventReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }

        public virtual Task<bool> OnQrscanEventReceived(object sender, QrscanEventReceivedEventArgs e)
		{
            return Task.FromResult(false);
        }
    }
}
