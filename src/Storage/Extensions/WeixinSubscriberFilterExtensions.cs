using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Storage.Extensions;

#pragma warning disable 1591

public static class WeixinSubscriberFilterExtensions
{
    public static void Validate(this WeixinSubscriberFilter filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        if (String.IsNullOrWhiteSpace(filter.OpenId))
        {
            throw new ArgumentException("No filter values set.", nameof(filter));
        }
    }
}
