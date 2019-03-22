using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCore.Weixin.DataProtection
{
	public class WeixinMessageCryptographicException : CryptographicException
	{
		public int ErrorCode { get; set; }

		public WeixinMessageCryptographicException(int errorCode, string message) : base(message)
		{
			ErrorCode = errorCode;
		}

		public WeixinMessageCryptographicException(int errorCode, string message, Exception inner) : base(message, inner)
		{
			ErrorCode = errorCode;
		}
	}
}
