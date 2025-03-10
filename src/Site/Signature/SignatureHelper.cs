using System;
using System.Text;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信签名验证
/// </summary>
public class SignatureHelper
{
	/// <summary>
	/// 检查签名是否正确
	/// </summary>
	/// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数</param>
	/// <param name="args">所有参与签名的数据，例如：timestamp时间戳，nonce随机数，websiteToken微信网站特征字，加密消息体Encrypt</param>
	public static bool ValidateSignature(string signature, params string[] args)
	{
		return signature == CalculateSignature(args);
	}

	/// <summary>
	/// 计算签名
	/// </summary>
	/// <param name="args">所有参与签名的数据，例如：timestamp时间戳，nonce随机数，websiteToken微信网站特征字，加密消息体Encrypt</param>
	/// <returns>签名字符串</returns>
	public static string CalculateSignature(params string[] args)
	{
		Array.Sort(args, StringComparer.Ordinal);
		var arrString = string.Join("", args);
		var sha1 = System.Security.Cryptography.SHA1.Create();
		var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
		var hash = BitConverter.ToString(sha1Arr).Replace("-", "");
		hash = hash.ToLower();
		return hash;
	}
}
