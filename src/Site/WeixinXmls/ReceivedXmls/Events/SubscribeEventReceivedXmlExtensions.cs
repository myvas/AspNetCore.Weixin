namespace Myvas.AspNetCore.Weixin;

public static class SubscribeEventReceivedXmlExtensions
{
    public const string EventKeyPrefix = "qrscene_";

    public static string EventKeyAsScene(this SubscribeEventReceivedXml o)
    {
        if (o.EventKey?.StartsWith(EventKeyPrefix) ?? false)
        {
            return o.EventKey?.Substring(EventKeyPrefix.Length) ?? "";
        }
        else
        {
            return "";
        }
    }

    public static SubscribeEventReceivedXml EventKeyFromScene(this SubscribeEventReceivedXml o, string value)
    {
        if (value?.StartsWith(EventKeyPrefix)??false)
        {
            o.EventKey = value;
        }
        else
        {
            o.EventKey = EventKeyPrefix + value;
        }

        return o;
    }
}
