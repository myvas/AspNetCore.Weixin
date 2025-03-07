using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Services;

/// <summary>
/// Service to provide CancellationToken for async operations.
/// </summary>
public interface ICancellationTokenProvider
{
    /// <summary>
    /// Returns the current CancellationToken, or null if none present.
    /// </summary>
    CancellationToken CancellationToken { get; }
}
