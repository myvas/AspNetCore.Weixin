namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 事件之取消订阅
    /// </summary>
    public class WeixinRequestEventClick : EventWeixinRequest, IWeixinRequestEvent
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override RequestEventType Event
		{
			get { return RequestEventType.CLICK; }
		}
	}


}
