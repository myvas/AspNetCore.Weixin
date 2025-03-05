using System;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 微信框架异常类
    /// </summary>
    public class WeixinException : Exception
    {
        /// <summary>
        /// 微信错误
        /// </summary>
        public IWeixinError ErrorJson { get; private set; }
        public int? ErrorCode { get { return ErrorJson?.errcode; } }
        public string ErrorMessage { get { return ErrorJson?.errmsg; } }

        protected WeixinException()
        {
        }

        public WeixinException(IWeixinError errorJson)
            : base(errorJson.errmsg)
        {
            ErrorJson = errorJson;
        }


        public WeixinException(IWeixinError errorJson, Exception innerException)
            : base(errorJson.errmsg, innerException)
        {
            ErrorJson = errorJson;
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
}
