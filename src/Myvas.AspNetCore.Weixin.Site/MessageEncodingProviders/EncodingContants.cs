using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{
    public static class EncodingContants
	{
		/// <summary>
		/// 兼容模式：同时支持明文消息和加密消息。
		/// </summary>
		public const string Compatible = "Compatible";
		/// <summary>
		/// 明文模式：仅支持明文消息，将丢弃加密过的消息。
		/// </summary>
		public const string ClearText = "ClearText";
		/// <summary>
		/// 加密模式：仅支持加密模式，将丢弃明文消息。
		/// </summary>
		public const string AesEncrypted = "AesEncrypted";
	}
}
