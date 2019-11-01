using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.Weixin
{
	/// <summary>
	/// 事件之URL跳转视图（View）
	/// </summary>
	public class RequestMessageEventView : RequestMessageEventBase, IRequestMessageEventBase
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override ReceivedEventType Event
		{
			get { return ReceivedEventType.VIEW; }
		}
	}

	/// <summary>
	/// 事件之取消订阅
	/// </summary>
	public class RequestMessageEventClick : RequestMessageEventBase, IRequestMessageEventBase
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override ReceivedEventType Event
		{
			get { return ReceivedEventType.CLICK; }
		}
	}

	public class RequestMessageEventEnter : RequestMessageEventBase, IRequestMessageEventBase
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override ReceivedEventType Event
		{
			get { return ReceivedEventType.VIEW; }
		}
	}

	/// <summary>
	/// 事件之订阅
	/// </summary>
	public class RequestMessageEventSubscribe : RequestMessageEventBase, IRequestMessageEventBase
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override ReceivedEventType Event
		{
			get { return ReceivedEventType.subscribe; }
		}

		/// <summary>
		/// 带场景码时，作为换二维码图的票据
		/// </summary>
		public string Ticket { get; set; }
	}

	/// <summary>
	/// 事件之取消订阅
	/// </summary>
	public class RequestMessageEventUnsubscribe : RequestMessageEventBase, IRequestMessageEventBase
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override ReceivedEventType Event
		{
			get { return ReceivedEventType.unsubscribe; }
		}
	}


	/// <summary>
	/// 事件之二维码扫描（关注微信）
	/// </summary>
	public class RequestMessageEventScan : RequestMessageEventBase, IRequestMessageEventBase
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override ReceivedEventType Event
		{
			get { return ReceivedEventType.SCAN; }
		}

		public string Ticket { get; set; }
	}


	/// <summary>
	/// 获取用户地理位置（高级接口下才能用）
	/// 获取用户地理位置的方式有两种，一种是仅在进入会话时上报一次，一种是进入会话后每隔5秒上报一次。公众号可以在公众平台网站中设置。
	/// 用户同意上报地理位置后，每次进入公众号会话时，都会在进入时上报地理位置，或在进入会话后每5秒上报一次地理位置，上报地理位置以推送XML数据包到开发者填写的URL来实现。
	/// </summary>
	public class RequestMessageEventLocation : RequestMessageEventBase, IRequestMessageEventBase
	{
		/// <summary>
		/// 事件类型
		/// </summary>
		public override ReceivedEventType Event
		{
			get { return ReceivedEventType.LOCATION; }
		}

		/// <summary>
		/// 地理位置维度，事件类型为LOCATION的时存在
		/// </summary>
		public decimal Latitude { get; set; }
		/// <summary>
		/// 地理位置经度，事件类型为LOCATION的时存在
		/// </summary>
		public decimal Longitude { get; set; }
		/// <summary>
		/// 地理位置精度，事件类型为LOCATION的时存在
		/// </summary>
		public decimal Precision { get; set; }
	}


}
