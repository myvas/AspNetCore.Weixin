using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinEventArgs<T> : EventArgs
    where T : ReceivedXml
{
    /// <summary>
    /// The context of Weixin request and response.
    /// </summary>
    public WeixinContext Context { get; }

    /// <summary>
    /// The data stored by the sender for this event.
    /// </summary>
    public T Xml { get; }
    
    public WeixinEventArgs(WeixinContext context, T data)
    {
        (Context, Xml) = (context, data);
    }
}
