using System;
using Microsoft.AspNetCore.Http;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// Default values related to <see cref="WeixinSiteOptions"/>.
/// </summary>
public static class WeixinSiteOptionsDefaults
{
    /// <summary>
    /// Determines whether enable debug mode to skip the signature verification and User-Agent checking.
    /// <para>default is false</para>
    /// </summary>
    public const bool Debug = false;

    /// <summary>
    /// The path to which the WeixinSite middleware will handle on.
    /// <para>default is "/wx"</para>
    /// </summary>
    public static PathString Path = "/wx";

    /// <summary>
    /// Determines whether strictly require all messages to be encrypted, no plain text messages are allowed.
    /// <para>default is false</para>
    /// </summary>
    public const bool StrictMode = false;

    /// <summary>
    /// default is ClearText
    /// </summary>
    public const string EncryptionMode = EncryptionModes.ClearText;

    /// <summary>
    /// The default max length of Weixin request content, default is 32MB.
    /// </summary>
    public const long MaxRequestContentLength = 1024 * 1024 * 32;

    /// <summary>
    /// The default max length of Weixin response buffer size, default is 32MB.
    /// </summary>
    public const long MaxResponseContentBufferSize = 1024 * 1024 * 32;

    /// <summary>
    /// default is 60 seconds
    /// </summary>
    public static TimeSpan Timeout = TimeSpan.FromSeconds(60);
}