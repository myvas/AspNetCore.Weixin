using System;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// 微信框架异常类
/// </summary>
public class WeixinException : Exception
{
    /// <summary>
    /// 微信错误
    /// </summary>
    public IWeixinError ErrorJson { get; private set; }
    public int? ErrorCode { get { return ErrorJson?.ErrorCode; } }
    public string ErrorMessage { get { return ErrorJson?.ErrorMessage; } }

    protected WeixinException()
    {
    }

    public WeixinException(IWeixinError errorJson)
        : base(errorJson.ErrorMessage)
    {
        ErrorJson = errorJson;
    }


    public WeixinException(IWeixinError errorJson, Exception innerException)
        : base(errorJson.ErrorMessage, innerException)
    {
        ErrorJson = errorJson;
    }

    public WeixinException(int errorCode, string errorMessage, Exception innerException)
        : base(errorMessage, innerException)
    {
        ErrorJson = new WeixinErrorJson(errorCode, errorMessage);
    }

    public WeixinException(string message)
        : this(new WeixinErrorJson(WeixinErrorCodes.CustomError, message)) { }

    public WeixinException(string message, Exception innerException)
        : this(new WeixinErrorJson(WeixinErrorCodes.CustomError, message), innerException)
    {
    }

    public WeixinException(string format, params object[] args)
        : this(string.Format(format, args))
    {
    }

    public WeixinException(Exception innerException, string format, params object[] args)
        : this(string.Format(format, args), innerException)
    {
    }
}
