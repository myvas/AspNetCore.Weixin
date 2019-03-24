using AspNetCore.Weixin;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Weixin
{
	/// <summary>
	/// Options for the WeixinWelcomePageMiddleware.
	/// </summary>
	public class WeixinWelcomePageOptions : WeixinAccessTokenOptions
	{
		/// <summary>
		/// 用于验证开发者服务器
		/// </summary>
		public string WebsiteToken { get; set; }

		public bool Debug { get; set; } = WeixinWelcomePageOptionsDefaults.Debug;

		public string Path { get; set; } = WeixinWelcomePageOptionsDefaults.Path;

		public string EncodingAESKey { get; set; }

		public WeixinMessageEvents Events { get; set; }

		public WeixinWelcomePageOptions()
		{
			Events = new WeixinMessageEvents();
		}
	}
}
