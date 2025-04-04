﻿using System;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信WiFi相关API返回值（JSON）
/// </summary>
/// <remarks>注意：微信WiFi相关API与微信基础API的JSON属性名不一样!</remarks>
public class WeixinWifiErrorJson : WeixinJson, IWeixinErrorJson
{
    public WeixinWifiErrorJson()
    {
    }

    public WeixinWifiErrorJson(int? code, string msg) : this()
    {
        ErrorCode = code;
        ErrorMessage = msg;
    }

    [JsonIgnore]
    public virtual bool Succeeded => (ErrorCode ?? 0) == 0 && Exception is null;

    /// <summary>
    /// 微信错误代码
    /// </summary>
    //[JsonPropertyName("errcode")]
    [JsonPropertyName("errorCode")]
    public int? ErrorCode { get; set; }

    /// <summary>
    /// 微信错误描述
    /// </summary>
    //[JsonPropertyName("errmsg")]
    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; set; }

    #region deprecated
    [Obsolete("Use ErrorCode instead.")]
    [JsonIgnore]
    public int? errcode { get => ErrorCode; }

    [Obsolete("Use ErrorMessage instead.")]
    [JsonIgnore]
    public string errmsg { get => ErrorMessage; }

    [JsonIgnore]
    public Exception Exception { get; set; }
    #endregion
}
