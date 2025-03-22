namespace Myvas.AspNetCore.Weixin;

public interface IWeixinReceivedEventEntity : IEntity,
    IWeixinReceivedEventSubscribe,
    IWeixinReceivedEventSubscribeWithScene,
    IWeixinReceivedEventLocation,
    IWeixinReceivedEventQrscan,
    IWeixinReceivedEventClickMenu,
    IWeixinReceivedEventViewMenu
{
    string ConcurrencyStamp { get; set; }
}
