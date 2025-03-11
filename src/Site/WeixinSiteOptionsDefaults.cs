using System;
using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin;

public static class WeixinSiteOptionsDefaults
{
    /// <summary>
    /// default is false
    /// </summary>
    public const bool Debug = false;
    /// <summary>
    /// default is /wx
    /// </summary>
    public static PathString Path = "/wx";

    /// <summary>
    /// default is ClearText
    /// </summary>
    public const string DefaultEncryptionMode = EncryptionModes.ClearText;

    /// <summary>
    /// The default max length of Weixin request content, default is 32MB.
    /// </summary>
    public const int MaxRequestContentLength = 1024 * 1024 * 32;

    /// <summary>
    /// The default max length of Weixin response buffer size, default is 32MB.
    /// </summary>
    public const int DefaultMaxResponseContentBufferSize = 1024 * 1024 * 32;

    /// <summary>
    /// default is 60 seconds
    /// </summary>
    public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60); //60 seconds


}