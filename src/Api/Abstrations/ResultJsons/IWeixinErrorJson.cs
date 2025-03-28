using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinErrorJson
{
    [XmlIgnore]
    [JsonIgnore]
    bool Succeeded { get; }

    /// <summary>
    /// 微信错误代码
    /// </summary>
    int? ErrorCode { get; set; }

    /// <summary>
    /// 微信错误描述
    /// </summary>
    string ErrorMessage { get; set; }

    [XmlIgnore]
    [JsonIgnore]
    Exception Exception { get; set; }

    #region deprecated
    /// <summary>
    /// 微信错误代码
    /// </summary>
    [Obsolete("Use ErrorCode instead.")]
    [XmlIgnore]
    [JsonIgnore]
    int? errcode { get; }

    /// <summary>
    /// 微信错误描述
    /// </summary>
    [Obsolete("Use ErrorMessage instead.")]
    [XmlIgnore]
    [JsonIgnore]
    string errmsg { get; }
    #endregion
}