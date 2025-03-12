using System;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信API返回值（JSON）
/// </summary>
public class WeixinErrorJson : WeixinJson, IWeixinError
{
    public WeixinErrorJson()
    {
    }

    public WeixinErrorJson(int? code, string msg) : this()
    {
        ErrorCode = code;
        ErrorMessage = msg;
    }

    [JsonIgnore]
    public virtual bool Succeeded { get { return !ErrorCode.HasValue || ErrorCode!.Value == WeixinErrorCodes.OK; } }

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

    #region deprecated
    [Obsolete("Use ErrorCode instead.")]
    [JsonIgnore]
    public int? errcode { get => ErrorCode; }

    [Obsolete("Use ErrorMessage instead.")]
    [JsonIgnore]
    public string errmsg { get => ErrorMessage; }
    #endregion
}
