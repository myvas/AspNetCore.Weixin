using System;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信API返回值（JSON）
/// </summary>
public partial class WeixinErrorJson : WeixinJson, IWeixinErrorJson
{
    public WeixinErrorJson()
    {
    }

    public WeixinErrorJson(int? code, string msg) : this()
    {
        ErrorCode = code;
        ErrorMessage = msg;
    }

    /// <summary>
    /// 微信错误代码
    /// </summary>
    [JsonPropertyName("errcode")]
    public int? ErrorCode { get; set; }

    /// <summary>
    /// 微信错误描述
    /// </summary>
    [JsonPropertyName("errmsg")]
    public string ErrorMessage { get; set; }

    [JsonIgnore]
    public Exception Exception { get; set; }

    [JsonIgnore]
    public virtual bool Succeeded => (ErrorCode ?? 0) == 0 && Exception is null;
}
