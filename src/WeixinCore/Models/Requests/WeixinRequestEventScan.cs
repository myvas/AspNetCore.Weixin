namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 事件之二维码扫描（关注微信）
    /// </summary>
    public class WeixinRequestEventScan : EventWeixinRequest, IWeixinRequestEvent
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override RequestEventType Event
		{
			get { return RequestEventType.SCAN; }
		}

		public string Ticket { get; set; }
	}


}
