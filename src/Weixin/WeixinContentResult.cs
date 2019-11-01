using Microsoft.AspNetCore.Mvc;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinContentResult : ContentResult
    {
        private IMessageHandlerDocument MessageHandlerDocument;
        public WeixinContentResult(IMessageHandlerDocument messageHandlerDocument)
        {
            MessageHandlerDocument = messageHandlerDocument;
        }

        public WeixinContentResult(string content)
        {
            base.Content = content;
        }
    }
}
