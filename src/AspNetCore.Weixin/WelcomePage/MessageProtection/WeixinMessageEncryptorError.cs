using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AspNetCore.Weixin.DataProtection
{
	internal static class WeixinMessageEncryptorError
	{
		public static WeixinMessageCryptographicException ValidateSignatureFailed(Exception inner = null)
		{
			var message = "签名验证错误";
			return new WeixinMessageCryptographicException(-40001, message, inner);
		}

		public static WeixinMessageCryptographicException ParseXmlFailed(Exception inner = null)
		{
			var message = "XML解析失败";
			return new WeixinMessageCryptographicException(-40002, message, inner);
		}

		public static WeixinMessageCryptographicException CalculateSignatureFailed(Exception inner = null)
		{
			var message = "SHA加密生成签名失败";
			return new WeixinMessageCryptographicException(-40003, message, inner);
		}
		
		public static WeixinMessageCryptographicException IllegalAesKey(Exception inner = null)
		{
			var message = "AES Key非法";
			return new WeixinMessageCryptographicException(-40004, message, inner);
		}

		public static WeixinMessageCryptographicException ValidateAppidFailed(Exception inner = null)
		{
			var message = "AppId校验错误";
			return new WeixinMessageCryptographicException(-40005, message, inner);
		}
		public static WeixinMessageCryptographicException AesEncryptFailed(Exception inner = null)
		{
			var message = "AES加密失败";
			return new WeixinMessageCryptographicException(-40006, message, inner);
		}
		public static WeixinMessageCryptographicException AesDecryptFailed(Exception inner = null)
		{
			var message = "AES解密失败";
			return new WeixinMessageCryptographicException(-40007, message, inner);
		}
		public static WeixinMessageCryptographicException IllegalBuffer(Exception inner = null)
		{
			var message = "解密后得到的Buffer非法";
			return new WeixinMessageCryptographicException(-40008, message, inner);
		}
		public static WeixinMessageCryptographicException EncodeBase64Failed(Exception inner = null)
		{
			var message = "Base64加密异常";
			return new WeixinMessageCryptographicException(-40009, message, inner);
		}
		public static WeixinMessageCryptographicException DecodeBase64Failed(Exception inner = null)
		{
			var message = "Base64解密异常";
			return new WeixinMessageCryptographicException(-40010, message, inner);
		}

	}
}
