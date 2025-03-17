using System;

namespace Myvas.AspNetCore.Weixin;

public static class EventReceivedXmlExtensions
{
    public static RequestEventType EventAsEnum(this EventReceivedXml o)
    {
        return (RequestEventType)Enum.Parse(typeof(RequestEventType), o.Event, true);
    }

    public static EventReceivedXml EventFromEnum(this EventReceivedXml o, RequestEventType value)
    {
        o.Event = value.ToString();
        return o;
    }
}
