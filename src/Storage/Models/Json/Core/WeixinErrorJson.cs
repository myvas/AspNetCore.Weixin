using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin;

#pragma warning disable 1591

/// <summary>
/// 微信API返回值（JSON）
/// </summary>
public class WeixinErrorJson : WeixinJson
{
    public virtual bool Succeeded { get { return !ErrorCode.HasValue || ErrorCode == WeixinResponseStatus.OK; } }

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
}
