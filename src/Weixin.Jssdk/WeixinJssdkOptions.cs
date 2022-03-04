namespace Myvas.AspNetCore.Weixin;

/// <summary>
/// The options.
/// </summary>
public class WeixinJssdkOptions
{
    /// <summary>
    /// The constructor.
    /// </summary>
    public WeixinJssdkOptions() { }

    /// <summary>
    /// Copy from <see cref="WeixinOptions"/>
    /// </summary>
    /// <param name="options">The <see cref="WeixinOptions"/></param>
    public WeixinJssdkOptions(WeixinOptions options)
    {
        AppId = options.AppId;
    }

    public string AppId { get; set; }
}
