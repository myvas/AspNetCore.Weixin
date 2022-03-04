using Microsoft.AspNetCore.Http;
using Myvas.AspNetCore.Weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Options for the WeixinSiteMiddleware.
/// </summary>
/// <remarks>
/// <code>app.UseWeixinSite("/wx");</code>
/// </remarks>
public class WeixinSiteEndpointOptions
{
    /// <summary>
    /// Specifies which requests paths will be responded to.
    /// Exact matches only.
    /// </summary>
    /// <remarks>Default is /wx</remarks>
    public PathString Path { get; set; } = WeixinSiteOptionsDefaults.Path;

}
