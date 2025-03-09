using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinJsapiTicketException : Exception
{
	public int ErrorCode { get; set; }

	public WeixinJsapiTicketException(int errorCode, string message) : base(message)
	{
		ErrorCode = errorCode;
	}

	public WeixinJsapiTicketException(int errorCode, string message, Exception inner) : base(message, inner)
	{
		ErrorCode = errorCode;
	}
}