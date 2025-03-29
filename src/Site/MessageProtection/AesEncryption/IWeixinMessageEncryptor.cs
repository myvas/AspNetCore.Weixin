namespace Myvas.AspNetCore.Weixin;

public interface IWeixinMessageEncryptor
{
	string Decrypt(string msg_signature, string timestamp, string nonce, string data);
	string Encrypt(string data, string timestamp, string nonce);
}