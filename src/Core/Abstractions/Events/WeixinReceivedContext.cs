using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinReceivedContext<TReceivedXml>
        where TReceivedXml : ReceivedXml
    {
        public WeixinReceivedContext(HttpContext requestContext, string text, ReceivedXml xml)
        {
            Context = requestContext;
            Text = text;
            Xml = xml as TReceivedXml;
        }

        public HttpContext Context { get; }
        public string Text { get; }
        public TReceivedXml Xml { get; }
    }
}
