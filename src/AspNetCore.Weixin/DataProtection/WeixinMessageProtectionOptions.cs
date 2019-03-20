namespace AspNetCore.Weixin.DataProtection
{
	public class WeixinMessageProtectionOptions
	{
		public string AppId { get; set; }
		public string WebsiteToken { get; set; }
		public string EncodingAesKey { get; set; }
	}
}
