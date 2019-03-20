using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCore.Weixin.Core.Constants
{
	/// <summary>
	/// 公众平台接口域名
	/// </summary>
	/// <remarks>
	/// 开发者可以根据自己的服务器部署情况，选择最佳的接入点（延时更低，稳定性更高）。除此之外，可以将其他接入点用作容灾用途，当网络链路发生故障时，可以考虑选择备用接入点来接入。
	/// </remarks>
	public static class WeixinApiServers
	{
		/// <summary>
		/// 通用域名(api.weixin.qq.com)，使用该域名将访问官方指定就近的接入点；
		/// </summary>
		public const string Default = "api.weixin.qq.com";
		/// <summary>
		/// 通用异地容灾域名(api2.weixin.qq.com)，当上述域名不可访问时可改访问此域名；
		/// </summary>
		public const string Backup = "api2.weixin.qq.com";
		/// <summary>
		/// 上海域名(sh.api.weixin.qq.com)，使用该域名将访问上海的接入点；
		/// </summary>
		public const string Shanghai = "sh.api.weixin.qq.com";
		/// <summary>
		/// 深圳域名(sz.api.weixin.qq.com)，使用该域名将访问深圳的接入点；
		/// </summary>
		public const string Shenzhen = "sz.api.weixin.qq.com";
		/// <summary>
		/// 香港域名(hk.api.weixin.qq.com)，使用该域名将访问香港的接入点。
		/// </summary>
		public const string Hongkong = "hk.api.weixin.qq.com";

		/// <summary>
		/// 公众平台接口域名列表
		/// </summary>
		public static string[] Servers
		{
			get
			{
				return new[] { Default, Backup, Shanghai, Shenzhen, Hongkong };
			}
		}
	}
}
