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

namespace AspNetCore.Weixin.DataProtection
{
	public class WeixinMessageEncryptor : IWeixinMessageEncryptor
	{
		private readonly WeixinMessageProtectionOptions _options;
		private readonly ILogger _logger;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="websiteToken">公众平台后台由开发者指定的Token</param>
		/// <param name="encodingAesKey">公众平台后台由开发者指定的EncodingAESKey</param>
		/// <param name="appId">公众帐号的appid</param>
		public WeixinMessageEncryptor(IOptions<WeixinMessageProtectionOptions> optionsAccessor, ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory?.CreateLogger<WeixinMessageEncryptor>() ?? throw new ArgumentNullException(nameof(loggerFactory));
			_options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
		}

		/// <summary>
		/// 检验消息的真实性，并获取解密后的明文
		/// </summary>
		/// <param name="signature">签名串，对应URL参数的msg_signature</param>
		/// <param name="timestamp">时间戳，对应URL参数的timestamp</param>
		/// <param name="nonce">随机串，对应URL参数的nonce</param>
		/// <param name="data">密文，对应POST请求的数据</param>
		/// <returns>解密后的原文</returns>
		/// <exception cref="WeixinMessageCryptographicException">异常</exception>
		public string Decrypt(string signature, string timestamp, string nonce, string data)
		{
			string result = "";

			if (_options.EncodingAESKey.Length != 43)
			{
				throw WeixinMessageEncryptorError.IllegalAesKey();
			}

			XmlDocument doc = new XmlDocument();
			doc.XmlResolver = null;
			XmlNode root;
			string sEncryptMsg;
			try
			{
				doc.LoadXml(data);
				root = doc.FirstChild;
				sEncryptMsg = root["Encrypt"].InnerText;
			}
			catch (Exception ex)
			{
				throw WeixinMessageEncryptorError.ParseXmlFailed(ex);
			}

			//verify signature
			bool ret = VerifySignature(_options.WebsiteToken, timestamp, nonce, sEncryptMsg, signature);

			//decrypt
			string cpid = "";
			try
			{
				result = CryptographyHelper.AesDecrypt(sEncryptMsg, _options.EncodingAESKey, ref cpid);
			}
			catch (FormatException ex)
			{
				throw WeixinMessageEncryptorError.DecodeBase64Failed(ex);
			}
			catch (Exception ex)
			{
				throw WeixinMessageEncryptorError.AesDecryptFailed(ex);
			}
			if (cpid != _options.AppId)
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

			if (_options.EncodingAESKey.Length != 43)
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
			string MsgSigature = CalculateSignature(_options.WebsiteToken, timestamp, nonce, raw);

			result = "";
			string EncryptLabelHead = "<Encrypt><![CDATA[";
			string EncryptLabelTail = "]]></Encrypt>";
			string MsgSigLabelHead = "<MsgSignature><![CDATA[";
			string MsgSigLabelTail = "]]></MsgSignature>";
			string TimeStampLabelHead = "<TimeStamp><![CDATA[";
			string TimeStampLabelTail = "]]></TimeStamp>";
			string NonceLabelHead = "<Nonce><![CDATA[";
			string NonceLabelTail = "]]></Nonce>";
			result = result + "<xml>" + EncryptLabelHead + raw + EncryptLabelTail;
			result = result + MsgSigLabelHead + MsgSigature + MsgSigLabelTail;
			result = result + TimeStampLabelHead + timestamp + TimeStampLabelTail;
			result = result + NonceLabelHead + nonce + NonceLabelTail;
			result += "</xml>";
			return result;
		}

		private static bool VerifySignature(string token, string timestamp, string nonce, string data, string signature)
		{
			string hash = CalculateSignature(token, timestamp, nonce, data);
			if (hash != signature)
			{
				throw WeixinMessageEncryptorError.ValidateSignatureFailed();
			}

			return true;
		}

		public static string CalculateSignature(string token, string timestamp, string nonce, string data)
		{
			string result = "";

			ArrayList AL = new ArrayList();
			AL.Add(token);
			AL.Add(timestamp);
			AL.Add(nonce);
			AL.Add(data);
			AL.Sort(new WeixinMessageDictionarySort());
			string raw = "";
			for (int i = 0; i < AL.Count; ++i)
			{
				raw += AL[i];
			}

			SHA1 sha;
			ASCIIEncoding enc;
			string hash = "";
			try
			{
				sha = new SHA1CryptoServiceProvider();
				enc = new ASCIIEncoding();
				byte[] dataToHash = enc.GetBytes(raw);
				byte[] dataHashed = sha.ComputeHash(dataToHash);
				hash = BitConverter.ToString(dataHashed).Replace("-", "");
				hash = hash.ToLower();
			}
			catch (Exception ex)
			{
				throw WeixinMessageEncryptorError.CalculateSignatureFailed(ex);
			}

			result = hash;
			return result;
		}
	}
}
