using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin
{
    public static class EncryptionModes
	{
		/// <summary>
		/// ����ģʽ��ͬʱ֧��������Ϣ�ͼ�����Ϣ��
		/// </summary>
		public const string Compatible = "Compatible";
		/// <summary>
		/// ����ģʽ����֧��������Ϣ�����������ܹ�����Ϣ��
		/// </summary>
		public const string ClearText = "ClearText";
		/// <summary>
		/// ����ģʽ����֧�ּ���ģʽ��������������Ϣ��
		/// </summary>
		public const string AES = "AES";
	}
}
