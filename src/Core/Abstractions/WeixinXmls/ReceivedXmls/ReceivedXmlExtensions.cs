using System;

namespace Myvas.AspNetCore.Weixin;

public static class ReceivedXmlExtensions
{
	public static DateTime CreateTimeAsDateTime(this ReceivedXml receivedXml)
	{
		return WeixinTimestampHelper.ToLocalTime(receivedXml.CreateTime);
	}

	public static ReceivedXml CreateTimeFromDateTime(this ReceivedXml receivedXml, DateTime createTime)
	{
		receivedXml.CreateTime = WeixinTimestampHelper.FromLocalTime(createTime);
		return receivedXml;
	}

	public static RequestMsgType MsgTypeAsEnum(this ReceivedXml receivedXml)
	{
		return (RequestMsgType)Enum.Parse(typeof(RequestMsgType), receivedXml.MsgType, true);
	}

	public static ReceivedXml MsgTypeFromEnum(this ReceivedXml receivedXml, RequestMsgType value)
	{
		receivedXml.MsgType = value.ToString();
		return receivedXml;
	}
}
