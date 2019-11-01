using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Myvas.AspNetCore.Weixin
{
    internal static class ObjectToNewtonJsonExtensions
    {
        public static string ToJson(this object source)
        {
            return JsonConvert.SerializeObject(source);
        }

        public static T FromJson<T>(this string source)
        {
            return JsonConvert.DeserializeObject<T>(source);
        }
    }
}
