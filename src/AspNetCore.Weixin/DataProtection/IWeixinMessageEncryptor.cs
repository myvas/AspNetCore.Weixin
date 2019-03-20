namespace AspNetCore.Weixin.DataProtection
{
	public interface IWeixinMessageEncryptor
	{
		string Decrypt(string signature, string timestamp, string nonce, string data);
		string Encrypt(string data, string timestamp, string nonce);
	}
}