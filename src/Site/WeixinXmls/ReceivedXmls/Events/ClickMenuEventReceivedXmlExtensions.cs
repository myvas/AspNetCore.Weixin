namespace Myvas.AspNetCore.Weixin;

public static class ClickMenuEventReceivedXmlExtensions
{
    /// <summary>
    /// 事件KEY值，与自定义菜单接口中KEY值对应。这是<see cref="EventKey"/>的别名。
    /// </summary>
    public static ClickMenuEventReceivedXml EventKeyFromMenuItemKey(this ClickMenuEventReceivedXml o, string value)
    {
        o.EventKey = value;
        return o;
    }

    /// <summary>
    /// 事件KEY值，与自定义菜单接口中KEY值对应。这是<see cref="EventKey"/>的别名。
    /// </summary>
    public static string EventKeyAsMenuItemKey(this ClickMenuEventReceivedXml o)
    {
        return o.EventKey;
    }
}
