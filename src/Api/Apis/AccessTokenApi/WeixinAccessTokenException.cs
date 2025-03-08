using System;

namespace Myvas.AspNetCore.Weixin;

public class WeixinAccessTokenException : WeixinException
{
    public WeixinAccessTokenException(IWeixinError errorJson) : base(errorJson)
    {
    }

    public WeixinAccessTokenException(int errorCode, string errorMessage, Exception innerException) : base(errorCode, errorMessage, innerException)
    {
    }

    protected WeixinAccessTokenException()
    {
    }
}
