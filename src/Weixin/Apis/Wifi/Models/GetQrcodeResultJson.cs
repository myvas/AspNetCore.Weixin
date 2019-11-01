
namespace AspNetCore.Weixin
{
    /// <summary>
    /// 获取微信WIFI二维码图片的接口返回Json。</summary>
    /// <example>
    /// 正常情况下，微信会返回下述JSON数据包给公众号：
    /// <code>
    /// {"errorCode":0, 
    /// "errorMessage":"", 
    /// "qrCodeUrl": "" #二维码图片的url
    /// }</code>
    /// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
    /// <code>
    /// {"errcode":40013,"errmsg":"invalid appid"}</code></example>
    public class GetQrcodeResultJson : WifiErrorJson
    {
        /// <summary>
        /// 微信WIFI二维码图片地址。
        /// </summary>
        /// <example></example>
        public string qrCodeUrl;
    }
}
