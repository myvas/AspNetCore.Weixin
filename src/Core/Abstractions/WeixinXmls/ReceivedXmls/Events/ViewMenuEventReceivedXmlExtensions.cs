namespace Myvas.AspNetCore.Weixin;

public static class ViewMenuEventReceivedXmlExtensions
{
    public static string EventKeyAsUrl(this ViewMenuEventReceivedXml o)
    {
        return o.EventKey;
    }

    public static ViewMenuEventReceivedXml EventKeyFromUrl(this ViewMenuEventReceivedXml o, string value)
    {
        o.EventKey = value;
        return o;
    }
}
