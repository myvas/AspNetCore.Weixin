using Microsoft.AspNetCore.Http;
using System;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinResultContext<TReceivedXml>
        where TReceivedXml : ReceivedXml
    {
        public WeixinResultContext(
            HttpContext context,
            string text,
            TReceivedXml xml)
        {
            Context = context?? throw new ArgumentNullException(nameof(context));
            Text = text;
            Xml = xml;
        }

        public HttpContext Context { get; }
        public string Text { get; }
        public TReceivedXml Xml { get; }
    }

    public class WeixinFailedContext<TReceivedXml> : WeixinResultContext<TReceivedXml>
        where TReceivedXml : ReceivedXml
    {
        public WeixinFailedContext(
            HttpContext context,
            string text,
            TReceivedXml xml) 
            : base(context, text,xml)
        {
        }

        /// <summary>
        /// Gets or sets the exception associated with the authentication failure.
        /// </summary>
        public Exception Exception { get; set; } = default!;
    }
}
