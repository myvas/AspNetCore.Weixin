using System;
using System.Collections.Generic;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{

	public class WeixinAccessTokenException : Exception
	{
		public int ErrorCode { get; set; }

		public WeixinAccessTokenException(int errorCode, string message) : base(message)
		{
			ErrorCode = errorCode;
		}

		public WeixinAccessTokenException(int errorCode, string message, Exception inner) : base(message, inner)
		{
			ErrorCode = errorCode;
		}
	}
}
