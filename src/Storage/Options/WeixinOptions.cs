using Myvas.AspNetCore.Weixin.Configuration;

namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// The options.
/// </summary>
public class WeixinOptions
{
    /// <summary>
    /// The client id.
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// The secret.
    /// </summary>
    public string AppSecret { get; set; }

    /// <summary>
    /// Gets or sets the caching options.
    /// </summary>
    /// <value>
    /// The caching options.
    /// </value>
    public CachingOptions Caching { get; set; } = new CachingOptions();
}
