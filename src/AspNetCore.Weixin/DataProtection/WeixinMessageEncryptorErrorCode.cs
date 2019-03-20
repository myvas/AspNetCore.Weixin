namespace AspNetCore.Weixin.DataProtection
{
	public enum WeixinMessageEncryptorErrorCode
	{
		Ok = 0,
		ValidateSignatureFailed = -40001,
		ParseXmlFailed = -40002,
		ComputeSignatureFailed = -40003,
		IllegalAesKey = -40004,
		ValidateAppidFailed = -40005,
		AesEncryptFailed = -40006,
		AesDecryptFailed = -40007,
		IllegalBuffer = -40008,
		EncodeBase64Failed = -40009,
		DecodeBase64Failed = -40010
	};
}
