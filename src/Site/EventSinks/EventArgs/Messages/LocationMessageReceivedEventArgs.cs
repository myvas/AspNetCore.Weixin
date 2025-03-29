namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到地理位置消息
/// </summary>
public class LocationMessageReceivedEventArgs : WeixinEventArgs<LocationMessageReceivedXml>
{
    public LocationMessageReceivedEventArgs(WeixinContext context, LocationMessageReceivedXml data) : base(context, data)
    {
    }
}
