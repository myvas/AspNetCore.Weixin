using System.Text.Json;
using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Weixin
{
    internal static class ObjectToNewtonJsonExtensions
    {
        public static string ToJson(this object source)
        {
            return JsonSerializer.Serialize(source);
        }

        public static T FromJson<T>(this string source)
        {
            return JsonSerializer.Deserialize<T>(source);
        }
    }
}
