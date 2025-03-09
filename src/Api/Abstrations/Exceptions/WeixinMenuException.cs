﻿using System;

namespace Myvas.AspNetCore.Weixin;

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
