using System;

namespace AspNetCore.Weixin
{
	internal static class WeixinJsapiTicketError
	{
		public static WeixinJsapiTicketException GenericError(Exception inner = null)
		{
			var message = "获取access_token接口调用失败";
			return new WeixinJsapiTicketException(50000, message, inner);
		}

		public static WeixinJsapiTicketException UnknownResponse(Exception inner = null)
		{
			var message = "获取access_token接口没有正确返回";
			return new WeixinJsapiTicketException(50001, message, inner);
		}
	}
}