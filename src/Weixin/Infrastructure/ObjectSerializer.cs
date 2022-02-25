using System.Text.Json;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin;

internal static class ObjectSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string ToString(object o)
    {
        return JsonSerializer.Serialize(o, Options);
    }

    public static T FromString<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value, Options);
    }
}
