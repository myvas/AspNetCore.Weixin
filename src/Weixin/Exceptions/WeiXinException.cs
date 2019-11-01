using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 微信框架异常类
    /// </summary>
    public class WeixinException : Exception
    {

        #region 自定义：JSON错误返回值接口
        /// <summary>
        /// 微信错误返回值
        /// </summary>
        public WeixinErrorJson ErrorJson { get; private set; }

        public WeixinException(WeixinErrorJson errorJson)
            : base(errorJson.errmsg)
        {
            ErrorJson = errorJson;
        }

        public WeixinException(Exception innerException, WeixinErrorJson errorJson)
            : base(errorJson.errmsg, innerException)
        {
            ErrorJson = errorJson;
        }
        #endregion

        #region 标准接口
        public WeixinException(string message)
            : base(message)
        {
            ErrorJson = new WeixinErrorJson();
            ErrorJson.errcode = WeixinResponseStatus.CustomError;
            ErrorJson.errmsg = message;
        }

        public WeixinException(string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorJson = new WeixinErrorJson();
            ErrorJson.errcode = WeixinResponseStatus.CustomError;
            ErrorJson.errmsg = message;
        }
        
        #endregion

        #region 标准接口带形参
        public WeixinException(string format, params object[] args)
            : base(string.Format(format, args))
        {
            ErrorJson = new WeixinErrorJson();
            ErrorJson.errcode = WeixinResponseStatus.CustomError;
            ErrorJson.errmsg = string.Format(format, args);
        }

        public WeixinException(Exception innerException, string format, params object[] args)
            : base(string.Format(format, args), innerException)
        {
            ErrorJson = new WeixinErrorJson();
            ErrorJson.errcode = WeixinResponseStatus.CustomError;
            ErrorJson.errmsg = string.Format(format, args);
        }

        #endregion
    }
}
