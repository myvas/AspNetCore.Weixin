using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin
{
	/// <summary>
	/// 收到消息或事件
	/// </summary>
	[XmlRoot("xml", Namespace = "")]
	public class ReceivedEventArgs : EventArgs
	{
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
		public ReceivedMsgType MsgType
		{
			get
			{
				return (ReceivedMsgType)Enum.Parse(typeof(ReceivedMsgType), MsgTypeStr, true);
			}
			set
			{
				MsgTypeStr = MsgType.ToString();
			}
		}
	}
}
