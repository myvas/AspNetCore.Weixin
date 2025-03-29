namespace Myvas.AspNetCore.Weixin;

public interface IWeixinReceivedEvent : IWeixinReceived
{
    string Event { get; set; }
}

public interface IWeixinReceivedEventSubscribe : IWeixinReceivedEvent
{
}
public interface IWeixinReceivedEventSubscribeWithScene
{
    string EventKey { get; set; }
    string Ticket { get; set; }
}
public interface IWeixinReceivedEventQrscan : IWeixinReceivedEvent
{
    string EventKey { get; set; }
    string Ticket { get; set; }
}
public interface IWeixinReceivedEventLocation : IWeixinReceivedEvent
{
    decimal? Longitude { get; set; }
    decimal? Latitude { get; set; }
    decimal? Precision { get; set; }
}
public interface IWeixinReceivedEventViewMenu : IWeixinReceivedEvent
{
    string EventKey { get; set; }
}
public interface IWeixinReceivedEventClickMenu : IWeixinReceivedEvent
{
    string EventKey { get; set; }
}