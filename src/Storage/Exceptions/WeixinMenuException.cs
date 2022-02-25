using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin;

#pragma warning disable 1591

public class WeixinMenuException : WeixinException
{
    public WeixinMenuException(string message)
        : base(message)
    {
    }

    public WeixinMenuException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
