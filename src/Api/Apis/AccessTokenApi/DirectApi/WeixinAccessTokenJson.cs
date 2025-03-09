using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信全局访问票据数据。调用微信API接口时必须。</summary>
/// <example>
/// 正常情况下，微信会返回下述JSON数据包给公众号：
/// <code>
/// {"access_token":"ACCESS_TOKEN","expires_in":7200}</code>
/// 错误时微信会返回错误码等信息，JSON数据包示例如下（该示例为AppID无效错误）:
/// <code>
/// {"errcode":40013,"errmsg":"invalid appid"}</code></example>
/// <remarks>
/// <list type="bullet"><item>
/// 调用所有微信接口时均需使用https协议。
/// </item><item>
/// 获取access_token的api调用频率限制：每日限额2000次（测试号为200次）。
/// </item><item>
/// 重复获取将导致上次获取的access_token失效。
/// </item></list></remarks>
public class WeixinAccessTokenJson : WeixinErrorJson
{
    /// <summary>
    /// 微信公众号全局唯一票据。存储空间需要512个字符或以上。
    /// </summary>
    /// <example>XkC7gqcx7pctuGe5zPIita23N7dJKIfkwz_2ULReV_Pn7T09lMyhTwlgGK5cghtqGQlPUlZ7ur_nMqhGuJNXwA</example>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// 凭证有效时间。单位：秒。
    /// <para>例如：7200</para>
    /// </summary>
    /// <example></example>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    public override bool Succeeded => base.Succeeded && !string.IsNullOrEmpty(AccessToken) && ExpiresIn > 0;
}
