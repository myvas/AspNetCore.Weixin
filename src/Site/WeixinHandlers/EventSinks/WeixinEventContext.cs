using Microsoft.AspNetCore.Http;
using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinEventContext<TReceivedXml>
    where TReceivedXml : ReceivedXml, new()
{
    public WeixinEventContext(
        HttpContext context,
        string text,
        TReceivedXml xml)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Text = text;
        Xml = xml;
    }

    public HttpContext Context { get; }
    public string Text { get; }
    public TReceivedXml Xml { get; }
}
