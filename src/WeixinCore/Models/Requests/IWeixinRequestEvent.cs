namespace Myvas.AspNetCore.Weixin
{
    public interface IWeixinRequestEvent : IWeixinRequest
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        RequestEventType Event { get; }
        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        string EventKey { get; set; }
    }
}
