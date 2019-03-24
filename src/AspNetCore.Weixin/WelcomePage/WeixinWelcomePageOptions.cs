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

		public bool WeixinClientAccessOnly { get; set; } = WeixinWelcomePageOptionsDefaults.WeixinClientAccessOnly;

		public string Path { get; set; } = WeixinWelcomePageOptionsDefaults.Path;
		/// <summary>
		/// Specifies which requests paths will be responded to. Exact matches only. Leave null to handle all requests.
		/// </summary>
		public PathString PathString { get { return new PathString(Path); } }

		public string EncodingMode { get; set; } = WeixinMessageEncodingTypes.Compatible;

		public string EncodingAESKey { get; set; }

		public WeixinMessageEvents Events { get; set; }
		public WeixinWelcomePageOptions()
		{
			Events = new WeixinMessageEvents();
		}
	}
}
