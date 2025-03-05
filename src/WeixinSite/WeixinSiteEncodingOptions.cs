namespace Myvas.AspNetCore.Weixin
{
    public class WeixinSiteEncodingOptions
    {
        /// <summary>
        /// 采用严格模式，即：所有消息必须加密，不允许明文消息
        /// <para>default is false</para>
        /// </summary>
        public bool StrictMode { get; set; } = false;

        /// <summary>
        /// 消息加解密密钥
        /// </summary>
        public string EncodingAESKey { get; set; }
    }
}
