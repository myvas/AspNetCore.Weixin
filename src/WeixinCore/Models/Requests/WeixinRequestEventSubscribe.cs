namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 事件之订阅
    /// </summary>
    public class WeixinRequestEventSubscribe : EventWeixinRequest, IWeixinRequestEvent
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override RequestEventType Event
		{
			get { return RequestEventType.subscribe; }
		}

		/// <summary>
		/// 带场景码时，作为换二维码图的票据
		/// </summary>
		public string Ticket { get; set; }
	}


}
