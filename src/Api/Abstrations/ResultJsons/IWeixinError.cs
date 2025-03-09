using System;

namespace Myvas.AspNetCore.Weixin;

public interface IWeixinError
{
    bool Succeeded { get; }

    /// <summary>
    /// 微信错误代码
    /// </summary>
    int? ErrorCode { get; }

    /// <summary>
    /// 微信错误描述
    /// </summary>
    string ErrorMessage { get; }

    #region deprecated
    /// <summary>
    /// 微信错误代码
    /// </summary>
    [Obsolete("Use ErrorCode instead.")]
    int? errcode { get; }


    /// <summary>
    /// 微信错误描述
    /// </summary>
    [Obsolete("Use ErrorMessage instead.")]
    string errmsg { get; }
    #endregion
}