using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;

namespace AspNetCore.Weixin.DataProtection
{
	internal static class CryptographyHelper
	{
		public static UInt32 HostToNetworkOrder(UInt32 val)
		{
			UInt32 result = 0;

			for (int i = 0; i < 4; i++)
				result = (result << 8) + ((val >> (i * 8)) & 255);

			return result;
		}

		public static Int32 HostToNetworkOrder(Int32 val)
		{
			Int32 result = 0;

			for (int i = 0; i < 4; i++)
				result = (result << 8) + ((val >> (i * 8)) & 255);

			return result;
		}

		/// <summary>
		/// 解密方法
		/// </summary>
		/// <param name="data">密文</param>
		/// <param name="encodingAesKey"></param>
		/// <returns></returns>
		public static string AesDecrypt(string data, string encodingAesKey, ref string appid)
		{
			byte[] Key;
			Key = Convert.FromBase64String(encodingAesKey + "=");
			byte[] Iv = new byte[16];
			Array.Copy(Key, Iv, 16);
			byte[] btmpMsg = AesDecrypt(data, Iv, Key);

			int len = BitConverter.ToInt32(btmpMsg, 16);
			len = IPAddress.NetworkToHostOrder(len);


			byte[] bMsg = new byte[len];
			byte[] bAppid = new byte[btmpMsg.Length - 20 - len];
			Array.Copy(btmpMsg, 20, bMsg, 0, len);
			Array.Copy(btmpMsg, 20 + len, bAppid, 0, btmpMsg.Length - 20 - len);

			string result = Encoding.UTF8.GetString(bMsg);
			appid = Encoding.UTF8.GetString(bAppid);

			return result;
		}

		public static string AesEncrypt(string data, string encodingAesKey, string appid)
		{
			byte[] Key;
			Key = Convert.FromBase64String(encodingAesKey + "=");
			byte[] Iv = new byte[16];
			Array.Copy(Key, Iv, 16);
			string Randcode = CreateRandCode(16);
			byte[] bRand = Encoding.UTF8.GetBytes(Randcode);
			byte[] bAppid = Encoding.UTF8.GetBytes(appid);
			byte[] btmpMsg = Encoding.UTF8.GetBytes(data);
			byte[] bMsgLen = BitConverter.GetBytes(HostToNetworkOrder(btmpMsg.Length));
			byte[] bMsg = new byte[bRand.Length + bMsgLen.Length + bAppid.Length + btmpMsg.Length];

			Array.Copy(bRand, bMsg, bRand.Length);
			Array.Copy(bMsgLen, 0, bMsg, bRand.Length, bMsgLen.Length);
			Array.Copy(btmpMsg, 0, bMsg, bRand.Length + bMsgLen.Length, btmpMsg.Length);
			Array.Copy(bAppid, 0, bMsg, bRand.Length + bMsgLen.Length + btmpMsg.Length, bAppid.Length);

			return AesEncrypt(bMsg, Iv, Key);

		}

		private static string CreateRandCode(int codeLen)
		{
			string codeSerial = "2,3,4,5,6,7,a,c,d,e,f,h,i,j,k,m,n,p,r,s,t,A,C,D,E,F,G,H,J,K,M,N,P,Q,R,S,U,V,W,X,Y,Z";
			if (codeLen == 0)
			{
				codeLen = 16;
			}
			string[] arr = codeSerial.Split(',');

			string code = "";
			int randValue = -1;
			Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
			for (int i = 0; i < codeLen; i++)
			{
				randValue = rand.Next(0, arr.Length - 1);
				code += arr[randValue];
			}

			return code;
		}

		private static string AesEncrypt(string data, byte[] iv, byte[] key)
		{
			var aes = new RijndaelManaged();
			//秘钥的大小，以位为单位
			aes.KeySize = 256;
			//支持的块大小
			aes.BlockSize = 128;
			//填充模式
			aes.Padding = PaddingMode.PKCS7;
			aes.Mode = CipherMode.CBC;
			aes.Key = key;
			aes.IV = iv;
			var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
			byte[] xBuff = null;

			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
				{
					byte[] xXml = Encoding.UTF8.GetBytes(data);
					cs.Write(xXml, 0, xXml.Length);
				}
				xBuff = ms.ToArray();
			}

			var result = Convert.ToBase64String(xBuff);
			return result;
		}

		private static string AesEncrypt(byte[] data, byte[] iv, byte[] key)
		{
			var aes = new RijndaelManaged();
			//秘钥的大小，以位为单位
			aes.KeySize = 256;
			//支持的块大小
			aes.BlockSize = 128;
			//填充模式
			//aes.Padding = PaddingMode.PKCS7;
			aes.Padding = PaddingMode.None;
			aes.Mode = CipherMode.CBC;
			aes.Key = key;
			aes.IV = iv;
			var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
			byte[] xBuff = null;

			#region 自己进行PKCS7补位，用系统自己带的不行
			byte[] msg = new byte[data.Length + 32 - data.Length % 32];
			Array.Copy(data, msg, data.Length);
			byte[] pad = KCS7Encoder(data.Length);
			Array.Copy(pad, 0, msg, data.Length, pad.Length);
			#endregion

			#region 注释的也是一种方法，效果一样
			//ICryptoTransform transform = aes.CreateEncryptor();
			//byte[] xBuff = transform.TransformFinalBlock(msg, 0, msg.Length);
			#endregion

			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
				{
					cs.Write(msg, 0, msg.Length);
				}
				xBuff = ms.ToArray();
			}

			var result = Convert.ToBase64String(xBuff);
			return result;
		}

		private static byte[] KCS7Encoder(int textLen)
		{
			int block_size = 32;
			// 计算需要填充的位数
			int amount_to_pad = block_size - (textLen % block_size);
			if (amount_to_pad == 0)
			{
				amount_to_pad = block_size;
			}
			// 获得补位所用的字符
			char pad_chr = chr(amount_to_pad);
			string tmp = "";
			for (int index = 0; index < amount_to_pad; index++)
			{
				tmp += pad_chr;
			}
			return Encoding.UTF8.GetBytes(tmp);
		}

		/// <summary>
		/// 将数字转化成ASCII码对应的字符，用于对明文进行补码
		/// </summary>
		/// <param name="n">需要转化的数字</param>
		/// <returns>转化得到的字符</returns>
		static char chr(int n)
		{

			byte target = (byte)(n & 0xFF);
			return (char)target;
		}

		private static byte[] AesDecrypt(string data, byte[] iv, byte[] key)
		{
			RijndaelManaged aes = new RijndaelManaged();
			aes.KeySize = 256;
			aes.BlockSize = 128;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.None;
			aes.Key = key;
			aes.IV = iv;
			var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
			byte[] xBuff = null;
			using (var ms = new MemoryStream())
			{
				using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
				{
					byte[] xXml = Convert.FromBase64String(data);
					byte[] msg = new byte[xXml.Length + 32 - xXml.Length % 32];
					Array.Copy(xXml, msg, xXml.Length);
					cs.Write(xXml, 0, xXml.Length);
				}
				xBuff = Decode2(ms.ToArray());
			}
			return xBuff;
		}

		private static byte[] Decode2(byte[] decrypted)
		{
			int pad = (int)decrypted[decrypted.Length - 1];
			if (pad < 1 || pad > 32)
			{
				pad = 0;
			}

			byte[] result = new byte[decrypted.Length - pad];
			Array.Copy(decrypted, 0, result, 0, decrypted.Length - pad);

			return result;
		}
	}
}
