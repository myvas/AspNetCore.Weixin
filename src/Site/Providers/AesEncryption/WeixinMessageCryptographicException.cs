using System;
using System.Security.Cryptography;

namespace Myvas.AspNetCore.Weixin;

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
