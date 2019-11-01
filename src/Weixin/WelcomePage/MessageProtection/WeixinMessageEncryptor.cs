using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
//using System.Web;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
//-40001 ： 签名验证错误
//-40002 :  xml解析失败
//-40003 :  sha加密生成签名失败
//-40004 :  AESKey 非法
//-40005 :  appid 校验错误
//-40006 :  AES 加密失败
//-40007 ： AES 解密失败
//-40008 ： 解密后得到的buffer非法
//-40009 :  base64加密异常
//-40010 :  base64解密异常

namespace AspNetCore.Weixin
{
	public class WeixinMessageEncryptor : IWeixinMessageEncryptor
	{
		private readonly WeixinWelcomePageOptions _options;
		private readonly ILogger _logger;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="websiteToken">公众平台后台由开发者指定的Token</param>
		/// <param name="encodingAesKey">公众平台后台由开发者指定的EncodingAESKey</param>
		/// <param name="appId">公众帐号的appid</param>
		public WeixinMessageEncryptor(IOptions<WeixinWelcomePageOptions> optionsAccessor, ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory?.CreateLogger<WeixinMessageEncryptor>() ?? throw new ArgumentNullException(nameof(loggerFactory));
			_options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
		}

		/// <summary>
		/// 检验消息的真实性，并获取解密后的明文
		/// </summary>
		/// <param name="msg_signature">签名串，对应URL参数的msg_signature</param>
		/// <param name="timestamp">时间戳，对应URL参数的timestamp</param>
		/// <param name="nonce">随机串，对应URL参数的nonce</param>
		/// <param name="data">密文，对应POST请求的数据</param>
		/// <returns>解密后的原文</returns>
		/// <exception cref="WeixinMessageCryptographicException">异常</exception>
		public string Decrypt(string msg_signature, string timestamp, string nonce, string data)
		{
			string result = "";

			if (_options.EncodingAESKey.Length != 43)
			{
				throw WeixinMessageEncryptorError.IllegalAesKey();
			}

			string encryptBody;
			XmlDocument doc = new XmlDocument
			{
				XmlResolver = null
			};
			XmlNode root;
			try
			{
				doc.LoadXml(data);
				root = doc.FirstChild;
				encryptBody = root["Encrypt"].InnerText;
			}
			catch (Exception ex)
			{
				throw WeixinMessageEncryptorError.ParseXmlFailed(ex);
			}

			if (!SignatureHelper.ValidateSignature(msg_signature, _options.WebsiteToken, timestamp, nonce, encryptBody))
			{
				throw WeixinMessageEncryptorError.CalculateSignatureFailed();
			}

			string appId = "";
			try
			{
				result = CryptographyHelper.AesDecrypt(encryptBody, _options.EncodingAESKey, ref appId);
			}
			catch (FormatException ex)
			{
				throw WeixinMessageEncryptorError.DecodeBase64Failed(ex);
			}
			catch (Exception ex)
			{
				throw WeixinMessageEncryptorError.AesDecryptFailed(ex);
			}
			if (appId != _options.AppId)
				throw WeixinMessageEncryptorError.ValidateAppidFailed();

			return result;
		}

		/// <summary>
		/// 将微信号回复用户的消息加密打包
		/// </summary>
		/// <param name="data">微信号待回复用户的消息，xml格式的字符串</param>
		/// <param name="timestamp">时间戳，可以自己生成，也可以用URL参数的timestamp</param>
		/// <param name="nonce">随机串，可以自己生成，也可以用URL参数的nonce</param>
		/// <returns>加密后的可以直接回复用户的密文，包括msg_signature, timestamp, nonce, encrypt的xml格式的字符串</returns>
		/// <exception cref="WeixinMessageCryptographicException">异常</exception>
		public string Encrypt(string data, string timestamp, string nonce)
		{
			string result = "";

			if (string.IsNullOrWhiteSpace(_options.EncodingAESKey))
			{
				return data; //不加密，直接返回原文，以便支持微信开发者测试号
			}

			if (_options.EncodingAESKey?.Length != 43)
			{
				throw WeixinMessageEncryptorError.IllegalAesKey();
			}

			string raw = "";
			try
			{
				raw = CryptographyHelper.AesEncrypt(data, _options.EncodingAESKey, _options.AppId);
			}
			catch (Exception ex)
			{
				throw WeixinMessageEncryptorError.AesEncryptFailed(ex);
			}

			string msg_signature = SignatureHelper.CalculateSignature(_options.WebsiteToken, timestamp, nonce, raw);

			result = $"<xml><Encrypt><![CDATA[{raw}]]></Encrypt><MsgSignature><![CDATA[{msg_signature}]]></MsgSignature><TimeStamp><![CDATA[{timestamp}]]></TimeStamp><Nonce><![CDATA[{nonce}]]></Nonce></xml>";
			return result;
		}
	}
}
