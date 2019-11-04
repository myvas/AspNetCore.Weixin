using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    /// <summary>
    /// 未知请求类型。
    /// </summary>
    public class WeixinUnknownRequestMsgTypeException : WeixinException //ArgumentOutOfRangeException
    {
        public WeixinUnknownRequestMsgTypeException(string message)
            : base(message)
        {
        }

        public WeixinUnknownRequestMsgTypeException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
