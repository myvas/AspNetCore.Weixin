namespace Myvas.AspNetCore.Weixin;

public static class EncryptionModes
{
	/// <summary>
	/// 兼容模式，同时支持明文消息和加密消息。
	/// </summary>
	public const string Compatible = "Compatible";

	/// <summary>
	/// 明文模式，只支持明文消息，不加密消息。
	/// </summary>
	public const string ClearText = "ClearText";
	
	/// <summary>
	/// 加密模式，只支持加密模式，不支持明文消息。
	/// </summary>
	public const string AES = "AES";
}
