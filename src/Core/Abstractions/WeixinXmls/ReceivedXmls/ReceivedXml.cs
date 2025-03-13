using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 收到的微信消息或事件
/// </summary>
[XmlRoot("xml", Namespace = "")]
public class ReceivedXml : WeixinXml
{
	/// <summary>
	/// 开发者微信号
	/// </summary>
	[XmlElement("ToUserName")]
	public string ToUserName { get; set; }

	/// <summary>
	/// 发送方帐号（一个OpenID）
	/// </summary>
	[XmlElement("FromUserName")]
	public string FromUserName { get; set; }

	/// <summary>
	/// 消息创建时间
	/// </summary>
	[XmlElement("CreateTime")]
	public long CreateTime { get; set; }

	/// <summary>
	/// 消息类型
	/// </summary>
	[XmlElement("MsgType")]
	public string MsgType { get; set; }
}
