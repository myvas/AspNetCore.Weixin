using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Configuration;

/// <summary>
/// Caching options.
/// </summary>
public class CachingOptions
{
    private static readonly TimeSpan Default = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Gets or sets the client store expiration.
    /// </summary>
    /// <value>
    /// The client store expiration.
    /// </value>
    public TimeSpan ClientStoreExpiration { get; set; } = Default;

    /// <summary>
    /// Gets or sets the scope store expiration.
    /// </summary>
    /// <value>
    /// The scope store expiration.
    /// </value>
    public TimeSpan ResourceStoreExpiration { get; set; } = Default;

    /// <summary>
    /// Gets or sets the CORS origin expiration.
    /// </summary>
    public TimeSpan CorsExpiration { get; set; } = Default;

    /// <summary>
    /// Duration identity provider store cache duration
    /// </summary>
    public TimeSpan IdentityProviderCacheDuration { get; set; } = TimeSpan.FromMinutes(60);


    /// <summary>
    /// The timeout for concurrency locking in the default cache.
    /// </summary>
    public TimeSpan CacheLockTimeout { get; set; } = TimeSpan.FromSeconds(60);
}
