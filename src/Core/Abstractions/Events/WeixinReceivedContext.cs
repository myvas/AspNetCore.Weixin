using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinReceivedContext<TReceivedXml> : WeixinContext
        where TReceivedXml : ReceivedXml
    {
        public WeixinReceivedContext(HttpContext requestContext, string text, ReceivedXml xml) : base(requestContext, text)
        {
            Xml = xml as TReceivedXml;
        }

        public TReceivedXml Xml { get; }
    }
}
