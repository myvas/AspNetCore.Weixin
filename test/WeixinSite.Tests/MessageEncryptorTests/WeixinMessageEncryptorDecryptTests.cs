using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.Weixin.Site.Tests.TestServers;
using System.Diagnostics;

namespace Myvas.AspNetCore.Weixin.Site.Tests.MessageEncryptorTests;

public class WeixinMessageEncryptorDecryptTests
{
	TestServer testServer;

	public WeixinMessageEncryptorDecryptTests()
	{
		testServer = TestServerBuilder.CreateServer(app =>
		{
			app.UseWeixinSite();
		}, services =>
		{
			services.AddWeixin(options =>
			{
				options.AppId = "wxaf5aa2d87ff3b700";
				options.AppSecret = "USELESS_IN_THIS_TEST";
			})
			.AddWeixinSite(options =>
			{
				options.WebsiteToken = "MdPhLRFuJ9X48WWQDHJA3nxIK";
			})
			.AddMessageProtection(options =>
			{
				options.EncodingAESKey = "5o7tcB4nbWtcX76QyF1fi90FBt4ZxFD8N6oND0tHVa4";
			});
		}, null);
	}

	[Fact]
	public void ReceivedEventArgs_Compatible()
	{
		var _encryptor = testServer.Services.GetRequiredService<IWeixinMessageEncryptor>();

		var xml = @" <xml>
<ToUserName><![CDATA[gh_08dc1481d8cc]]></ToUserName>
<FromUserName><![CDATA[oEylh5ksJ9m44qL7nXvHu0npUTww]]></FromUserName>
<CreateTime>1553425488</CreateTime>
<MsgType><![CDATA[text]]></MsgType>
<Content><![CDATA[好]]></Content>
<MsgId>22239703566971011</MsgId>
<Encrypt><![CDATA[jF3WhwHgtyZ95HvHcWqaRLlbr0OkTNPY4ngqrbyZqyhoMWt1PnTnyXruXTdDxOlzVHdS3e1SPzneMNVInqPRWT9DkQuPuud3rJHv9VQc4efnQdGhtVXP5h2XXXqEVcDKTlaJj5fqcECF+KURkfAM+zlZI3PdqgsAq1v1sHsm2FeM8SeQ4IVlzJvPIiLVyjSxXJB4e0Wzy3VRqS5L4FWHzyEcEs90D0YmeIdHEsUjzcvEhX46MiAJyyzf8lq6GnuxxFFuAieUNLgVV7Y+LjhFY2ylNw8qI3/f/dL3hkm8Qr+IwYOvofqPVMVTgGcrTDTexvpRg8+gALeMGNYsDtjUpYKHeT02hZwFSRM/Bz4aXJ3AFiuL8pbC6u0WbFkfz0D+IIHRqKwnsqwFs97KlemI5Aa+8avcOWyuuqkxzE8L8Ic=]]></Encrypt>
</xml>";

		// HTTP/1.0 POST http://wx.steamlet.com/wx?signature=77b27c5651d7ec874f87d9ec7f953b922810f476&timestamp=1553425488&nonce=580423986&openid=oEylh5ksJ9m44qL7nXvHu0npUTww&encrypt_type=aes&msg_signature=a76bb4d3348628369b38e8fecebf5f0545d48786 text/xml 764
		var timestamp = "1553425488";
		var nonce = "580423986";
		var msg_signature = "a76bb4d3348628369b38e8fecebf5f0545d48786";
		var encrypt_type = "aes";

		var received = WeixinXmlConvert.DeserializeObject<ReceivedXml>(xml);
		if (encrypt_type == "aes")
		{
			var decryptedXml = _encryptor.Decrypt(msg_signature, timestamp, nonce, xml);
			Debug.WriteLine("Request Body[Decrypted]({0}): {1}", decryptedXml?.Length, decryptedXml);

			xml = decryptedXml;
		}

		var result = WeixinXmlConvert.DeserializeObject<ReceivedXml>(xml);
		Assert.Equal("gh_08dc1481d8cc", result.ToUserName);
		Assert.Equal(RequestMsgType.text, result.MsgTypeAsEnum());
	}

	[Fact]
	public void ReceivedEventArgs_aes()
	{
		var _encryptor = testServer.Services.GetRequiredService<IWeixinMessageEncryptor>();

		var xml = @"<xml>
<ToUserName><![CDATA[gh_08dc1481d8cc]]></ToUserName>
<Encrypt><![CDATA[+4+tcgOSZlVj8aW3DpoKQzIuZm9UxFT8f5O1bqX7SIF5FxhMlqSaWZ8Gqq1pIK4Web0nQQ8VxMM12DqDB8M77GdcvjAHfJbO6qkXFpb9z+8bSik8G1QQUzYMyWrZby1DAH+cZov6sMnJzdCWYRJ/TL4pxuDFlfxIRomIZxvq74CEyJ5TnuJ3LGor1n9ZJ3LJU7Av7zJrcdHBYdG12uz2dHddqBNiaeOrnNLCNHmrbJDSwz0p2QlBtthtH0WXG1QwPLBFoDFLAjVwFMFUh3+B2Nk8RT1rgFQ7CIItRr6vWCUwh9rifBJFA4b0Js24Y5Sd0/ZIPUzwZeeCpxL6iTsLHgEcm7DxKU2v/IVYt6ABR4kyk7myHbMIlY5gCbk7JBlGjW1fCXlOF0cOTxh1/Ar7Ombx/zBhOG5+O4gPGFZGMAI=]]></Encrypt>
</xml>";

		// http://wx.steamlet.com/wx?signature=51631d92ac7c9e7abb663ec2d5d7399e175ecc0d&timestamp=1553419390&nonce=1180353179&openid=oEylh5ksJ9m44qL7nXvHu0npUTww&encrypt_type=aes&msg_signature=6b09992cbda171f56d43698acbee5c8667056e18
		var timestamp = "1553419390";
		var nonce = "1180353179";
		var msg_signature = "6b09992cbda171f56d43698acbee5c8667056e18";
		var encrypt_type = "aes";

		if (encrypt_type == "aes")
		{
			var decryptedXml = _encryptor.Decrypt(msg_signature, timestamp, nonce, xml);
			Debug.WriteLine("Request Body[Decrypted]({0}): {1}", decryptedXml?.Length, decryptedXml);

			xml = decryptedXml;
		}

		var result = WeixinXmlConvert.DeserializeObject<ReceivedXml>(xml);
		Assert.Equal("gh_08dc1481d8cc", result.ToUserName);
		Assert.Equal(RequestMsgType.text, result.MsgTypeAsEnum());
	}
}
