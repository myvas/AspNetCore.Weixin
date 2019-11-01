namespace AspNetCore.Weixin
{
	public interface IWeixinJsapiTicket
	{
		/// <summary>
		/// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
		/// </summary>
		/// <returns>微信JSSDK访问凭证</returns>
		string GetTicket();

		/// <summary>
		/// 获取微信JSSDK访问凭证。仅在需要时调用微信JSSDK接口，即：若凭证尚在有效期内，则直接取回上一次得到的凭证。
		/// </summary>
		/// <param name="forceRenew">强制立即取新。这将废弃并替换旧的，而不管是否旧的令牌是否过期。</param>
		/// <returns>微信JSSDK访问凭证</returns>
		string GetTicket(bool forceRenew);
	}
}