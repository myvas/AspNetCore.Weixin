using Myvas.AspNetCore.Weixin;
using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{
	/// <summary>
	/// Options for the WeixinWelcomePageMiddleware.
	/// </summary>
	public class WeixinWelcomePageOptions : WeixinAccessTokenOptions
	{
		/// <summary>
		/// 网站令牌(Token)，用于验证开发者服务器。
		/// </summary>
		public string WebsiteToken { get; set; }

		/// <summary>
		/// 消息加解密密钥
		/// </summary>
		public string EncodingAESKey { get; set; }

		/// <summary>
		/// 是否允许微信web开发工具等调试终端访问，默认为: false（不允许）。
		/// </summary>
		public bool Debug { get; set; } = WeixinWelcomePageOptionsDefaults.Debug;

		/// <summary>
		/// 服务器地址路径，默认为: /wx
		/// </summary>
		public string Path { get; set; } = WeixinWelcomePageOptionsDefaults.Path;

		/// <summary>
		/// 接收微信消息或事件
		/// </summary>
		public WeixinMessageEvents Events { get; set; }

		public WeixinWelcomePageOptions()
		{
			Events = new WeixinMessageEvents();
		}
	}
}
