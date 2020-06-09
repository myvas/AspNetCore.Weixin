namespace Myvas.AspNetCore.Weixin
{
    public class WeixinReceivedEvent : Entity,
        IWeixinReceivedEventSubscribe,
        IWeixinReceivedEventSubscribeWithScene,
        IWeixinReceivedEventLocation,
        IWeixinReceivedEventQrscan,
        IWeixinReceivedEventClickMenu,
        IWeixinReceivedEventViewMenu
    {
        public string FromUserName { get; set; }
        public string ToUserName { get; set; }
        public string CreateTime { get; set; }
        public string MsgType { get; set; }
        public string Event { get; set; }
        public string EventKey { get; set; }
        public string Ticket { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Precision { get; set; }
    }
}