namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinReceivedEvent
    {
        string Event { get; set; }
    }

    public interface IWeixinReceivedEventSubscribe : IWeixinReceivedEvent
    {
    }
    public interface IWeixinReceivedEventSubscribeWithScene : IWeixinReceivedEvent
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
        string Longitude { get; set; }
        string Latitude { get; set; }
        string Precision { get; set; }
    }
    public interface IWeixinReceivedEventViewMenu : IWeixinReceivedEvent
    {
        string EventKey { get; set; }
    }
    public interface IWeixinReceivedEventClickMenu : IWeixinReceivedEvent
    {
        string EventKey { get; set; }
    }
}