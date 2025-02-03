namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 事件之取消订阅
    /// </summary>
    public class WeixinRequestEventUnsubscribe : EventWeixinRequest, IWeixinRequestEvent
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override RequestEventType Event
		{
			get { return RequestEventType.unsubscribe; }
		}
	}


}
