using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCore.Weixin
{
	internal static class WeixinAccessTokenError
	{
		public static WeixinAccessTokenException GenericError(Exception inner = null)
		{
			var message = "获取access_token接口调用失败";
			return new WeixinAccessTokenException(50000, message, inner);
		}

		public static WeixinAccessTokenException UnknownResponse(Exception inner = null)
		{
			var message = "获取access_token接口没有正确返回";
			return new WeixinAccessTokenException(50001, message, inner);
		}

		public static WeixinAccessTokenException Busy(Exception inner = null)
		{
			var message = "系统繁忙，请稍后再试";
			return new WeixinAccessTokenException(-1, message, inner);
		}

		public static WeixinAccessTokenException AppSecretError(Exception inner = null)
		{
			var message = "AppSecret错误或者AppSecret不属于这个公众号";
			return new WeixinAccessTokenException(40001, message, inner);
		}
		
		public static WeixinAccessTokenException IllegalGrantType(Exception inner = null)
		{
			var message = "grant_type字段值非法（请确保为client_credential）";
			return new WeixinAccessTokenException(40002, message, inner);
		}

		public static WeixinAccessTokenException InvalidAppId(Exception inner = null)
		{
			var message = "AppID有误";
			return new WeixinAccessTokenException(40013, message, inner);
		}

		public static WeixinAccessTokenException ValidateAppidFailed(Exception inner = null)
		{
			var message = "调用接口的IP地址不在白名单中";
			return new WeixinAccessTokenException(40164, message, inner);
		}
	}
}
