using System;

namespace Myvas.AspNetCore.Weixin
{
    public class WeixinErrorResultException : WeixinException
    {

        public WeixinErrorResultException(string message) : base(message) { }
        public WeixinErrorResultException(string message, Exception inner) : base(message, inner) { }

        public WeixinErrorResultException(string message, Exception inner, WeixinErrorJson errorJson)
            : base(message, inner)
        {
        }
    }
}
