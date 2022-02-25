using Myvas.AspNetCore.Weixin.AccessTokenServer.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.Weixin.Extensions;

public static class PersistedTokenFilterExtensions
{
    public static void Validate(this PersistedTokenFilter filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        if (String.IsNullOrWhiteSpace(filter.AppId))
        {
            throw new ArgumentException("No filter values set.", nameof(filter));
        }
    }
}
