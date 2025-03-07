namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 事件之URL跳转视图（View）
    /// </summary>
    public class WeixinRequestEventView : EventWeixinRequest, IWeixinRequestEvent
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override RequestEventType Event
		{
			get { return RequestEventType.VIEW; }
		}
	}


}
