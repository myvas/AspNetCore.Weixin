namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 上报地理位置事件
/// </summary>
public class LocationEventReceivedEventArgs : WeixinEventArgs<LocationEventReceivedXml>
{
    public LocationEventReceivedEventArgs(WeixinContext context, LocationEventReceivedXml data) : base(context, data)
    {
    }
}
