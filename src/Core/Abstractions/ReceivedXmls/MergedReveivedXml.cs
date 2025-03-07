using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
	/// <summary>
	/// 将收到消息或事件合并（放弃之，难以做到！）
	/// </summary>
	[XmlRoot("xml", Namespace = "")]
	public class MergedReceivedXml
	{
		#region ReceivedXml
		/// <summary>
		/// 开发者微信号
		/// </summary>
		public string ToUserName { get; set; }

		/// <summary>
		/// 发送方帐号（一个OpenID）
		/// </summary>
		public string FromUserName { get; set; }

		/// <summary>
		/// 消息创建时间
		/// </summary>
		[XmlElement("CreateTime")]
		public long CreateTimeStr { get; set; }
		[XmlIgnore]
		public DateTime CreateTime
		{
			get
			{
				return WeixinTimestampHelper.ToLocalTime(CreateTimeStr);
			}
			set
			{
				CreateTimeStr = WeixinTimestampHelper.FromLocalTime(value);
			}
		}

		/// <summary>
		/// 消息类型
		/// </summary>
		[XmlElement("MsgType")]
		public string MsgTypeStr { get; set; }
		[XmlIgnore]
		public RequestMsgType MsgType
		{
			get
			{
				return (RequestMsgType)Enum.Parse(typeof(RequestMsgType), MsgTypeStr, true);
			}
			set
			{
				MsgTypeStr = MsgType.ToString();
			}
		}
        #endregion

        #region Event
        /// <summary>
        /// 事件类型
        /// </summary>
        [XmlElement("Event", Namespace = "")]
		public string EventValue { get; set; }
		[XmlIgnore]
		public RequestEventType Event
		{
			get
			{
				return (RequestEventType)Enum.Parse(typeof(RequestEventType), EventValue, true);
			}
			set
			{
				EventValue = value.ToString();
			}
		}
		#region Subscribe, ClientMenu, Qrscan, ViewMenu
		/// <summary>
		/// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
		/// </summary>
		public string EventKey { get; set; }

		/// <summary>
		/// 二维码的ticket，可用来换取二维码图片
		/// </summary>
		public string Ticket { get; set; }
		#endregion
		#endregion
		#region Message
		public Int64 MsgId { get; set; }
		#region Image
		public string PicUrl { get; set; }
		public string MediaId { get; set; }
		#endregion
		#region Link
		public string Title { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
		#endregion
		#endregion
	}
}
