using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Storage.Extensions;

/// <summary>
/// 
/// </summary>
public static class UnsubscribeEventReceivedEntryFilterExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void Validate(this UnsubscribeEventReceivedEntryFilter filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        if (String.IsNullOrWhiteSpace(filter.OpenId))
        {
            throw new ArgumentException("No filter values set.", nameof(filter));
        }
    }
}
