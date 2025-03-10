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
    /// default is 10 MB
    /// </summary>
    public const int DefaultMaxResponseContentBufferSize = 1024 * 1024 * 10; //10 MB

    /// <summary>
    /// default is 60 seconds
    /// </summary>
    public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(60); //60 seconds

    /// <summary>
    /// default is ClearText
    /// </summary>
    public const string DefaultEncryptionMode = EncryptionModes.ClearText;

}