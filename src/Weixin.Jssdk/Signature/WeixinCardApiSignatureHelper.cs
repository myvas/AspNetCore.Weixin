using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
	public static class WeixiCardApiSignatureHelper
	{
		/// <summary>
		/// 微信卡券签名
		/// <para><pre>
		/// 1.所有参与签名的字段值（字符串）排序（字典序）
		/// 2.拼接这些值
		/// 3.SHA1
		/// </pre></para>
		/// <paramref name="args">参与签名的字段，有：code,openid, 以及timestamp,nonce_str，以及api_ticket,card_id</>
		/// </summary>
		public static string CalculateCardApiSignature(params string[] args)
		{
			//Array.Sort(args, StringComparer.Ordinal);
			//var arrString = string.Join("", args);
			//var sha1 = System.Security.Cryptography.SHA1.Create();
			//var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
			//var hash = BitConverter.ToString(sha1Arr).Replace("-", "");
			//hash = hash.ToLower();
			//return hash;

			return SignatureHelper.CalculateSignature(args);
		}
	}
}
