namespace Myvas.AspNetCore.Weixin
{
    public class WeixinRequestEventEnter : EventWeixinRequest, IWeixinRequestEvent
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
