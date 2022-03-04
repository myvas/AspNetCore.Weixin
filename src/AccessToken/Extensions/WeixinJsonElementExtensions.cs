using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Myvas.AspNetCore.Weixin.Extensions;

internal static class WeixinJsonElementExtensions
{
    public static bool TryGetBoolean(this JsonElement je, out bool result)
    {
        var (p, r) = je.ValueKind switch
        {
            JsonValueKind.True => (true, true),
            JsonValueKind.False => (false, true),
            _ => (default, false)
        };
        result = p;
        return r;
    }

    public static bool TryGetsString(this JsonElement je, out string result)
    {
        var (p, r) = je.ValueKind switch
        {
            JsonValueKind.String => (je.GetString(), true),
            JsonValueKind.Null => (null, true),
            _ => (default, false)
        };
        result = p;
        return r;
    }
}
