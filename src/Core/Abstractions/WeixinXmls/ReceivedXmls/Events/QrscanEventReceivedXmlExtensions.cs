namespace Myvas.AspNetCore.Weixin;

public static class QrscanEventReceivedXmlExtensions
{
    /// <summary>
    /// 场景ID
    /// </summary>
    public static string EventKeyAsScene(this QrscanEventReceivedXml o)
    {
        return o.EventKey;
    }

    /// <summary>
    /// 场景ID
    /// </summary>
    public static QrscanEventReceivedXml EventKeyFromScene(this QrscanEventReceivedXml o, string value)
    {
        o.EventKey = value;
        return o;
    }
}
